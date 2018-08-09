using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CoreBDD
{

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
            attrs.AddRange(testCase.TestMethod.Method.GetCustomAttributes((typeof(WhenAttribute).AssemblyQualifiedName)));
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