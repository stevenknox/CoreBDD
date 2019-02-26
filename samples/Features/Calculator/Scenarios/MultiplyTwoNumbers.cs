using Xunit;
using CoreBDD;
using Xunit.Abstractions;

namespace SampleBDD.Tests
{
    public class AdvancedCalculator : CalculatorFeature
    {
        [Scenario("Multiply two numbers")]
        public void MultiplyTwoNumbers()
        {
            Calculator calculator = new Calculator();

            Given("I have a calculator", () => { });
            When("I key in 10", () => calculator.Key(10));
            And("I key in 5 and press multiply", () => calculator.Multiply(5));
            Then("It sets the Total to 50", () => calculator.Total.ShouldBe(50));
            And("It sets the equation to 10 x 5", () => calculator.Equation.ShouldBe("10 x 5"));
        }

        [ScenarioOutline("Divide two numbers")]
        [Examples(10, 2, 5)]
        [Examples(20, 4, 5)]
        public void DivideTwoNumbers(int number, int divideby, int result)
        {
            Given($"I have a calculator", () => new Calculator(), out Calculator inlineCalc);
            When($"I key in {number}", () => inlineCalc.Key(number));
            And($"I key in {divideby} and press divide", () => inlineCalc.Divide(divideby));
            Then($"It sets the Total to {result}", () => inlineCalc.Total.ShouldBe(result));
            And($"It sets the equation to {number} / {divideby}", () => inlineCalc.Equation.ShouldBe($"{number} / {divideby}"));
        }
    }
}