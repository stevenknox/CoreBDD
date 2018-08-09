using System;
using Xunit;

namespace CoreBDD
{
    [TestCaseOrderer("CoreBDD.OrderedSpecification", "CoreBDD")]
    [Collection("CoreBDD")]
    public class Specification : IClassFixture<GivenWhenThenFixture>
    {
        public GivenWhenThenFixture Context;

        public Specification(GivenWhenThenFixture context)
        {
            Context = context;
        }

        public Specification()
        {

        }

        public virtual void Given(string context, Action action)
        {
            System.Console.WriteLine(context);
            action.Invoke();
        }

        public virtual void When(string context, Action action)
        {
            System.Console.WriteLine(context);
            action.Invoke();
        }

        public virtual void Then(string context, Action action)
        {
            System.Console.WriteLine(context);
            action.Invoke();
        }

    }
}