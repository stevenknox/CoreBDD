using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using CoreBDD.CommandLine.Tools;

namespace CoreBDD.CommandLine
{
    [Command(Description = "CoreBDD command line tool")]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, Description = "Command to run (test, generate etc")]
        [Required]
        public string Task { get; }

        [Option("-p|--path")]
        public string Path { get; }

        [Option("-o|--output")]
        public string Output { get; }

        [Option("-s|--specs")]
        public bool TestWithSpecs { get; set; }

        [Argument(1)]
        public string Generate { get; }

        private int OnExecute()
        {

           switch (Task)
           {
               case "test" : return TestRunner.Run(TestWithSpecs, Path, Output);
               case "generate": return CodeGeneration.Generate((Generate)Enum.Parse(typeof(Generate), Generate, true), Path, Output);
           }
           Console.ReadKey();
           return 0;
        }
    }
}
