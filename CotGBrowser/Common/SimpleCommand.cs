using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CotGBrowser.Common
{
    /// <summary>
    /// Implementacja dosyć prostej klasy 'polecenia' 
    /// </summary>
    /// <remarks>
    /// Odpowiada za dwie fundamentalne sprawy:
    /// 1. Wykonanie podpiętej pod polecenie metody
    /// 2. Odpowieź na pytanie czy można w danym momencie uruchomić polecenie
    /// </remarks>
    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// Konstruktor z podstawowymi metdami 
        /// </summary>
        /// <param name="mv">Model który zawiera poecenie, zmiany jego cech (Notify...) powodują odpalenie zdarzenia CanExecuteChanged</param>
        /// <param name="executeAction">Kod do wykonania podczas odpalenia polecenia</param>
        /// <param name="canExecutePredicate">Kod sprawdzający czy można uruchomić polecenie, jeżeli null
        /// to można zawsze</param>
        public SimpleCommand(
            INotifyPropertyChanged mv,
            Action<object> executeAction,
            Predicate<object> canExecutePredicate = null)
        {
            if (executeAction == null)
                throw new ArgumentNullException("executeAction");

            m_model = mv;

            if (m_model != null)
                m_model.PropertyChanged += m_model_PropertyChanged;

            m_ExecuteAction = executeAction;
            m_CanExecutePredicate = canExecutePredicate;
        }

        void m_model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DoCanExecuteChanged();
        }

        protected INotifyPropertyChanged m_model;
        protected Action<object> m_ExecuteAction;
        protected Predicate<object> m_CanExecutePredicate;
        private ICommand add2RoleCmd;
        private Func<object, bool> p;

        #region ICommand

        /// <summary>
        /// Należy tą metodą odpalić z zewnątrz jeżel zmieniły się warunki w których można odpalić polecenie
        /// </summary>
        public void DoCanExecuteChanged()
        {
            EventHandler h = CanExecuteChanged;

            if (h != null)
                h(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            if (m_CanExecutePredicate != null)
                return m_CanExecutePredicate(parameter);
            else
                return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            m_ExecuteAction(parameter);
        }

        #endregion
    }
}
