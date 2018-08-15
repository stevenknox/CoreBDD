using CoreBDD;

namespace SampleBDD.Tests
{
    [Feature("Login", 
    @"In order to avoid silly mistakes
    As a math idiot
    I want to be told the sum of two numbers")]
    public class LoginFeature : Specification
    {
         public LoginFeature(SpecFixture context):base(context) 
         {
            
         }

		 public LoginFeature() 
         {
            
         }
    }
}