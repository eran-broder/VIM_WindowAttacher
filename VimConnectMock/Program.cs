// See https://aka.ms/new-console-template for more information

using System.Collections.ObjectModel;
using System.Diagnostics;
using UIAutomationClient;
using Vigor.Windows.Native;

var leechName = "VimWidget";
var anchorName = "Referral (Incoming)";

var leechWindowHandle = Native.FindWindow(null, leechName);
var anchorHandle = Native.FindWindow(null, anchorName);

Console.WriteLine($"A: [{anchorHandle}] L:[{leechWindowHandle}]");

var info = new ProcessStartInfo(@"..\..\..\..\ActiveWindowAttacher\bin\Debug\net6.0-windows\ActiveWindowAttacher.exe");
info.ArgumentList.Add($"--window={anchorHandle.ToInt32()}");
info.ArgumentList.Add($"--leech={leechWindowHandle.ToInt32()}");
info.ArgumentList.Add($"--xoffset={40}");
info.ArgumentList.Add($"--yoffset={40}");
//info.ArgumentList.Add($"--text=HellYa!");
//info.ArgumentList.Add($"--width=120");
//info.ArgumentList.Add($"--height=40");
info.RedirectStandardOutput = true;
var started = Process.Start(info)!;
Console.WriteLine("Process started");

var output = started.StandardOutput;
while (true)
{
    if (started.HasExited)
    {
        Console.WriteLine("PROCESS HAS TERMINATED");
        break;
    }
    var line = output.ReadLine();
    Console.WriteLine(line);
}
