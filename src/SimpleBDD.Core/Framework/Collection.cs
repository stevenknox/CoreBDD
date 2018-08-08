using Xunit;

namespace SimpleBDD
{
    [CollectionDefinition("SimpleBBD")]
    public class Collection : ICollectionFixture<SimpleBDDFixture> { }
}