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
    }
}
