using Xunit;

namespace SimpleBDD
{
    [TestCaseOrderer("SimpleBDD.OrderedSpecification", "SimpleBDD")]
    [Collection("SimpleBBD")]
    public class Specification : IClassFixture<GivenWhenThenFixture>
    {
        public GivenWhenThenFixture Context;

        public Specification(GivenWhenThenFixture context)
        {
            Context = context;
        }

    }
}