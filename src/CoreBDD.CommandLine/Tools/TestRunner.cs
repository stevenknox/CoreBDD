using CoreBDD.CommandLine.Tools.Analyzer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xunit.Runners;
using static System.Console;
using static System.Math;

namespace CoreBDD.CommandLine.Tools
{
    public static class TestRunner
    {
        // We use consoleLock because messages can arrive in parallel, so we want to make sure we get
        // consistent console output.
        static object consoleLock = new object();

        // Use an event to know when we're done
        static ManualResetEvent finished = new ManualResetEvent(false);

        // Start out assuming success; we'll set this to 1 if we get a failed test
        static int result = 0;

        public static int Run(bool generateSpecs = false, string path = "", string output = "")
        {
            var assembliesToTest = new List<string>();

            if (string.IsNullOrEmpty(path))
            {
                //find assemblies using xUnit and CoreBDD in the solution
                assembliesToTest.AddRange(FindCoreBDDAssemblies.Find());
            }
            else
            {
                assembliesToTest.Add(path);
            }
            
            foreach (var assemblyPath in assembliesToTest)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                using (var runner = AssemblyRunner.WithoutAppDomain(assembly.Location))
                {
                    var name = assembly.GetName().Name;
                    runner.OnDiscoveryComplete = OnDiscoveryComplete;
                    runner.OnExecutionComplete = OnExecutionComplete;
                    runner.OnTestFailed = OnTestFailed;
                    runner.OnTestSkipped = OnTestSkipped;

                    ForegroundColor = ConsoleColor.Green;
                    WriteLine($"Discovering tests in {name}");
                    ResetColor();
                    runner.Start();

                    finished.WaitOne();
                    finished.Dispose();
                                        
                    if (generateSpecs)
                    {
                        CodeGeneration.Generate(Generate.Specs, assembliesToTest, output);
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine($"Finished Tests for {name}");
                        ResetColor();
                    }
                }
            }

           
            return result;
        }

        static void OnDiscoveryComplete(DiscoveryCompleteInfo info)
        {
            lock (consoleLock)
                WriteLine($"{Environment.NewLine}Running {info.TestCasesToRun} of {info.TestCasesDiscovered} tests...");
        }

        static void OnExecutionComplete(ExecutionCompleteInfo info)
        {
            lock (consoleLock)
                WriteLine($"{Environment.NewLine}Finished: {info.TotalTests} tests in {Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed, {info.TestsSkipped} skipped){Environment.NewLine}");

            finished.Set();
        }

        static void OnTestFailed(TestFailedInfo info)
        {
            lock (consoleLock)
            {
                ForegroundColor = ConsoleColor.Red;

                WriteLine($"{Environment.NewLine}[FAIL] {0}: {1}", info.TestDisplayName, info.ExceptionMessage);
                if (info.ExceptionStackTrace != null)
                    WriteLine(info.ExceptionStackTrace);

                ResetColor();
            }

            result = 1;
        }

        static void OnTestSkipped(TestSkippedInfo info)
        {
            lock (consoleLock)
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine($"{Environment.NewLine}[SKIP] {0}: {1}", info.TestDisplayName, info.SkipReason);
                ResetColor();
            }
        }
    }
}