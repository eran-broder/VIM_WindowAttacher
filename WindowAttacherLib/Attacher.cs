using System.Runtime.InteropServices;
using Vigor.Windows.Native;

namespace WindowAttacherLib
{
    public static class Attacher
    {


        public static Action AttachAndTrack(IntPtr anchoredWindow, IntPtr leechWindow, (int, int) relativePosition)
        {
            var threadId = Native.GetWindowThreadProcessId(anchoredWindow, out var processId);

            void OnClosed(IntPtr hwineventhook, uint eventtype, IntPtr hwnd, int idobject, int idchild, uint dweventthread, uint dwmseventtime)
            {
                if (hwnd == anchoredWindow)
                {
                    //TODO: we should inform the calling process that the window of interest is no longer available
                }
            }

            void Reposition()
            {
                Native.GetWindowRect(anchoredWindow, out var rect);

                var childOfAnchor = Native.GetWindow(anchoredWindow, Native.GetWindowType.GW_CHILD);
                var nextOfAnchor = Native.GetWindow(anchoredWindow, Native.GetWindowType.GW_HWNDNEXT);
                var prevOfAnchor = Native.GetWindow(anchoredWindow, Native.GetWindowType.GW_HWNDPREV);

                Console.WriteLine($"child [{childOfAnchor}] [{NativeWrappers.GetWindowTitle(childOfAnchor)}]");
                Console.WriteLine($"next [{nextOfAnchor}] [{NativeWrappers.GetWindowTitle(nextOfAnchor)}]");
                Console.WriteLine($"prev [{prevOfAnchor}] [{NativeWrappers.GetWindowTitle(prevOfAnchor)}]");

                Native.SetWindowPos(
                    leechWindow,
                    new IntPtr(Native.SetWindowPosInsertAfter.HWND_TOPMOST),
                    //windowBelowAnchor,
                    rect.left,
                    rect.top,
                    0, 0,
                    Native.SetWindowPosFlags.IgnoreResize | Native.SetWindowPosFlags.DoNotChangeOwnerZOrder);

                //We made the window top most so it will appear no matter what. now it is time to remove this privilege.
                Native.SetWindowPos(
                    leechWindow,
                    new IntPtr(Native.SetWindowPosInsertAfter.HWND_NOTOPMOST),
                    0,
                    0,
                    0, 0,
                    Native.SetWindowPosFlags.IgnoreResize | Native.SetWindowPosFlags.IgnoreMove);
            }

            Native.WinEventDelegate onEvent = (hook, type, hwnd, idObject, child, thread, time) => Reposition();

            return EventHookWrapperFactory.Create(processId, threadId, Native.WinEventFlags.WINEVENT_OUTOFCONTEXT)
                .Add(Native.WinEvents.EVENT_OBJECT_LOCATIONCHANGE, onEvent)
                .Add(Native.WinEvents.EVENT_OBJECT_SHOW, onEvent)
                .Add(Native.WinEvents.EVENT_OBJECT_DESTROY, OnClosed)
                .Start();
        }


        public static Action AttachAndTrack2(IntPtr anchoredWindow, IntPtr leechWindow, (int, int) relativePosition)
        {
            AssertValidHandles(anchoredWindow, leechWindow);

            void OnEvent(IntPtr hwineventhook, uint eventtype, IntPtr hwnd, int idobject, int idchild, uint dweventthread, uint dwmseventtime)
            {
                Native.GetWindowRect(anchoredWindow, out var rect);
                Native.SetWindowPos(
                    leechWindow,
                    new IntPtr(Native.SetWindowPosInsertAfter.HWND_TOPMOST),
                    rect.left,
                    rect.top,
                    0, 0,
                    Native.SetWindowPosFlags.IgnoreResize | Native.SetWindowPosFlags.DoNotChangeOwnerZOrder);

                //We made the window top most so it will appear no matter what. now it is time to remove this privilege.
                Native.SetWindowPos(
                    leechWindow,
                    new IntPtr(Native.SetWindowPosInsertAfter.HWND_NOTOPMOST),
                    0,
                    0,
                    0, 0,
                    Native.SetWindowPosFlags.IgnoreResize | Native.SetWindowPosFlags.IgnoreMove);
            }

            var threadId = Native.GetWindowThreadProcessId(anchoredWindow, out var processId);

            var hookId = Native.SetWinEventHook(
                Native.WinEvents.EVENT_OBJECT_DESTROY,
                Native.WinEvents.EVENT_OBJECT_LOCATIONCHANGE, 
                IntPtr.Zero, 
                OnEvent, 
                processId, 
                threadId,
                Native.WinEventFlags.WINEVENT_OUTOFCONTEXT);

            return () => Native.UnhookWinEvent(hookId);
        }

        private static void AssertValidHandles(IntPtr anchoredWindow, IntPtr leechWindow)
        {
            void Assert(IntPtr win, string handleName) => NativeAssertions.Assert(Native.IsWindow(win), $"'{handleName}' handle, of value '{win}' is not a window");

            Assert(leechWindow, nameof(leechWindow));
            Assert(anchoredWindow, nameof(anchoredWindow));
            NativeAssertions.AssertNonZero(leechWindow, "Cannot leech the desktop itself");
            NativeAssertions.AssertNonZero(anchoredWindow, "Cannot leech to the desktop itself");
        }
    }
}
