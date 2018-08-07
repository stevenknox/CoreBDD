using Xunit;

namespace SimpleBDD
{
    [Scenario("Subtract two numbers")]
    public class SubtractTwoNumbers : CalculatorFeature
    {
        readonly Calculator calc;

        public SubtractTwoNumbers(GivenWhenThenFixture state) : base(state)
        {
             calc = new Calculator();
        }

        [Given("I have entered {0} into the calculator", 5)]
        public void Given(int first)
        {
            Context.Given.First = first;
        }

        [And("I have also entered {0} into the calculator", 2)]
        public void And(int second)
        {
            Context.Given.Second = second;
        }

        [When("I press minus")]
        public void When()
        {
            Context.When = calc.Subtract(Context.Given.First, Context.Given.Second);
        }

        [Then("the result should be {0}", 3)]
        public void Then(int result)
        {
            Assert.Equal(Context.When, result);
        }

    }
}
