namespace CoreBDD.CommandLine.Tools.Analyzer
{
     /// <remarks>
    // Copyright (c) 2018 Jerrie Pelser Blog
    // https://github.com/jerriepelser-blog/AnalyzeDotNetProject/blob/master/LICENSE
     /// </remarks>
    public class RunStatus
    {
        public string Output { get; }
        public string Errors { get; }
        public int ExitCode { get; }

        public bool IsSuccess => ExitCode == 0;

        public RunStatus(string output, string errors, int exitCode)
        {
            Output = output;
            Errors = errors;
            ExitCode = exitCode;
        }
    }
}