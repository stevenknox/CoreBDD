using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace CoreBDD
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [DataDiscoverer("Xunit.Sdk.InlineDataDiscoverer", "xunit.core")]
    public class ExamplesAttribute : DataAttribute
    {
        readonly object[] data;
        public ExamplesAttribute(params object[] data)
        {
            this.data = data;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return new[] { data };
        }
    }
}