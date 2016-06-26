using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace GotGLib.DB
{
    abstract public class NHUnitOfWork : INHUnitOfWork
    {
        public ISession Session { get; set; }

        public IsolationLevel TransactionIsolationLevel { get; set; }

        public abstract void Execute();

        public void Validate()
        {
        }
    }
}
