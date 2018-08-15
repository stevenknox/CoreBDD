using CoreBDD.CommandLine.Tools.Analyzer;
using Gherkin;
using Humanizer;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Console;

namespace CoreBDD.CommandLine.Tools
{
    public class CodeGeneration
    {
        public static int Generate(CodeGenerationBuilder builder)
        {
            switch (builder.TypeOfContent)
            {
                case Tools.GenerateContent.Specs:
                    GenerateSpecs(builder);
                    break;
                case Tools.GenerateContent.Tests:
                case Tools.GenerateContent.Test:
                    GenerateTests(builder);
                    break;
                case Tools.GenerateContent.Feature:
                case Tools.GenerateContent.Scenario:
                    GenerateFromTemplate(builder);
                    break;
                default:
                    break;
            }

            return 0;
        }

        private static void GenerateFromTemplate(CodeGenerationBuilder builder)
        {
            var token2 = "";
            builder.Name = builder.Name.Pascalize();
            builder.Namespace = string.IsNullOrWhiteSpace(builder.Namespace) ? "MyApp" : builder.Namespace.Pascalize();
            var pluralFolderName = builder.TypeOfContent.ToString().Pluralize();
            if (!string.IsNullOrWhiteSpace(builder.Parent) && builder.TypeOfContent == GenerateContent.Scenario)
            {
                token2 = builder.Parent.Titleize().Pascalize();
                pluralFolderName = "/Features/" + token2;
            }
            if (string.IsNullOrWhiteSpace(builder.OutputPath))
            {
                builder.OutputPath = Directory.GetCurrentDirectory() + "/" + (Directory.GetCurrentDirectory().EndsWith(pluralFolderName, StringComparison.OrdinalIgnoreCase) ? "" : $"{pluralFolderName}/") + builder.Name;
                if (!Directory.Exists(builder.OutputPath))
                    Directory.CreateDirectory(builder.OutputPath);

            }
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"CoreBDD.CommandLine.Tools.Templates.{builder.TypeOfContent.ToString()}.template");
            var template = "";
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                template = reader.ReadToEnd();
            };
            template = template.Replace("[Name]", builder.Name);
            template = template.Replace("[Namespace]", builder.Namespace);
            template = string.IsNullOrWhiteSpace(token2) ? template : template.Replace("[Token2]", token2);
            File.WriteAllText(Path.Combine(builder.OutputPath, builder.Name + (builder.TypeOfContent == GenerateContent.Feature ? "Feature" : "") + ".cs"), template);
        }

        private static void GenerateSpecs(CodeGenerationBuilder builder)
        {
            var assemblies = FindCoreBDDAssemblies.Find(builder.PathToAssemblies);
            foreach (var spec in assemblies)
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"Generating {builder.TypeOfContent.ToString()} for {Path.GetFileNameWithoutExtension(spec)}");
                ResetColor();

                if (builder.OutputPath == "")
                    builder.OutputPath = $"{Directory.GetParent(spec).Parent.Parent.Parent.ToString()}{Path.DirectorySeparatorChar}Specs{Path.DirectorySeparatorChar}";

                SpecGeneration.GenerateSpecs.OutputFeatureSpecs(spec, builder.OutputPath);
            }
        }

        private static void GenerateTests(CodeGenerationBuilder builder)
        {
            //path == gherkin feature or folder with .features
            var parser = new Parser();
            var gherkinDocument = parser.Parse(@"C:\Users\Steven\Source\CoreBDD\samples\Specs\CalculatorFeature.feature");
            //output class (will append, or create of doesnt exist)

            WriteLine($"Generating test for " + gherkinDocument.Feature.Name);
        }
    }
}