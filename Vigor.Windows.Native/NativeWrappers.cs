using System.Text;

namespace Vigor.Windows.Native;

public static class NativeWrappers
{
    public static Native.POINT GetCursorPos()
    {
        NativeAssertions.AssertNonZero(Native.GetCursorPos(out var point));
        return point;
    }

    public static string GetClassName(IntPtr window)
    {
        int nRet;
        // Pre-allocate 256 characters, since this is the maximum class name length.
        var className = new StringBuilder(256);
        //Get the window class name
        nRet = Native.GetClassName(window, className, className.Capacity);
        NativeAssertions.AssertNonZero(nRet, "Failed to fetch class name for window"); //TODO: really? debug assertion probably more suited here
        return className.ToString();
    }


    public static IReadOnlyList<IntPtr> GetChildWindows(IntPtr window)
    {
        return GetChildWindowsInternal(window).ToList();
    }

    private static IEnumerable<IntPtr> GetChildWindowsInternal(IntPtr window)
    {
        var child = Native.GetWindow(window, Native.GetWindowType.GW_CHILD);
        if (child != IntPtr.Zero)
        {
            yield return child;
            var sibling = Native.GetWindow(child, Native.GetWindowType.GW_HWNDNEXT);
            while (sibling != IntPtr.Zero)
            {
                yield return sibling;
                sibling = Native.GetWindow(sibling, Native.GetWindowType.GW_HWNDNEXT);
            }
        }
    }

    public static string GetWindowTitle(IntPtr handle)
    {
        int length = Native.GetWindowTextLength(handle);
        StringBuilder sb = new StringBuilder(length + 1);
        Native.GetWindowText(handle, sb, sb.Capacity);
        return sb.ToString();
    }

}