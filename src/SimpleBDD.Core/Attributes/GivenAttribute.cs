namespace SimpleBDD
{
    public class GivenAttribute : BDDAttribute
    {
        public GivenAttribute():base(1)
        {
            
        }

        public GivenAttribute(params object[] data):base(1,data)
        {
            
        }
    }
}