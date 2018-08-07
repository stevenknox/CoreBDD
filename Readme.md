# SimpleBDD

BDD framework for xUnit.net

## Getting started with SimpleBDD

Following the usual calculator example, we can start with the following model to test
 
   
  ``` csharp
public class Calculator
    {
        public int Add(int x, int y) => x + y;
        public int Subtract(int x, int y) => x - y;
    }
```

We can define a *Feature* to collate a suite of scenarios by deriving from the *Specification* base class and decorating with the *Feature* attribute

  ``` csharp
    [Feature("Calculator", 
    @"In order to avoid silly mistakes
    As a math idiot
    I want to be told the sum of two numbers")]
    public class CalculatorFeature : Specification
    {
         public CalculatorFeature(GivenWhenThenFixture context):base(context) 
         {
            
         }
    }
```

To create a scenario for this feature, simply inherit from the new Feature class, decorate with a Scenario attribute and provide Given, When, Then methods that will execute in order

  ``` csharp
    [Scenario("Add two numbers")]
    public class AddTwoNumbers : CalculatorFeature
    {
        readonly Calculator calc;

        public AddTwoNumbers(GivenWhenThenFixture state) 
            : base(state) => calc = new Calculator();

        [Given("I have entered {0} into the calculator", 1)]
        public void Given(int first) => Context.Given.First = first;

        [And("I have also entered {0} into the calculator", 2)]
        public void And(int second) => Context.Given.Second = second;

        [When("I press add")]
        public void When() => Context.When = calc.Add(Context.Given.First, Context.Given.Second);

        [Then("the result should be {0}", 3)]
        public void Then(int result) => Context.Then.ShouldBe(result);
    }
```

The above shows a simple, terse implementation using expression bodied members for the Given/When/Then implementation. A more verbose example may look like

  ``` csharp
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
```

When the tests complete running, a *feature.spec* file is generated under the Specs folder of the xUnit test project. It generates Gherkin specs for the feature and related scenarios. Example *CalculatorFeature.spec* :

  ``` gherkin
    Feature: Calculator
	In order to avoid silly mistakes
    As a math idiot
    I want to be told the sum of two numbers

    Scenario: Add two numbers
    			Given I have entered 1 into the calculator
    			And I have also entered 2 into the calculator
    			When I press add
    			Then the result should be 3

    Scenario: Subtract two numbers
    			Given I have entered 5 into the calculator
    			And I have also entered 2 into the calculator
    			When I press minus
    			Then the result should be 3

```