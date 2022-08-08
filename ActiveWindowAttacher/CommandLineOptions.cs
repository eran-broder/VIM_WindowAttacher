using CommandLine;

namespace ActiveWindowAttacher;

public class CommandLineOptions
{
    [Option(Required = true, HelpText = "Handle of the window to be anchored to.")]
    public int Window { get; set; }

    [Option(Required = true, HelpText = "Handle of the leech window.")]
    public int Leech { get; set; }

    [Option(Required = true, HelpText = "horizontal offset relative to the parent.")]
    public int XOffset { get; set; }

    [Option(Required = true, HelpText = "vertical offset relative to the parent.")]
    public int YOffset { get; set; }
}