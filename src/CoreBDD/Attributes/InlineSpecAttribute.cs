using Xunit;

namespace CoreBDD
{
    public class InlineSpec : BDDAttribute
    {
        public InlineSpec() : base(0)
        {

        }

        public InlineSpec(params object[] data) : base(0, data)
        {

        }
    }
}