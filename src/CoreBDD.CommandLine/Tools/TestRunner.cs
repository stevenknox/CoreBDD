using CoreBDD.CommandLine.Tools.Analyzer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static System.Console;

namespace CoreBDD.CommandLine.Tools
{
    public static class TestRunner
    {
        public static int Run(bool generateSpecs = false, string path = "", string output = "")
        {
            if (string.IsNullOrWhiteSpace(path)) path = Directory.GetCurrentDirectory();

            var assembliesToTest = new List<string>();
                        
            if (string.IsNullOrEmpty(path) || path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) == false)
            {
                //find assemblies using xUnit and CoreBDD in the solution
                assembliesToTest.AddRange(FindCoreBDDAssemblies.Find(path));
            }
            else
            {
                assembliesToTest.Add(path);
            }

            foreach (var assemblyPath in assembliesToTest)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                try
                {
                    var name = assembly.GetName().Name;

                    ForegroundColor = ConsoleColor.Green;
                    WriteLine($"Discovering tests in {name}");
                    ResetColor();

                    Xunit.ConsoleClient.Program.Main(new string[] { assembly.Location });

                    if (generateSpecs)
                    {
                        CodeGeneration.Generate(CodeGenerationBuilder.BuildSpecs(output).WithAssemblies(assembliesToTest));
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine($"Finished Tests for {name}");
                        ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine($"Something went wrong - {ex.Message}");
                    ResetColor();
                }

            }

            return 0;
        }
    }
 }