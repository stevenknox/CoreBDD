using Xunit;

namespace SimpleBDD
{
    public class InlineSpecAttribute : BDDAttribute
    {
        public InlineSpecAttribute() : base(0)
        {

        }

        public InlineSpecAttribute(params object[] data) : base(0, data)
        {

        }
    }
}