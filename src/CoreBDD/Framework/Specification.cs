using System;
using System.Linq;
using Xunit;
using static System.Console;

namespace CoreBDD
{
    [TestCaseOrderer("CoreBDD.OrderedSpecification", "CoreBDD")]
    [Collection("CoreBDD")]
    public class Specification : IClassFixture<SpecFixture>
    {
        public SpecFixture Context;

        public Specification(SpecFixture context)
        {
            var x = (Feature)this.GetType().GetCustomAttributes(typeof(Feature), true).ToList().First();

            Context = context;
            Context.Feature = x;
        }

        public Specification()
        {
            Console.WriteLine();
        }

        public virtual void Given(string context, Action action)
        {
            PrintStep("Given", context);
            action.Invoke();
        }

        public virtual void When(string context, Action action)
        {
            PrintStep("When", context);
            action.Invoke();
        }

        public virtual void Then(string context, Action action)
        {
            PrintStep("Then", context);
            action.Invoke();
        }

        public virtual void And(string context, Action action)
        {
            PrintStep("And", context);
            action.Invoke();
        }

        public virtual void But(string context, Action action)
        {
            PrintStep("But", context);
            action.Invoke();
        }

        static void PrintStep(string step, string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(step + " ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text.TrimStart(step.ToCharArray()));
            ResetColor();
        }

    }
}