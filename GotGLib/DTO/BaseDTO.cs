using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DTO
{
    public abstract class BaseDTO : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private bool m_UISelected;

        /// <summary>
        /// Obiekt jest wybrany/zaznaczony przez użytkownika
        /// </summary>
        public bool UISelected
        {
            get { return m_UISelected; }
            set
            {
                if (m_UISelected != value)
                {
                    m_UISelected = value;
                    DoPropertyChanged();
                }
            }
        }

        #region INotifyPropertyChanged------------------------------------------------------------------------

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

        #region Obsługa informowania o błędach walidacji -------------------------------------------------------------------------------

        /// <summary>
        /// Dodanie komunikatu z błędem, zakładam że na raz jest tylko 1 komunikat związany z polem
        /// </summary>
        /// <param name="propertyName">Nazwa cechy</param>
        /// <param name="msg">Komunikat</param>
        public void AddError(string propertyName, string msg)
        {
            Errors.Remove(propertyName);

            List<string> l = new List<string>();
            l.Add(msg);
            Errors.Add(propertyName, l);
            RaiseErrorsChanged(propertyName);
        }

        /// <summary>
        /// Przeciążona metoda AddError umożliwiająca podanie nazwy cechy w formie wyrażenia lambda.. znakomicie ułatwia refaktoring kodu
        /// i ogranicza pomyłki
        /// </summary>
        /// <param name="propertyLambda">Patrz: PropName</param>
        /// <param name="msg">Komunikat</param>
        public void AddError<T>(Expression<Func<T>> propertyLambda, string msg)
        {
            AddError(PropName(propertyLambda), msg);
        }

        /// <summary>
        /// Wyczyszczenie błędów związanych z cechą
        /// </summary>
        /// <param name="propertyName">Nazwa cechy</param>
        public void ClearErrors(string propertyName)
        {
            if (Errors.Keys.Contains(propertyName))
            {
                Errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Wyczyszczenie wszystkich błędów
        /// </summary>
        public void ClearErrors()
        {
            //muszę zrobić kopię bo w foreach nie mogę modyfikować kolekcji po które kroczę...
            List<string> keys = new List<string>(Errors.Keys);

            foreach (var propName in keys)
                ClearErrors(propName);
        }

        /// <summary>
        /// Przeciążenie ClearErrors analogiczne do AddError
        /// </summary>
        public void ClearErrors<T>(Expression<Func<T>> propertyLambda)
        {
            ClearErrors(PropName(propertyLambda));
        }

        /// <summary>
        /// Odpalenie zdarzenia informującego UI, że zmienił się stan błędów
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Listy błędów podpięte pod cechy klasy
        /// </summary>
        private Dictionary<string, List<string>> m_Errors;

        /// <summary>
        /// Błędy powiązane z polem nazwa_pola=błąd
        /// </summary>
        public Dictionary<string, List<string>> Errors
        {
            get
            {
                if (m_Errors == null)
                    m_Errors = new Dictionary<string, List<string>>();

                return m_Errors;
            }
            set
            {
                m_Errors = value;

                if (m_Errors != null)
                    foreach (string propName in m_Errors.Keys)
                        RaiseErrorsChanged(propName);
            }
        }

        #endregion

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (Errors.Keys.Contains(propertyName))
                    return Errors[propertyName];
                else
                    return null;
            }
            else
                return null;

        }

        public bool HasErrors
        {
            get { return Errors.Count > 0; }
        }

        #endregion

        /// <summary>
        /// Zwraca nazwę property podanej jako wyrażenie lamda, przykładowo użycie wyrażenia "() => this.NazwaProperty" zwróci "NazwaProperty"
        /// </summary>
        /// <remarks>
        /// Metoda ta ma największe zastosowanie w obsłudze interfejsu/zdarzenia PropertyChanged, gdzie jako parametr przekazywana jest
        /// tekstowo nazwa zmienionego property. Zamiast w obsłudze robić porównanie z tekstem - który potem b. utrudnia refaktoring
        /// można użyć przykładowo czegoś takiego:
        /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~{.cs}
        /// if( e.PropertyName == PropName(() => this.NazwaProperty) )
        /// {
        ///  //kod związany ze zmianą NazwaProperty
        /// }
        /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /// 
        /// </remarks>
        /// <param name="propertyLambda">Wyrażenie lambda typu () => obiekt.NazwaProperty </param>
        /// <returns>Tekst/nazwa property</returns>
        public static string PropName<T>(Expression<Func<T>> propertyLambda)
        {
            MemberExpression me = propertyLambda.Body as MemberExpression;

            if (me == null)
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");

            return me.Member.Name;
        }

        /// <summary>
        /// Metoda wirtualna do pokrycia, służy do sprawdzenia czy dane obiektu mają szukany tekst.
        /// Wykorzystywana przy prostym szukaniu danych w rekordach
        /// </summary>
        /// <param name="exp">Fragment tekstu do wyszukania</param>
        /// <returns>True - jeżeli znaleziono, domyślnie bez pokrycia jest false</returns>
        virtual public bool ContainsSearchExp(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return true; //puste więc nie filtruję

            //a jak niepuste to domyślnie nie wiem jak szukać...
            return false;
        }

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
    }
}
