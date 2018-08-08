using System;
using SimpleBDD.SpecGeneration;
using Xunit;

namespace SimpleBDD
{
    [CollectionDefinition("SimpleBDD")]
    public class Collection : ICollectionFixture<GenerateSpecsFixture> { }

    public class GenerateSpecsFixture : IDisposable
    {
        public void Dispose()
        {
            GenerateSpecs.OutputFeatureSpecs(this.GetType().Assembly.Location, @"..\..\..\Specs\");
        }
    }
}