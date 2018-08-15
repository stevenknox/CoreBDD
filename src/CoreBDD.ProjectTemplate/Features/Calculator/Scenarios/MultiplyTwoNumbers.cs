using Xunit;
using CoreBDD;

namespace CoreBDD.ProjectTemplate
{
    public class AdvancedCalculator : CalculatorFeature
    {
        Calculator calculator;

        [Spec("Multiply two numbers")]
        public void MultiplyTwoNumbers()
        {

            Given("I have a calculator",           () => calculator = new Calculator());
            When("I key in 10",                    () => calculator.Key(10));
            And("I key in 5 and press multiply",  () => calculator.Multiply(5));
            Then("It sets the Total to 50",        () => calculator.Total.ShouldBe(50));
            And("It sets the equation to 10 x 5", () => calculator.Equation.ShouldBe("10 x 5"));
        }

        [DataDrivenSpec("Divide two numbers")]
        [InlineData(10, 2, 5)]
        [InlineData(20, 4, 5)]
        public void DivideTwoNumbers(int number, int divideby, int result)
        {
            Given($"I have a calculator",         () => calculator = new Calculator());
            When($"I key in {number}",                    () => calculator.Key(number));
            And($"I key in {divideby} and press divide",  () => calculator.Divide(divideby));
            Then($"It sets the Total to {result}",        () => calculator.Total.ShouldBe(result));
            And($"It sets the equation to {number} / {divideby}", () => calculator.Equation.ShouldBe($"{number} / {divideby}"));
        }
    }
}