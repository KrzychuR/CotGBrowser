using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CotGBrowser.Common
{
    public abstract class BaseModelView : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <summary>
        /// Inicjalizacja
        /// </summary>
        public BaseModelView()
        {
            Log = log4net.LogManager.GetLogger(GetType());
            IsBusy = false;
            StatusInfo = "?";
            m_Errors = new Dictionary<string, List<string>>();
        }

        #region Dane/Cechy ----------------------------------------------------------------------------------

        /// <summary>
        /// Sygnalizacja zajękości modelu pracą w tle
        /// </summary>
        public bool IsBusy
        {
            get { return m_IsBusy; }
            set
            {
                if (m_IsBusy != value)
                {
                    m_IsBusy = value;
                    DoPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Aktualny status/informacja dla usera
        /// </summary>
        public string StatusInfo
        {
            get { return m_StatusInfo; }
            set
            {
                if (m_StatusInfo != value)
                {
                    m_StatusInfo = value;
                    DoPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Ogólny komunikat błędu, przydatne bardzo do dialogów
        /// </summary>
        public string ErrorMsg
        {
            get { return m_ErrorMsg; }
            set
            {
                if (m_ErrorMsg != value)
                {
                    m_ErrorMsg = value;
                    DoPropertyChanged();
                }
            }
        }

        private string m_ErrorMsg;
        private string m_StatusInfo;
        private bool m_IsBusy;

        #endregion

        #region Chronione ------------------------------------------------------------------------------------------

        protected ILog Log { get; private set; }

        /// <summary>
        /// Metod upraszczejąca ustawianie cech wraz z powiadamianiem obserwatorów
        /// </summary>
        protected virtual bool SetProperty<T>(ref T container, T value, [CallerMemberName] String propertyName = "")
        {
            if (object.Equals(container, value))
                return false;

            container = value;
            DoPropertyChanged(propertyName);
            return true;
        }
        
        #endregion

        #region INotifyPropertyChanged --------------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Odpalenie zdarzenia w bezpieczny sposób. Jeżeli metoda zostanie odpalona bez parametru to
        /// nazwa parametru zostanie uzupełniona o nazwę moetody/property wywołującego
        /// </summary>
        /// <param name="propertyName">Nie podawać bez potrzeby!</param>
        protected void DoPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChangedEventHandler _h = PropertyChanged;

            if (_h != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyDataErrorInfo ------------------------------------------------------------------------------------------

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Odpalenie zdarzenia z informacją że zmienił się stan błędów związanych ze wskazanym property
        /// </summary>
        /// <param name="propertyName">Nazwa property, którego zmienił się stan komunikatów o błędach</param>
        protected void DoErrorsChanged(string propertyName)
        {
            EventHandler<DataErrorsChangedEventArgs> h = ErrorsChanged;

            if (h != null)
                h(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Błędy powiązane z polem nazwa_pola=błąd
        /// </summary>
        private Dictionary<string, List<string>> m_Errors;

        /// <summary>
        /// Pobranie błędów związanych z danym property
        /// </summary>
        /// <param name="propertyName">Property którego mają dotyczyć błędy</param>
        /// <returns>Lista błędów</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (m_Errors != null)
            {
                if (m_Errors.Keys.Contains(propertyName))
                    return m_Errors[propertyName];
                else
                    return null;
            }
            else
                return null;
        }

        //Czy są jakieś błędy ?
        public bool HasErrors
        {
            get { return m_Errors != null ? m_Errors.Count > 0 : false; }
        }

        /// <summary>
        /// Wyczyszczenie błędów związanych z cechą
        /// </summary>
        /// <param name="memberExp">Wyrażenie wskazujące której cechy dotyczy czyszczenie</param>
        protected void ClearErrors<T>(Expression<Func<T, object>> memberExp)
        {
            MemberExpression exp = memberExp.Body as MemberExpression;

            if (exp == null)
                throw new ArgumentException("Musisz podać wyrażenie typu MemberExpression", "memberExp");

            if (m_Errors.Keys.Contains(exp.Member.Name))
            {
                m_Errors.Remove(exp.Member.Name);
                DoErrorsChanged(exp.Member.Name);
            }
        }

        /// <summary>
        /// Dodanie komunikatu z błędem
        /// </summary>
        /// <param name="memberExp">Wyrażenie wskazujące której cechy dotyczy podany komunikat błędu</param>
        /// <param name="msg">Komunikat</param>
        protected void AddError<T>(Expression<Func<T, object>> memberExp, string msg)
        {
            MemberExpression exp = memberExp.Body as MemberExpression;

            if (exp == null)
                throw new ArgumentException("Musisz podać wyrażenie typu MemberExpression", "memberExp");

            List<string> propertyErrors;

            //czy są błędy związane z kotrolką ?
            if (!m_Errors.TryGetValue(exp.Member.Name, out propertyErrors))
            {
                ///nie mam - to będzie nowa lista..
                propertyErrors = new List<string>();
                m_Errors.Add(exp.Member.Name, propertyErrors);
            }

            //czy jest taki komunikat? jak jest to nie dodaję...
            if (propertyErrors.FirstOrDefault(x => x == msg) == null)
                propertyErrors.Add(msg);

            DoErrorsChanged(exp.Member.Name);
        }

        #endregion
    }
}
