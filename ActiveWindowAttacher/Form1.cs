using CommandLine;
using Vigor.Windows.Native;
using WindowAttacherLib;

namespace ActiveWindowAttacher
{
    public partial class VimActiveWindowAttacher : Form
    {
        public VimActiveWindowAttacher()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Go();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Native.SetParent(this.Handle, Native.SetParentSpecial.HWND_MESSAGE);
        }

        private void Go()
        {
            //ExecuteBasedOnCommandLineArgs();
            StandaloneExecute();
        }

        private static void StandaloneExecute()
        {
            var leechName = Console.ReadLine();
            leechName ??= "VimWidget";
            Console.Write("Enter anchor name: ");
            var anchorName = Console.ReadLine();
            anchorName ??= "Referral (Incoming)";

            var leechWindowHandle = Native.FindWindow(null, leechName);
            var anchorHandle = Native.FindWindow(null, anchorName);
            Attacher.AttachAndTrack(anchorHandle, leechWindowHandle, (0, 0));
        }

        private void ExecuteBasedOnCommandLineArgs()
        {
            Parser.Default.ParseArguments<CommandLineOptions>(Environment.GetCommandLineArgs())
                .WithParsed(AttachWindow)
                .WithNotParsed(HandleParseError);
        }

        private void AttachWindow(CommandLineOptions obj)
        {
            Attacher.AttachAndTrack(new IntPtr(obj.Window), new IntPtr(obj.Leech), (obj.XOffset, obj.YOffset));
        }

        private void HandleParseError(IEnumerable<Error> obj)
        {
            Environment.Exit(-1);
            foreach (var error in obj)
            {
                Console.WriteLine(error);
            }
        }
    }
}