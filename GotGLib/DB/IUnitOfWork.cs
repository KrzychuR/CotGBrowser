using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DB
{
    public interface IUnitOfWork
    {
        void Validate();

        /// <summary>
        /// Implementacja pewnego algorytmu do wykonania w ramach jednej transakcji na bazie danych
        /// </summary>
        /// <remarks>
        /// UWAGA!!! Algorytm powinien być tak skontruowany aby uwzględnić iż..
        /// 1) Będzie zawsze wykonywany na bazie w transakcji
        /// 2) W przypadku deadlocka, metoda ta zostanie automatycznie powtórzona
        /// 
        /// Wynik działania oraz ewentualne parametry powinny być przekazywane przez dodatkowe
        /// cechy zadania zdefiniowane w klasach implementujących INHUnitOfWork
        /// </remarks>
        void Execute();
    }
}
