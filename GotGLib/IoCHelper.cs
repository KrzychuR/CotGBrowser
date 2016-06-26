using GotGLib.DB;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib
{
    public static class IoCHelper
    {
        private static IUnityContainer m_IoC;
        private static object lockObj = new object();

        public static bool IsInitialized { get { return m_IoC != null; } }

        public static IUnityContainer GetIoC()
        {
            lock (lockObj)
            {
                if (m_IoC == null)
                {
                    m_IoC = new UnityContainer();

                    //wybór typu bazy danych
                    //m_IoC.RegisterType<DialectHelper, MsSqlDialectHelper>();
                    m_IoC.RegisterType<DialectHelper, SQLiteDialectHelper>();

                    //konfiguracja bazy, jedna klasa zarządzająca bazą/wątek
                    m_IoC.RegisterType<Database, Database>(new PerThreadLifetimeManager());

                    //klasa/interfejs z przeglądarką też jedna, żebym mógł się łatwo podpiąć do jej zdarzeń
                    m_IoC.RegisterType<JS.JScriptInterface, JS.JScriptInterface>(new ContainerControlledLifetimeManager());

                    //rejestruje fabrykę (samą siebie...) to dla tych klas które w dependency mają wstrzykniętą fabrykę
                    m_IoC.RegisterInstance<IUnityContainer>(m_IoC);
                }

                return m_IoC;
            }
        }
    }
}
