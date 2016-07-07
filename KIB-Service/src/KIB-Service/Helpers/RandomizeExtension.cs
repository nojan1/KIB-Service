using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIB_Service.Helpers
{
    public static class RandomizeExtension
    {
        private static Random rnd = new Random();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self)
        {
            var list = self.ToArray();
       
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
