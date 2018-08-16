using CoreBDD;
using Xunit;

namespace SampleBDD.Tests
{
    public class LoginToWebsiteScenario: LoginFeature
    {
        [Scenario("LoginToWebsite Description")]
        public void LoginToWebsite()
        {
            Given($"I have ....",   () => { });
            When($"I ...",          () => { });
            Then($"I ...",			() => { });
        }
    }
}