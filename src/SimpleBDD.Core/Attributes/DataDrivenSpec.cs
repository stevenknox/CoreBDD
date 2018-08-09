using System;
using Xunit;

namespace SimpleBDD
{
    public class DataDrivenSpec : TheoryAttribute
    {
        public string Spec { get; set; }
        public DataDrivenSpec()
        {
            
        }

        public DataDrivenSpec(string spec)
        {
            Spec = spec;
        }

    }
}