using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SimpleBDD
{

    [TestCaseOrderer("SimpleBDD.OrderedSpecification", "SimpleBDD")]
    public class Specification: IClassFixture<GivenWhenThenFixture>
    {
        public GivenWhenThenFixture state;

        public Specification(GivenWhenThenFixture state)
        {
            this.state = state;
        }
    }

    public class GivenWhenThenFixture : IDisposable
    {
        public dynamic given { get; set; }
        public dynamic when { get; set; }
        public dynamic then { get; set; }

        public GivenWhenThenFixture()
        {
            given = new ExpandoObject();
            when = new ExpandoObject();
            then = new ExpandoObject();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GivenWhenThenFixture() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class I_HaveAttribute : DataAttribute
    {
        readonly string Spec; 
        readonly object[] data;

        public I_HaveAttribute(params object[] data)
        {
            // Spec = spec;
            this.data = data;
        }

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return new[] { data };
        }
    }

    public class I_Expect : I_HaveAttribute
    {
        public I_Expect(params object[] data): base(data)
        {
           //first is always spec
        }

    }

    public class Given : TheoryAttribute
    {
        public int Priority { get { return 1; } }
        public Given()
        {
            
        }
    }

    public class When : FactAttribute
    {
        public int Priority { get { return 2; } }
        public When(string spec)
        {
            
        }
    }

    public class Then : TheoryAttribute
    {
        public int Priority { get { return 3; } }
        public Then()
        {
            
        }
    }
    public class OrderedSpecification : ITestCaseOrderer
    {
      public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

            foreach (TTestCase testCase in testCases)
            {
                int priority = 0;

                foreach (IAttributeInfo attr in GivenWhenThenAttributes(testCase))
                    priority = attr.GetNamedArgument<int>("Priority");

                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
                foreach (TTestCase testCase in list)
                    yield return testCase;
            }
        }


        private IEnumerable<IAttributeInfo> GivenWhenThenAttributes<TTestCase>(TTestCase testCase) where TTestCase : ITestCase
        {
            var attrs = new List<IAttributeInfo>();
            attrs.AddRange(testCase.TestMethod.Method.GetCustomAttributes((typeof(Given).AssemblyQualifiedName)));
            attrs.AddRange(testCase.TestMethod.Method.GetCustomAttributes((typeof(When).AssemblyQualifiedName)));
            attrs.AddRange(testCase.TestMethod.Method.GetCustomAttributes((typeof(Then).AssemblyQualifiedName)));
            return attrs;
        }

        static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue result;

            if (dictionary.TryGetValue(key, out result)) return result;

            result = new TValue();
            dictionary[key] = result;

            return result;
        }
    }
}