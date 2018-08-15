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
                PathToAssemblies = pathToAssemblies,
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
                PathToAssemblies = pathToAssemblies,
                OutputPath = outputPath,
                Name = name,
                Namespace = @namespace,
                Parent = parent
            };
        }

        public CodeGenerationBuilder WithAssemblies(List<string> assemblies)
        {
            Assemblies = assemblies;
            return this;
        }

        public GenerateContent TypeOfContent { get; set; }
        public string PathToAssemblies { get; set; }
        public string OutputPath { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Namespace { get; set; }
        public List<string> Assemblies { get; set; }
    }
}