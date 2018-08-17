using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.ProjectModel;
using static System.Console;

namespace CoreBDD.CommandLine.Tools.Analyzer
{
    /// <remarks>
    // Copyright (c) 2018 Jerrie Pelser Blog
    // https://github.com/jerriepelser-blog/AnalyzeDotNetProject/blob/master/LICENSE
     /// </remarks>
    public static class Analyze
    {
        public static bool IsCoreBDD(string projectPath)
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            return dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference 
                                            && p.TargetFrameworks.Any(f=> 
                                               f.Dependencies.Any(d=> d.Name == "CoreBDD"))
                                            ).Any();
        }

        public static List<string> Solution(string projectPath)
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            foreach(var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                // Generate lock file
                var lockFileService = new LockFileService();
                var lockFile = lockFileService.GetLockFile(project.FilePath, project.RestoreMetadata.OutputPath);

                WriteLine(project.Name);
                
                foreach(var targetFramework in project.TargetFrameworks)
                {
                    WriteLine($"  [{targetFramework.FrameworkName}]");

                    var lockFileTargetFramework = lockFile.Targets.FirstOrDefault(t => t.TargetFramework.Equals(targetFramework.FrameworkName));
                    if (lockFileTargetFramework != null)
                    {
                        foreach(var dependency in targetFramework.Dependencies)
                        {
                            var projectLibrary = lockFileTargetFramework.Libraries.FirstOrDefault(library => library.Name == dependency.Name);

                            ReportDependency(projectLibrary, lockFileTargetFramework, 1);
                        }
                    }
                }
            }
            return new List<string>();
        }

        private static void ReportDependency(LockFileTargetLibrary projectLibrary, LockFileTarget lockFileTargetFramework, int indentLevel)
        {
            Write(new String(' ', indentLevel * 2));
            WriteLine($"{projectLibrary.Name}, v{projectLibrary.Version}");

            foreach (var childDependency in projectLibrary.Dependencies)
            {
                var childLibrary = lockFileTargetFramework.Libraries.FirstOrDefault(library => library.Name == childDependency.Id);

                ReportDependency(childLibrary, lockFileTargetFramework, indentLevel + 1);
            }
        }
    }
}