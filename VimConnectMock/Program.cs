// See https://aka.ms/new-console-template for more information

using System.Collections.ObjectModel;
using System.Diagnostics;
using UIAutomationClient;
using Vigor.Windows.Native;

Console.WriteLine("Hello, World!");
var (anchor, leech) = StartChildProcesses();
var info = new ProcessStartInfo(@"C:\work\vim\WindowAttacher\WindowAttacher\bin\Debug\net6.0-windows\WindowAttacher.exe");
info.ArgumentList.Add($"--window={anchor.ToInt32()}");
info.ArgumentList.Add($"--leech={leech.ToInt32()}");
info.ArgumentList.Add($"--xoffset={0}");
info.ArgumentList.Add($"--yoffset={0}");
var started = Process.Start(info);
Console.WriteLine("Process started");
started.WaitForExit();



static (IntPtr anchorWindow, IntPtr leechWindow) StartChildProcesses()
{ 
    Process.Start(@"C:\work\vim\WindowAttacher\EhrMock\bin\Debug\net6.0-windows\EhrMock.exe");
    //Process.Start(@"C:\work\vim\WindowAttacher\VimWidgetMock\bin\Debug\net6.0-windows\VimWidgetMock.exe");
    var start = new ProcessStartInfo
    {
        WorkingDirectory = @"C:\work\vim\WindowAttacher\electron-quick-start",
        FileName = @"C:\Program Files\nodejs\npm.cmd",
        Arguments = "start"
    };
    Process.Start(start);
    Thread.Sleep(TimeSpan.FromSeconds(7));

    var client = new CUIAutomationClass();
    var condition = client.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Vim Button1");
    var anchorWindow = client.GetRootElement().FindFirst(TreeScope.TreeScope_Descendants, condition).CurrentNativeWindowHandle;
    var leechWindow = Native.FindWindow(null, "VimWidget");

    return (anchorWindow, leechWindow);

}