using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Helpers
{
    public static class DBHelper
    {
        public static ICollection<T> Query<T>(this DbConnection conn, string sql, Func<DbDataReader, T> unpackFunction)
        {
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
            }

            return data;
        }

        public static T Get<T>(this DbConnection conn, string sql, Func<DbDataReader, T> unpackFunction)
        {
            return conn.Query(sql, unpackFunction).FirstOrDefault();
        }

        public static void Insert(this DbConnection conn, string tableName, IEnumerable<KeyValuePair<string, object>> columnValues)
        {
            Insert<object>(conn, tableName, columnValues, null);
        }

        public static T Insert<T>(this DbConnection conn, string tableName, IEnumerable<KeyValuePair<string, object>> columnValues, Func<DbDataReader, T> unpackFunction)
        {
            string sql = "insert into " + tableName + " ";
            sql += string.Join(",", columnValues.Select(v => v.Key));
            sql += " values(";
            sql += string.Join(",", columnValues.Select(v => "@" + v.Key));
            sql += ")";

            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;

                foreach(var columnValue in columnValues)
                {
                    var dbParam = cmd.CreateParameter();
                    dbParam.ParameterName = columnValue.Key;
                    dbParam.Value = columnValue.Value;

                    cmd.Parameters.Add(dbParam);
                }

                var id = cmd.ExecuteNonQuery();

                if (unpackFunction != null)
                {
                    string getSql = "select * from " + tableName + " where id = " + id.ToString() + " limit 1";
                    return conn.Get(getSql, unpackFunction);
                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
