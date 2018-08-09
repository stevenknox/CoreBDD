using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CoreBDD
{

    public class Discoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public Discoverer(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            var args = factAttribute.GetNamedArgument<object[]>("args");

            yield return new TestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, args);
        }
    }
}