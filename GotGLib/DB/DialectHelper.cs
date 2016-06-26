using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DB
{
    /// <summary>
    /// Klasa z funkcjami specyficznymi dla danego dialektu/silnika
    /// </summary>
    public abstract class DialectHelper
    {
        /// <summary>
        /// Diagnostyka klasy
        /// </summary>
        protected ILog m_Log;

        /// <summary>
        /// Konstruktor
        /// </summary>
        protected DialectHelper()
        {
            m_Log = log4net.LogManager.GetLogger(this.GetType());
        }

        /*
        /// <summary>
        /// Connection string może wymagać parametrów specyficznych dla bazy
        /// </summary>
        /// <returns></returns>
        public virtual string BuildConnectionString()
        {
            string cs = Properties.Settings.Default.DBConnectionString;
            //Server=__SERVER__;Port=__PORT__;Database=__DB__;User Id=__USER__;Password=__PASS__;
            cs = cs.Replace("__SERVER__", Properties.Settings.Default.DBHost);
            cs = cs.Replace("__PORT__", Properties.Settings.Default.DBPort.ToString(CultureInfo.InvariantCulture));
            cs = cs.Replace("__DB__", Properties.Settings.Default.DBName);
            cs = cs.Replace("__USER__", Properties.Settings.Default.DBUserName);
            cs = cs.Replace("__PASS__", SecurityHelper.Decrypt(Properties.Settings.Default.DBPass));

            return cs;
        }
         */

        /// <summary>
        /// Skonfigurowanie NHibernate i utworzenie fabryki sesji (połączeń)
        /// </summary>
        /// <remarks>
        /// Ponieważ utworzenie fabryki jest stosunkowo kosztowne, nie powinna być ona tworzona
        /// zbyt często, należy jej instancje przechowywać do wielokrotnego nawiązywania połączenia
        /// </remarks>
        /// <param name="db">Obiekt klasy Database, jego zadaniem jest zwrócenie wszystkich
        /// Assembiels, które mają jakieś mapowania do bazy danych</param>
        /// <returns>Fabryka sesji</returns>
        public abstract ISessionFactory CreateSessionFactory(Database db);

        /// <summary>
        /// Sprawdzanie czy wyjątek jest deadlockiem.
        /// </summary>
        /// <remarks>
        /// Uwaga! Metoda powinna zgłosić wyjątek, jeżeli nie uda jej się jednoznacznie ustalić czy 
        /// przekazany wyjątek jest deadlockiem czy nie...
        /// </remarks>
        /// <param name="exception">Wyjątek do sprawdzenia</param>
        /// <returns>TRUE/FALSE w zależności od tego czy wyjątek jest związany z deadlockiem</returns>
        public abstract bool IsDeadlock(Exception exception);
    }
}
