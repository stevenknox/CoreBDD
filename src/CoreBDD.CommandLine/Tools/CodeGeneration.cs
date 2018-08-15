using CoreBDD.CommandLine.Tools.Analyzer;
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
                    GenerateFeature(builder);
                    break;
                case Tools.GenerateContent.Scenario:
                    break;
                default:
                    break;
            }

            return 0;
        }

        private static void GenerateFeature(CodeGenerationBuilder builder)
        {
            var type = "feature";
            builder.Name = builder.Name.Pascalize();
            builder.Namespace = string.IsNullOrWhiteSpace(builder.Namespace) ? "MyApp" : builder.Namespace.Pascalize();
            if (string.IsNullOrWhiteSpace(builder.OutputPath))
            {
                builder.OutputPath = Directory.GetCurrentDirectory() + "/" + (Directory.GetCurrentDirectory().EndsWith("Features", StringComparison.OrdinalIgnoreCase) ? "" : "Features/") + builder.Name;
                if (!Directory.Exists(builder.OutputPath))
                    Directory.CreateDirectory(builder.OutputPath);

            }
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"EmbeddedResource.Tools.Templates.{type}.template");
            var template = "";
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                template = reader.ReadToEnd();
            };
            template = template.Replace("[Feature]", builder.Name);
            template = template.Replace("[Namesace]", builder.Namespace);
            File.WriteAllText(Path.Combine(builder.OutputPath, builder.Name + ".cs"), template);
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
            //output class (will append, or create of doesnt exist)

            WriteLine($"Generating Tests from {builder.PathToAssemblies}");
        }
    }
}