using Xunit;
using System.IO;
using CoreBDD.CommandLine.Tools;
using System.Reflection;

namespace CoreBDD.Tests
{
    public class CodeGenerationTests
    {
        private readonly string baseFolder = $@"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}..\..\..\..\Output\";

        [Fact]
        public void ScaffoldFeature()
        {
            var feature = "Calculator";
            var targetFolder = $"{baseFolder}{feature}";

            if(!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var featureBuilder = CodeGenerationBuilder.BuildFeature(Path.GetFullPath(targetFolder), feature, "CoreBDD.Tests.Features");
            CodeGeneration.Generate(featureBuilder);
            
            Assert.True(File.Exists($@"{targetFolder}\{feature}Feature.cs"));

           Teardown();
        }

        [Fact]
        public void ScaffoldScenario()
        {
            var feature = "Calculator";
            var scenario = "AdvancedCalculator";
            var featureFolder = $@"{baseFolder}{feature}";
            var scenarioFolder = $@"{featureFolder}\Scenarios";

            if(!Directory.Exists(scenarioFolder))
                Directory.CreateDirectory(scenarioFolder);

            //Feature.cs file
            var featureBuilder = CodeGenerationBuilder.BuildFeature(Path.GetFullPath(featureFolder), feature, "CoreBDD.Tests.Features");
            CodeGeneration.Generate(featureBuilder);

             var scenarioBuilder = CodeGenerationBuilder.BuildScenario(Path.GetFullPath(scenarioFolder), scenario ,feature, "CoreBDD.Tests.Features");
            CodeGeneration.Generate(scenarioBuilder);

            Assert.True(File.Exists($@"{scenarioFolder}\{scenario}.cs"));

            Teardown();
        }

        [Fact]
        public void ScaffoldTestFromGherkin()
        {
            var @namespace = "CoreBDD.Tests.Features";
            var gherkinDocuments = $@"{Directory.GetParent(Assembly.GetExecutingAssembly().Location)}..\..\..\..\Input";

            var testBuilder = CodeGenerationBuilder.BuildTests(gherkinDocuments, Path.Combine(baseFolder, "Calculator"), @namespace);
            CodeGeneration.Generate(testBuilder);

            Assert.True(File.Exists($@"{baseFolder}\Calculator\CalculatorFeature.cs"));
            Assert.True(File.Exists($@"{baseFolder}\Calculator\Scenarios\Calculator.cs"));

            Teardown();
        }
       
        private void Teardown()
        {
            Directory.Delete(baseFolder, true);
        }
    }
}
