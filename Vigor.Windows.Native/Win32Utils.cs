using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigor.Windows.Native
{
    public class Win32Utils
    {
        public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                Native.EnumThreadWindows(thread.Id,
                    (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        public static void AddExtendedStyle(IntPtr windowHandle, int newStyleToAdd)
        {
            var styleFlag = (int)Native.WindowLongFlags.GWL_EXSTYLE;
            int exStyle = (int)Native.GetWindowLongPtr(windowHandle, styleFlag);

            exStyle |= newStyleToAdd;
            Native.SetWindowLongPtr(windowHandle, styleFlag, (IntPtr)exStyle);
        }
    }
}
