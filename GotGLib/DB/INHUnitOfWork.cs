using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DB
{
    /// <summary>
    /// Definicja cech jakie powinna posiadać klasa aby mogła być traktowana 
    /// jako zadanie do uruchomienia w kontekście sesji/połaczenia 
    /// </summary>
    public interface INHUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Poziom izolacji transakcji jaki ma być zastosowany dla danego zadania, domyślnie ReadCommited
        /// </summary>
        /// <remarks>
        /// Domyślny poziom izolacji transakcji ReadCommitted. Oznacza to iż lock na odczytanych
        /// danych nie jest trzymany do końca transakcji. Konsekwencją tego może być niespójność polegająca na tym
        /// iż dokona się aktualizacji bazy danych, mimo iż 'dane wejściowe' już się zmieniły.
        /// 
        /// Dodatkowo trzeba uważać na operacje typu 'jak nie mam rekordu to wstaw nowy' w takich przypadkach
        /// albo trzeba zakładać klucze na tabeli, albo też poziom transakcji ustawić na serializable
        /// </remarks>
        IsolationLevel TransactionIsolationLevel { get; set; }

        /// Sesja w ramach której ma działać algorytm
        ISession Session { get; set; }
    }
}
