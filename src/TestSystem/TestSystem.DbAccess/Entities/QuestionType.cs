using TestSystem.DbAccess.Helpers;

namespace TestSystem.DbAccess.Entities
{
    public enum QuestionTypeEnum
    {
        Open = 0,
        Closed = 1
    }

    public class QuestionType : EnumTable<QuestionTypeEnum>
    {
        public QuestionType(QuestionTypeEnum enumType) : base(enumType)
        {
        }

        public QuestionType() : base() { }
    }
}
