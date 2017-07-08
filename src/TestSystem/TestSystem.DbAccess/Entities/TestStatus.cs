using TestSystem.DbAccess.Helpers;

namespace TestSystem.DbAccess.Entities
{
    public enum TestStatusEnum
    {
        Active = 0,
        Inactive = 1
    }

    public class TestStatus : EnumTable<TestStatusEnum>
    {
        public TestStatus(TestStatusEnum enumType) : base(enumType)
        {
        }

        public TestStatus() : base() { }
    }
}
