namespace SimpleBDD
{
    [Scenario("Add two numbers")]
    public class AddTwoNumbers : CalculatorFeature
    {
        readonly Calculator calc;

        public AddTwoNumbers(GivenWhenThenFixture state) 
            : base(state) => calc = new Calculator();

        [Given("I have entered {0} into the calculator", 1)]
        public void GivenStep(int first) => Context.Given.First = first;

        [And("I have also entered {0} into the calculator", 2)]
        public void AndStep(int second) => Context.Given.Second = second;

        [When("I press add")]
        public void WhenStep() => Context.When = calc.Add(Context.Given.First, Context.Given.Second);

        [Then("the result should be {0}", 3)]
        public void ThenStep(int result) => Context.Then.ShouldBe(result);
    }
}
