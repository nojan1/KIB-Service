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
            var source = self.ToArray();
            var destination = new T[source.Length];

            for(int i = 0; i < source.Length; i++)
            {
                var swapWith = rnd.Next(0, source.Length);
                var backup = destination[i];

                destination[i] = source[swapWith];
                source[swapWith] = backup;
            }

            return destination;
        }
    }
}
