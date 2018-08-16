namespace CoreBDD
{

    public class But : BDDAttribute
     {
         public But():base(2)
        {
            
        }

        public But(params object[] data):base(2,data)
        {
            
        }
     }
}