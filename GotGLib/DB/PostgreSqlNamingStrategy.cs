using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotGLib.DB
{
    /// <summary>
    /// Klasa wykorzystywania jako pomoc w mapowaniu obiektów i cech na tabele i pola
    /// Umożliwia wykorzystywanie nazw CamelCase w C# i konwertowanie tego na camel_case w PGSQL
    /// </summary>
    internal class PostgreSqlNamingStrategy : INamingStrategy
    {
        public string ClassToTableName(string className)
        {
            return ConvertName(className);
        }

        public string ColumnName(string columnName)
        {
            return ConvertName(columnName);
        }

        public string LogicalColumnName(string columnName, string propertyName)
        {
            return String.IsNullOrWhiteSpace(columnName) ?
                ConvertName(propertyName) :
                ConvertName(columnName);
        }

        public string PropertyToColumnName(string propertyName)
        {
            return ConvertName(propertyName);
        }

        public string PropertyToTableName(string className, string propertyName)
        {
            return ConvertName(propertyName);
        }

        public string TableName(string tableName)
        {
            return ConvertName(tableName);
        }

        /// <summary>
        /// Metoda zamienia nazwę CamelCase na format przyjazny dla bazy danych czyli camel_case :)
        /// </summary>
        /// <param name="name">Nazwa do konwersji</param>
        /// <returns>Wynik</returns>
        protected string ConvertName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                StringBuilder _res = new StringBuilder();
                int charIx = 0;

                //pierwszy znak zamieniam zawsze na małą literę, więc ignoruję...
                _res.Append(name[charIx++]);

                while (charIx < name.Length)
                {
                    if (char.IsUpper(name[charIx]))
                        _res.Append('_');

                    _res.Append(name[charIx]);
                    charIx++;
                }

                return _res.ToString().ToLowerInvariant();
            }
            else
                return name;
        }

    }
}
