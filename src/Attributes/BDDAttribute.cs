using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace SimpleBDD
{
    [XunitTestCaseDiscoverer("SimpleBDD.Discoverer", "SimpleBDD")]
    public class BDDAttribute : FactAttribute
    {
        public int Priority { get; set; }
        public string Spec { get; set; }
        public object[] args { get; set; }
        public BDDAttribute()
        {
            
        }

        public BDDAttribute(int priority, params object[] data)
        {
            Priority = priority;
            Spec = data[0].ToString();
            this.args = data.Skip(1).ToArray();
        }
    }
}