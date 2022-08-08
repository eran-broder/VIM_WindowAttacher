using CommandLine;
using WindowAttacher;
using WindowAttacherLib;

try
{
    Parser.Default.ParseArguments<CommandLineOptions>(args)
        .WithParsed(AttachWindow)
        .WithNotParsed(HandleParseError);

}
catch (Exception e) //TODO: is this necessary?
{
    Console.Error.WriteLine(e.Message);
    Environment.Exit(1);
}

static void HandleParseError(IEnumerable<Error> errs)
{
    errs.ToList().ForEach(Console.WriteLine);
    Console.WriteLine(errs);
}

void AttachWindow(CommandLineOptions args)
{
    /*Attacher.Attach(new IntPtr(args.Window), 
        new IntPtr(args.Leech), 
        (args.XOffset, args.YOffset));*/
}