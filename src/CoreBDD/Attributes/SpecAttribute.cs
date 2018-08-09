using System;
using Xunit;

namespace CoreBDD
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