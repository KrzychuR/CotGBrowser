using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using GotGLib.JS;
using Microsoft.Practices.Unity;

namespace GotGLib.DB
{
    public class SQLiteDialectHelper : DialectHelper
    {
        public override ISessionFactory CreateSessionFactory(Database db)
        {
            Configuration _cfg = new Configuration();

            var dbFile = DBFileName();

            if(string.IsNullOrWhiteSpace(dbFile))
            {
                throw new NotSupportedException("You can not access database without world context.");
            }

            var fluentCfg = Fluently.Configure(_cfg)
                .Database(SQLiteConfiguration.Standard.UsingFile(m_DataFile))
                .Mappings(m =>
                {
                    foreach (Assembly a in db.GetAssembliesWithMappings())
                        m.FluentMappings.AddFromAssembly(a);
                })
                .Diagnostics(x =>
                {
                    if (Properties.Settings.Default.EnableNHDiag)
                    {
                        x.Enable();
                        m_Log.Warn("Diagnostyka NHibernate została włączona, może to negatywnie wpłynąć na wydajność.");
                    }
                    else
                        x.Disable();
                });

            if (Properties.Settings.Default.EnableNHDiag)
                fluentCfg.ExposeConfiguration(c => c.SetProperty("format_sql", "true"));

            return fluentCfg.BuildSessionFactory();
        }

        public override bool IsDeadlock(Exception exception)
        {
            return false;
        }

        public override string DBFileName()
        {
            string world = JSInterface.GetWorldNum();

            if (!string.IsNullOrWhiteSpace(world))
            {
                world = world.Trim().Replace(" ", "");

                string path = (AppDomain.CurrentDomain.GetData("DataDirectory") as string ?? "");

                if (!string.IsNullOrWhiteSpace(path))
                    path += @"\";

                var baseFile = path + @"data\db.sqlite";
                var dataDir = path + string.Format(@"data\{0}", world);
                m_DataFile = dataDir + @"\db.sqlite";

                if (!System.IO.File.Exists(m_DataFile))
                {
                    System.IO.Directory.CreateDirectory(dataDir);
                    System.IO.File.Copy(baseFile, m_DataFile, false);
                }
            }

            return m_DataFile;
        }

        private JScriptInterface m_JSInterface;
        
        /// <summary>
        /// JS Interface
        /// </summary>
        /// <remarks>
        /// Can not use [Dependency] attribute because JSInterface also depends on this class and we have circular dependence...
        /// </remarks>
        protected JScriptInterface JSInterface
        {
            get
            {
                if (m_JSInterface == null)
                    m_JSInterface = IoCHelper.GetIoC().Resolve<JScriptInterface>();

                return m_JSInterface;
            }
                
        }

        /// <summary>
        /// Actual DB data file
        /// </summary>
        private string m_DataFile;
    }
}
