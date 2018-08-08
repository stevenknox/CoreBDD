namespace SimpleBDD
{

    public class AndAttribute : BDDAttribute
     {
         public AndAttribute():base(2)
        {
            
        }

        public AndAttribute(params object[] data):base(2,data)
        {
            
        }
     }
}