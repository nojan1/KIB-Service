using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Helpers
{
    public class DBHelper
    {
        private ConnectionStringOption connectionStringOption;
        public DBHelper(ConnectionStringOption connectionStringOption)
        {
            this.connectionStringOption = connectionStringOption;
        }

        public DbConnection Connection
        {
            get
            {
                return new MySqlConnection(connectionStringOption.ConnectionString);
            }
        }

        public ICollection<T> Query<T>(string sql, Func<DbDataReader, T> unpackFunction)
        {
            var conn = Connection;
            var data = new List<T>();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;

                conn.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(unpackFunction(reader));
                }

                conn.Close();
            }

            return data;
        }

        public T Get<T>(string sql, Func<DbDataReader, T> unpackFunction)
        {
            return Query(sql, unpackFunction).FirstOrDefault();
        }

        public void Insert(string tableName, IEnumerable<KeyValuePair<string, object>> columnValues)
        {
            Insert<object>(tableName, columnValues, null);
        }

        public T Insert<T>(string tableName, IEnumerable<KeyValuePair<string, object>> columnValues, Func<DbDataReader, T> unpackFunction)
        {
            var conn = Connection;

            string sql = "insert into " + tableName + " (";
            sql += string.Join(",", columnValues.Select(v => v.Key));
            sql += ") values(";
            sql += string.Join(",", columnValues.Select(v => "@" + v.Key));
            sql += ")";

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;

                foreach (var columnValue in columnValues)
                {
                    var dbParam = cmd.CreateParameter();
                    dbParam.ParameterName = columnValue.Key;
                    dbParam.Value = columnValue.Value;

                    cmd.Parameters.Add(dbParam);
                }

                conn.Open();

                var id = cmd.ExecuteNonQuery();
                conn.Close();

                if (unpackFunction != null)
                {
                    string getSql = "select * from " + tableName + " where id = " + id.ToString() + " limit 1";
                    return Get(getSql, unpackFunction);
                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
