using CommandLine;
using System.IO;

namespace nqrgen
{
    public class Options
    {
        [Option('r', "render", HelpText = "Render, [Console|File]")]
        public RenderType RenderType { get; set; }

        [Option('d', "dir", HelpText = "Directory")]
        public DirectoryInfo Directory { get; set; }

        [Option('s', "size", HelpText = "Size", Default = 1)]
        public int Size { get; set; }

        [Option('m', "margin", HelpText = "Margin", Default = 0)]
        public int Margin { get; set; }

        [Option('c', "correction", HelpText = "Error Correction", Default = ErrorCorrection.Low)]
        public ErrorCorrection Correction { get; set; }

        [Option('v', "value", HelpText = "Value")]
        public string Content { get; set; }

    }

    public enum ErrorCorrection
    {
        Low = 0, Medium, Quite, High
    }

    public enum RenderType
    {
        Console = 0,
        File = 1
    }
}
