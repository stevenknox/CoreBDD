using System;
using System.Collections.Generic;
using System.Reflection;

namespace CoreBDD.CommandLine.Tools
{
    public class CodeGenerationBuilder
    {
        public static CodeGenerationBuilder BuildSpecs(string pathToAssemblies, string outputPath)
        {
            return new CodeGenerationBuilder
            {
                TypeOfContent = GenerateContent.Specs,
                Path = pathToAssemblies,
                OutputPath = outputPath
            };
        }

        public static CodeGenerationBuilder BuildSpecs(string outputPath)
        {
            return new CodeGenerationBuilder
            {
                TypeOfContent = GenerateContent.Specs,
                OutputPath = outputPath
            };
        }

        public static CodeGenerationBuilder Build(string typeOfContent, string pathToAssemblies, string outputPath, string name, string @namespace, string parent = "")
        {
            return new CodeGenerationBuilder
            {
                TypeOfContent = (GenerateContent)Enum.Parse(typeof(GenerateContent), typeOfContent, true),
                Path = pathToAssemblies,
                OutputPath = outputPath,
                Name = name,
                Namespace = @namespace,
                Parent = parent
            };
        }

        public static CodeGenerationBuilder BuildFeature(string outputPath, string name, string @namespace)
        {
            return new CodeGenerationBuilder
            {
                TypeOfContent = GenerateContent.Feature,
                OutputPath = outputPath,
                Name = name,
                Namespace = @namespace,
            };
        }

        public static CodeGenerationBuilder BuildScenario(string outputPath, string name, string feature, string @namespace)
        {
            return new CodeGenerationBuilder
            {
                TypeOfContent = GenerateContent.Scenario,
                OutputPath = outputPath,
                Name = name,
                Namespace = @namespace,
                Parent = feature
            };
        }

        public static CodeGenerationBuilder BuildTests(string pathToFeatures, string outputPath, string @namespace)
        {
            return new CodeGenerationBuilder
            {
                TypeOfContent = GenerateContent.Tests,
                OutputPath = outputPath,
                Path = pathToFeatures,
                Namespace = @namespace,
            };
        }

        public CodeGenerationBuilder WithContent(string content)
        {
            Content = content;
            return this;
        }

        public CodeGenerationBuilder WithAssemblies(List<string> assemblies)
        {
            Assemblies = assemblies;
            return this;
        }

        public GenerateContent TypeOfContent { get; set; }
        public string Path { get; set; }
        public string OutputPath { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Content { get; set; }
        public string Namespace { get; set; }
        public List<string> Assemblies { get; set; }
    }
}