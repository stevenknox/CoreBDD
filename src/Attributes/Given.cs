namespace SimpleBDD
{
    public class Given : BDDAttribute
    {
        public Given():base(1)
        {
            
        }

        public Given(params object[] data):base(1,data)
        {
            
        }
    }
}