using System;
using Xunit;

namespace CoreBDD
{

    public class Scenario : BDDAttribute
    {
       public Scenario():base(1)
        {
            
        }

        public Scenario(params object[] data):base(1,data)
        {
            
        }

    }
}