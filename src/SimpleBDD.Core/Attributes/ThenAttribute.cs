using Xunit.Sdk;

namespace SimpleBDD
{
    public class Then : BDDAttribute
    {
        public Then():base(4)
        {
            
        }

        public Then(params object[] data):base(4,data)
        {
            
        }
    }
}