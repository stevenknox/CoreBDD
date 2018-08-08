using System;

namespace SimpleBDD
{

    public class Spec : BDDAttribute
    {
       public Spec():base(1)
        {
            
        }

        public Spec(params object[] data):base(1,data)
        {
            
        }

    }
}