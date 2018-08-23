using System;
using System.IO;
using CoreBDD.SpecGeneration;
using Xunit;

namespace SampleBDD.Tests
{
    [CollectionDefinition("CoreBDD")]
    public class Collection : ICollectionFixture<GenerateSpecsFixture> { }

    public class GenerateSpecsFixture : IDisposable
    {
        public void Dispose()
        {
           var specsFolder = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName;
            GenerateSpecs.OutputFeatureSpecs(this.GetType().Assembly.Location, specsFolder + @"\Specs");
        }
    }
}