using CoreBDD;
using Xunit;

namespace CoreBDD.Tests.Features
{
    public class Calculator: CalculatorFeature
    {
        
			[Scenario("Add two numbers")]
			public void AddTwoNumbers()
			{
				Given($"I have entered 1 into the calculator",   () => { });
				And($"I have also entered 2 into the calculator",   () => { });
				When($"I press add",   () => { });
				Then($"the result should be 3",   () => { });
			}

			[Scenario("Multiply two numbers")]
			public void MultiplyTwoNumbers()
			{
				Given($"I have a calculator",   () => { });
				When($"I key in 10",   () => { });
				And($"I key in 5 and press multiply",   () => { });
				Then($"It sets the Total to 50",   () => { });
				And($"It sets the equation to 10 x 5",   () => { });
			}

			[Scenario("Divide two numbers")]
			public void DivideTwoNumbers()
			{
				Given($"I have a calculator",   () => { });
				When($"I key in 10",   () => { });
				And($"I key in 2 and press divide",   () => { });
				Then($"It sets the Total to 5",   () => { });
				And($"It sets the equation to 10 / 2",   () => { });
				Given($"I have a calculator",   () => { });
				When($"I key in 20",   () => { });
				And($"I key in 4 and press divide",   () => { });
				Then($"It sets the Total to 5",   () => { });
				And($"It sets the equation to 20 / 4",   () => { });
			}

			[Scenario("Subtract two numbers")]
			public void SubtractTwoNumbers()
			{
				Given($"I have entered 5 into the calculator",   () => { });
				And($"I have also entered 2 into the calculator",   () => { });
				When($"I press minus",   () => { });
				Then($"the result should be 3",   () => { });
			}

    }
}