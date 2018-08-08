using System;

namespace SimpleBDD
{

    public class SpecAttribute : BDDAttribute
    {
       public SpecAttribute():base(1)
        {
            
        }

        public SpecAttribute(params object[] data):base(1,data)
        {
            
        }

    }
}