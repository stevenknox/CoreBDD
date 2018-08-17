using System.IO;
using NuGet.ProjectModel;

namespace CoreBDD.CommandLine.Tools.Analyzer
{
    /// <remarks>
    // Copyright (c) 2018 Jerrie Pelser Blog
    // https://github.com/jerriepelser-blog/AnalyzeDotNetProject/blob/master/LICENSE
    /// </remarks>
    public class LockFileService
    {
        public LockFile GetLockFile(string projectPath, string outputPath)
        {
            // Run the restore command (tests need a build so lets do that too)
            var dotNetRunner = new DotNetRunner();
            string[] arguments = new[] {"build", $"\"{projectPath}\""};
            var runStatus = dotNetRunner.Run(Path.GetDirectoryName(projectPath), arguments);

            // Load the lock file
            string lockFilePath = Path.Combine(outputPath, "project.assets.json");
            return LockFileUtilities.GetLockFile(lockFilePath, NuGet.Common.NullLogger.Instance);
        }
    }
}