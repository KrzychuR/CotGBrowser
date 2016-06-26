using log4net;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DB
{
    /// <summary>
    /// Implementacja podstawowych metod umożliwiających prawidłową 
    /// konfigurację NHibernate, obsługę wyjątków i diagnostykę.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Metoda do ew. pokrycia w klasach potomnych. Musi zwrócić listę dll-lek z których mam załadować 
        /// mapowanie obiektów NH, potrzebne do FluentNHibernate
        /// </summary>
        public virtual IEnumerable<Assembly> GetAssembliesWithMappings()
        {
            List<Assembly> l = new List<Assembly>();
            l.Add(this.GetType().Assembly);
            return l;
        }

        /// <summary>
        /// Obiekt, którego zadaniem jest dostarczenie metod specyficznych dla danego silnika SQL
        /// </summary>
        [Dependency]
        public DialectHelper CurrenctDialectHelper { get; set; }

        #region SessionFactory

        /// <summary>
        /// Fabryka sesji/połączeń, cache aby nie tworzyć za każdym razem
        /// </summary>
        private ISessionFactory m_SessionFactory;

        /// <summary>
        /// Fabryka sesji/połączeń
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get
            {
                if (m_SessionFactory == null)
                    m_SessionFactory = CurrenctDialectHelper.CreateSessionFactory(this);

                return m_SessionFactory;
            }

            protected set
            {
                m_SessionFactory = value;
            }
        }

        #endregion

        /// <summary>
        /// Uruchomienie na bazie pewnej porcji pracy (NHUnitOfWork)
        /// </summary>
        /// <remarks>
        /// Metoda domyślnie może być wywołana tylko z parametrem work, wtedy zostanie automatycznie założona
        /// sesja i trnsakacja. 
        /// Dodatkowo aby ułatwić testy można podać jako parametr własną sesję. Jeżeli zostanie podana własna sesja
        /// to w zależności czy jest w niej transakcja (raczej powinna już być) zostanie założona lub nie.
        /// 
        /// </remarks>
        /// <param name="work">Zadania do wykonania</param>
        /// <param name="session">Opcjonalna sesja z trnskcją, jeżeli nie chcemy aby metoda założyła własną</param>
        public void ExecuteWork(IUnitOfWork work, ISession session = null)
        {
            INHUnitOfWork nhWork = work as INHUnitOfWork;

            if (nhWork == null)
                throw new ArgumentException("Zadanie musi implementować INHUnitOfWork", "work");

            //licznik deadlocków, jak za dużo to się poddaję 
            int deadlockCnt = 0;

            //skrót do konfiguracji
            Properties.Settings cfg = Properties.Settings.Default;

            //próbuję wykonać pracę, jeżęli wystąpi deadlock i jest to moja sesja... to 
            //staram się ponowić N-razy pracę
            while (true)
            {
                try
                {
                    TryExecuteWork(nhWork, session);
                    return;
                }
                catch (GenericADOException e)
                {
                    if (CurrenctDialectHelper.IsDeadlock(e) && session == null && deadlockCnt < cfg.MaxInterceptedDeadlocks)
                    {
                        Log.Warn("Deadlock intercepted: ", e);

                        //1. Wystapił deadlock...
                        //2. Jest to moja sesja więc mogę założyć ją od nowa, oraz transakcję
                        //3. Nie wyczerpałem limitu przechwyceń...
                        Random _r = new Random();

                        //usypiam na losowy czas, nie chcę 'atakować' bazy w pętli
                        System.Threading.Thread.Sleep(_r.Next(cfg.MinDeadlockSleep, cfg.MaxDeadlockSleep));
                        deadlockCnt++;
                    }
                    else
                    {
                        Log.Error(e.Message, e);
                        //Nie jest to deadlock, nie jest to moja sesja.. lub było ich za dużo
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Konstruktor, inicjalizacja diagnostyki
        /// </summary>
        public Database()
        {
            m_Log = log4net.LogManager.GetLogger(this.GetType());

            //powinien być jeden obiekt na wątek, więc taki krótki wpis jest pomocny
            m_Log.Debug("Database created.");
        }

        /// <summary>
        /// Diagnostyka
        /// </summary>
        private ILog m_Log;

        /// <summary>
        /// Diagnostyka
        /// </summary>
        protected ILog Log { get { return m_Log; } }

        /// <summary>
        /// Metoda wewnętrzna, wywoływana w pętli na wypadek gdyby wystąpił deadlock
        /// Parametry identyczne jak dla ExecuteWork
        /// </summary>
        protected virtual void TryExecuteWork(INHUnitOfWork work, ISession session = null)
        {
            if (work == null)
                throw new ArgumentNullException("work");

            ISession s;
            bool mySession = session == null;

            if (mySession)
                s = SessionFactory.OpenSession();
            else
                s = session;

            ITransaction t = null;

            bool myTran = mySession || s.Transaction == null;

            if (myTran)
            {
                if ((int)work.TransactionIsolationLevel == 0)
                    work.TransactionIsolationLevel = System.Data.IsolationLevel.ReadCommitted;

                t = s.BeginTransaction(work.TransactionIsolationLevel);
            }

            try
            {
                try
                {
                    work.Session = s;
                    work.Execute();

                    if (myTran) //jak moja to robię commit
                        try
                        {
                            t.Commit();
                        }
                        catch (GenericADOException e)
                        {
                            if (e.InnerException != null)
                                Log.Error("Commit exception", e.InnerException);

                            Log.Error("Commit exception", e);
                            throw;
                        }
                }
                catch (Exception e)
                {
                    if (myTran)
                        try
                        {
                            //jeżeli to nie deadlock.. to mam problem
                            if (!CurrenctDialectHelper.IsDeadlock(e))
                                t.Rollback();
                        }
                        catch (GenericADOException e_rollback)
                        {
                            if (e_rollback.InnerException != null)
                                Log.Error("Rollback exception", e_rollback.InnerException);

                            Log.Error("Rollback exception", e_rollback);
                            throw;
                        }
                        catch (TransactionException e_rollback)
                        {
                            Log.Error("Rollback exception", e_rollback);
                            throw;
                        }

                    //jeżeli był deadlock, to obsługa jego jest metodzie wywołującej tą
                    throw;
                }
            }
            finally
            {
                //jeżeli to moja sesja, to po zakończonej pracy - rozłączam się
                if (mySession)
                    s.Close();
            }
        }

    }
}
