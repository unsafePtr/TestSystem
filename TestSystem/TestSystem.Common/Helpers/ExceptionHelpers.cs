using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.Common.Helpers
{
    public static class ExceptionHelpers
    {
        public static void ThrowIfNotEnum<TEnum>()
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new Exception($"Invalid generic method argument of type {typeof(TEnum)}");
            }
        }

        public static void ThrowIfNull<T>(this T source, Exception exception)
            where T : class
        {
            if(source == null)
            {
                throw exception;
            }
        }

        public static void ThrowIfNull<T>(this Nullable<T> source, Exception exception)
            where T : struct
        {
            if(!source.HasValue)
            {
                throw exception;
            }
        }
    }
}
