using Xunit.Sdk;

namespace SimpleBDD
{
    public class ThenAttribute : BDDAttribute
    {
        public ThenAttribute():base(4)
        {
            
        }

        public ThenAttribute(params object[] data):base(4,data)
        {
            
        }
    }
}