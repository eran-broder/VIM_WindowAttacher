using System.Runtime.InteropServices;
using Vigor.Windows.Native;

namespace WindowAttacherLib
{
    public static class Attacher
    {
        //TODO: make sure the calling thread has a message loop
        public static Action Attach(IntPtr anchoredWindow, IntPtr leechWindow, (int, int) relativePosition)
        {
            var style = Native.GetWindowLongPtr(leechWindow, (int)Native.WindowLongFlags.GWL_STYLE);
            var newStyle = new IntPtr((int)style | (int)Native.WindowStyles.WS_CHILD);
            Native.SetWindowLongPtr(leechWindow, (int)Native.WindowLongFlags.GWL_STYLE, newStyle);

            NativeAssertions.AssertNotNullWithLastError(()=>Native.SetParent(leechWindow, anchoredWindow));

            Native.SetWindowPos(
                leechWindow,
                IntPtr.Zero,
                relativePosition.Item1,
                relativePosition.Item2,
                0, 0,
                Native.SetWindowPosFlags.IgnoreResize);

            var anchoredThread = Native.GetWindowThreadProcessId(anchoredWindow, out var anchoredProcessId);
            return () => { };
        }
    }
}
