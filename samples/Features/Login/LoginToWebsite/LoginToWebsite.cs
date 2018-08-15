using CoreBDD;
using Xunit;

namespace SampleBDD.Tests
{
    public class LoginToWebsiteScenario: LoginFeature
    {
        [Spec("LoginToWebsite Description")]
        public void LoginToWebsite()
        {
            Given($"I have ....",   () => { });
            When($"I ...",          () => { });
            Then($"I ...",			() => { });
        }
    }
}