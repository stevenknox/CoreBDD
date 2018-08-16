using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace CoreBDD
{
    [XunitTestCaseDiscoverer("CoreBDD.Discoverer", "CoreBDD")]
    public class BDDAttribute : FactAttribute
    {
        public int Priority { get; set; }
        public string Definition { get; set; }
        public object[] args { get; set; }
        public BDDAttribute()
        {
            
        }

        public BDDAttribute(int priority, params object[] data)
        {
            Priority = priority;
            if(data.Length > 0)
            {
                Definition = data[0].ToString();
                this.args = data.Skip(1).ToArray();
            }
        }
    }
}
