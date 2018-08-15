using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace CoreBDD.CommandLine.Tools.Analyzer
{
    public class FindCoreBDDAssemblies
    {
        public static List<string> Find(string path = "")
        {
            //find assemblies using xUnit and CoreBDD in the solution
            var assembliesToTest = new List<string>();
            if(string.IsNullOrWhiteSpace(path)) path = Directory.GetCurrentDirectory();

            string[] projects = System.IO.Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);

            foreach (var proj in projects)
            {
                if (Analyze.IsCoreBDD(proj))
                {
                    //how can we figure out path to the assembly?
                    var fileName = Path.GetFileNameWithoutExtension(proj) + ".dll";
                    var filePath = $"{Path.GetDirectoryName(proj)}{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}Debug{Path.DirectorySeparatorChar}";
                    string[] matchingAssemblies = Directory.GetFiles(filePath, fileName, SearchOption.AllDirectories);

                    assembliesToTest.AddRange(matchingAssemblies);
                }
            }
            if (!assembliesToTest.Any())
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("No CoreBDD Test probjects found");
                ResetColor();
            }
            return assembliesToTest;
        }
    }
}
