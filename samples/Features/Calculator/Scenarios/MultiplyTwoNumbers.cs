namespace SimpleBDD
{
    public class AdvancedCalculator : CalculatorFeature
    {
        Calculator calculator;

        [Spec("Multiply two numbers")]
        public void MultiplyTwoNumbers()
        {
            Given("I have a calculator",           () => calculator = new Calculator());
            When("I key in 10",                    () => calculator.Key(10));
            When("I key in 5 and press multiply",  () => calculator.Multiply(5));
            Then("It sets the Total to 50",        () => calculator.Total.ShouldBe(50));
            Then("It sets the equation to 10 x 5", () => calculator.Equation.ShouldBe("10 x 5"));
        }

        [Spec("Divide two numbers")]
        public void DivideTwoNumbers()
        {
            Given("I have a calculator",       () => calculator = new Calculator());
            When("I key in 42",                () => calculator.Key(42));
            Then("It sets the Total to 42",    () => calculator.Total.ShouldBe(42));
            Then("It sets the equation to 42", () => calculator.Equation.ShouldBe("42"));
        }
    }
}