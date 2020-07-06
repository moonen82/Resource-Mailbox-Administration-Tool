using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DataAccessLibrary
{
    public class SqliteDataAccess
    {
        public List<T> LoadData<T, U>(string query, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(query, parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string query, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(query, parameters);
            }
        }
    }
}


