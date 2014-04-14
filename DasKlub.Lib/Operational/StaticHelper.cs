using System;
using System.Collections.Generic;
using System.Linq;

namespace DasKlub.Lib.Operational
{
    public static class StaticHelper
    {
        /// <summary>
        ///     Sort a list randomly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <see cref="http://stackoverflow.com/questions/273313/randomize-a-listt-in-c" />
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}