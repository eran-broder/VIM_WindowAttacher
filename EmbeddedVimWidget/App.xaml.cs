using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using CommandLine;
using Vigor.Windows.Native;
using WindowAttacherLib;

namespace EmbeddedVimWidget
{

    public class CommandLineOptions
    {
        [Option(Required = true, HelpText = "Handle of the window to be anchored to.")]
        public int Window { get; set; }

        [Option(Required = true, HelpText = "horizontal offset relative to the parent.")]
        public int XOffset { get; set; }

        [Option(Required = true, HelpText = "vertical offset relative to the parent.")]
        public int YOffset { get; set; }

        [Option(Required = true, HelpText = "Width of the button.")]
        public int Width { get; set; }

        [Option(Required = true, HelpText = "Height of the button.")]
        public int Height { get; set; }

        [Option(Required = true, HelpText = "Text to present on the button.")]
        public string? Text { get; set; }
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(e.Args).WithParsed(Start)
                .WithNotParsed(HandleErrors);
            var anchorWindowHandle = Native.FindWindow(null, "Referral (Outgoing)");
            /*var myArgs = new CommandLineOptions()
            {
                Text = "Broder",
                XOffset = 20,
                YOffset = 20,
                Height = 40,
                Width = 100,
                Window = anchorWindowHandle.ToInt32()
            };
            Start(myArgs);*/
        }

        private void HandleErrors(IEnumerable<Error> obj)
        {
            Environment.Exit(1);
        }

        private void Start(CommandLineOptions obj)
        {
            var window = new MainWindow(obj.Text!);
            window.Width = obj.Width;
            window.Height = obj.Height;
            var handle = new WindowInteropHelper(window).EnsureHandle();
            //Attacher.Attach(new IntPtr(obj.Window), handle, (obj.XOffset, obj.YOffset));
            window.Show();
        }
    }
}
