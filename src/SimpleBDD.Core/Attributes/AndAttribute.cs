namespace SimpleBDD
{

    public class And : BDDAttribute
     {
         public And():base(2)
        {
            
        }

        public And(params object[] data):base(2,data)
        {
            
        }
     }
}