using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TestSystem.Common.Helpers;

namespace TestSystem.DbAccess.Helpers
{
    public static class SeedEnumExtensions
    {
        public static string GetEnumDescription<TEnum>(this TEnum item)
            where TEnum : struct
        {
            ExceptionHelpers.ThrowIfNotEnum<TEnum>();

            return item.GetType()
                       .GetField(item.ToString())
                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                       .Cast<DescriptionAttribute>()
                       .FirstOrDefault()?.Description ?? string.Empty;
        }

        public static void SeedEnumValues<T, TEnum>(this IDbSet<T> dbSet, Func<TEnum, T> converter)
            where T : class
        {
            Enum.GetValues(typeof(TEnum))
                .Cast<object>()
                .Select(value => converter((TEnum)value))
                .ToList()
                .ForEach(instance => dbSet.AddOrUpdate(instance));
        }
    }
}
