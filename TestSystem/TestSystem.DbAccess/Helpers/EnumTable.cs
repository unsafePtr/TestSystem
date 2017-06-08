using TestSystem.Common.Helpers;

namespace TestSystem.DbAccess.Helpers
{
    /// https://stackoverflow.com/a/34558339/6700082
    /// <summary>
    /// Provides code first way to create table with fixed enum values in Database
    /// </summary>
    /// <typeparam name="TEnum">Type of enum</typeparam>
    public class EnumTable<TEnum>
        where TEnum : struct
    {
        public TEnum Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Empty constructor for ef
        /// </summary>
        protected EnumTable() { }

        public EnumTable(TEnum enumType)
        {
            ExceptionHelpers.ThrowIfNotEnum<TEnum>();

            Id = enumType;
            Name = enumType.ToString();
        }

        public static implicit operator EnumTable<TEnum>(TEnum enumType) => new EnumTable<TEnum>(enumType);
        public static implicit operator TEnum(EnumTable<TEnum> status) => status.Id;
    }
}
