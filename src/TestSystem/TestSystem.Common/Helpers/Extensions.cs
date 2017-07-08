using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.Common.Helpers.Extensions
{
    public static class Extensions
    {
        public static TEnum ParseEnum<TEnum>(this string value)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm
        /// <summary>
        /// In place shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IList<T> Shuffle<T>(this IList<T> sequence)
        {
            for (int i = 0; i < sequence.Count; i++)
            {
                // for truly random
                int j = StaticRandom.Rand(0, i + 1); // i+1 exclusive upper bound
                T temp = sequence[i];
                sequence[i] = sequence[j];
                sequence[j] = temp;
            }

            return sequence;
        }
    }
}
