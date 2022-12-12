using Parcs.Module.CommandLine;
using CommandLine;

namespace QuickSort
{
    public class CommandLineOptions : BaseModuleOptions
    {
        [Option("if", Required = true, HelpText = "File path to the input array.")]
        public string InputFile { get; set; }
        [Option("of", Required = true, HelpText = "File path to the output array.")]
        public string OutputFile { get; set; }
        [Option("p", Required = true, HelpText = "Number of points.")]
        public int PointsNum { get; set; }        
    }
}
