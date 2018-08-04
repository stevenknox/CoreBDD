using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace SimpleBDD
{
    //we can extract this into markdown etc:
    //Given I have two numbers, 1 and 2
    //When I add them together
    //Then I should get 3
    public class CalculatorSpec : Specification
    {
        readonly Calculator calc;

         public CalculatorSpec(GivenWhenThenFixture state):base(state) 
         {
             calc = new Calculator();
         }

        [Given]
        [I_Have("two numbers", 1, 2)]
        public void Given(string spec, int first, int second)
        {
            //going to extract assignment here out and do automatically, so the given method body could be empty
            state.given.first = first;
            state.given.second = second;
        }

        [When("I add them together")]
        public void When()
        {
            //going to try automatically set state.when, so method body only needs the cal expression
            state.when = calc.Add(state.given.first, state.given.second);
        }

        [Then]
        [I_Expect("an answer of", 3)]
        public void Then(string spec, int result)
        {
            //use a nice assertion tool like Shouldly
            Assert.Equal(state.when, result);

        }
    }
}
