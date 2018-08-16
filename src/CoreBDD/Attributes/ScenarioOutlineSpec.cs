using System;
using Xunit;

namespace CoreBDD
{
    public class ScenarioOutline : TheoryAttribute
    {
        public string Scenario { get; set; }
        public ScenarioOutline()
        {
            
        }

        public ScenarioOutline(string spec)
        {
            Scenario = spec;
        }

    }
}