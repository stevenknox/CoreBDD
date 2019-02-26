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
            Context = context;
        }

        public Specification()
        {
            WriteLine();
        }

        public virtual void Given(string context, Action action)
        {
            PrintStep("Given", context);
            action.Invoke();
        }

        public virtual void Given<T>(string context, Func<T> action, out T result)
        {
            PrintStep("Given", context);
            result = action.Invoke();
        }

        public virtual void When(string context, Action action)
        {
            PrintStep("When", context);
            action.Invoke();
        }


        public virtual void When<T>(string context, Func<T> action, out T result)
        {
            PrintStep("When", context);
            result = action.Invoke();
        }

        public virtual void Then(string context, Action action)
        {
            PrintStep("Then", context);
            action.Invoke();
        }
        
        public virtual void Then<T>(string context, Func<T> action, out T result)
        {
            PrintStep("Then", context);
            result = action.Invoke();
        }
        

        public virtual void And(string context, Action action)
        {
            PrintStep("And", context);
            action.Invoke();
        }
        public virtual void And<T>(string context, Func<T> action, out T result)
        {
            PrintStep("And", context);
            result = action.Invoke();
        }

        public virtual void But(string context, Action action)
        {
            PrintStep("But", context);
            action.Invoke();
        }

        public virtual void But<T>(string context, Func<T> action, out T result)
        {
            PrintStep("But", context);
            result = action.Invoke();
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