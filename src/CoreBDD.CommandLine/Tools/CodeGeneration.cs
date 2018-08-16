using CoreBDD.CommandLine.Tools.Analyzer;
using Gherkin;
using Humanizer;
using System;
using System.Collections.Generic;
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
            }

            if (!Directory.Exists(builder.OutputPath))
                Directory.CreateDirectory(builder.OutputPath);

            var assembly = Assembly.GetEntryAssembly();
            var withContent = string.IsNullOrWhiteSpace(builder.Content) ? "" : "Empty.";
            var resourceStream = typeof(CodeGeneration).Assembly.GetManifestResourceStream($"CoreBDD.CommandLine.Tools.Templates.{builder.TypeOfContent.ToString()}.{withContent}template");
            var template = new StringBuilder();
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                template.Append(reader.ReadToEnd());
            };
            template.Replace("[Name]", builder.Name);
            template.Replace("[Namespace]", builder.Namespace);
            template.Replace("[Token2]", token2);
            template.Replace("[Content]", builder.Content);

            File.WriteAllText(Path.Combine(builder.OutputPath, builder.Name + (builder.TypeOfContent == GenerateContent.Feature ? "Feature" : "") + ".cs"), template.ToString());
        }

        private static void GenerateSpecs(CodeGenerationBuilder builder)
        {
            var assemblies = FindCoreBDDAssemblies.Find(builder.Path);
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
            //if path endds with .feature then process on file, else its a folder so scan
            var featureFiles = new List<string>();
            if(builder.Path.EndsWith(".feature"))
            {
                featureFiles.Add(builder.Path);
            }
            else
            {
                featureFiles.AddRange(System.IO.Directory.GetFiles(builder.Path, "*.feature", SearchOption.AllDirectories));
            };
            var scenarios = new StringBuilder();
            var parser = new Parser();
            foreach (var file in featureFiles)
            {
                var gherkinDocument = parser.Parse(file);
                var scenarioFolder = $@"{builder.OutputPath}{gherkinDocument.Feature.Name}\Scenarios";

                var featureBuilder = CodeGenerationBuilder.BuildFeature(builder.OutputPath, gherkinDocument.Feature.Name, builder.Namespace);
                CodeGeneration.Generate(featureBuilder);

                foreach (var s in gherkinDocument.Feature.Children)
                {
                    scenarios.AppendLine($"\r\n\t\t\t[Scenario(\"" + s.Name + "\")]");
                    scenarios.AppendLine($"\t\t\tpublic void {s.Name.Pascalize().Dehumanize()}()");
                    scenarios.AppendLine("\t\t\t{");
                    // scenarios.AppendLine(s.Description);
                    foreach (var step in s.Steps)
                    {
                        scenarios.AppendLine($"\t\t\t\t{step.Keyword.Trim()}($\"" + step.Text + "\",   () => { });");
                    }
                    scenarios.AppendLine("\t\t\t}");
                }

                var scenarioBuilder = CodeGenerationBuilder.BuildScenario(scenarioFolder, gherkinDocument.Feature.Name, gherkinDocument.Feature.Name, builder.Namespace)
                                                            .WithContent(scenarios.ToString());

                CodeGeneration.Generate(scenarioBuilder);
                scenarios.Clear();
            }
        }
    }
}