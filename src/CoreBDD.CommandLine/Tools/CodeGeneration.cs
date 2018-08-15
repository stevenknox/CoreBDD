using CoreBDD.CommandLine.Tools.Analyzer;
using Humanizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace CoreBDD.CommandLine.Tools
{
    public enum Generate
    {
        Specs,
        Tests,
        Feature,
        Scenario,
        Test
    }
    public class CodeGeneration
    {
        public static int Generate(Generate type, string pathToAssemblies = "", string output = "")
        {
            var assemblies = FindCoreBDDAssemblies.Find(pathToAssemblies);

            if (assemblies.Any())
                return Generate(type, assemblies, output);

            return 0;

        }
        public static int Generate(Generate type, List<string> assemblies, string output = "")
        {

            switch (type)
            {
                case Tools.Generate.Specs:
                    GenerateSpecs(type, assemblies, output);
                    break;
                case Tools.Generate.Tests:
                    break;
                case Tools.Generate.Feature:
                    GenerateFeature(output);
                    break;
                case Tools.Generate.Scenario:
                    break;
                case Tools.Generate.Test:
                    break;
                default:
                    break;
            }

            return 0;
        }

        private static void GenerateFeature(string output)
        {
            output = output.Pascalize();
            var folder = Directory.GetCurrentDirectory() + "/" + output;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText(Path.Combine(folder, output + ".cs"), "This is a test feature");
        }

        private static void GenerateSpecs(Generate type, List<string> assemblies, string output)
        {
            foreach (var spec in assemblies)
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"Generating {type.ToString()} for {Path.GetFileNameWithoutExtension(spec)}");
                ResetColor();

                if (output == "")
                    output = $"{Directory.GetParent(spec).Parent.Parent.Parent.ToString()}{Path.DirectorySeparatorChar}Specs{Path.DirectorySeparatorChar}";

                SpecGeneration.GenerateSpecs.OutputFeatureSpecs(spec, output);
            }
        }
    }
}