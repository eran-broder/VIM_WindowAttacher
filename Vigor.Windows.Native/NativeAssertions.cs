using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Vigor.Windows.Native;

public static class NativeAssertions
{
    private static void ThrowWithLastError(string msg = "") =>
        throw new Exception($"{msg} [error code : {Marshal.GetLastPInvokeError()}]");

    public static void AssertNonZero(int value, string msg = "")
    {
        AssertNonZero(value != 0, msg);
    }

    public static IntPtr AssertNotNullWithLastError(Func<IntPtr> valueEvaluator)
    {
        var value = valueEvaluator();
        if (value == IntPtr.Zero)
            throw new Exception($"win32 return null. last error is [{Marshal.GetLastWin32Error()}]");
        return value;
    }
    public static void AssertNonZero(bool value, string msg = "")
    {
        if (!value)
            ThrowWithLastError(msg);
    }
    
    [return:NotNull]
    public static T AssertNotNull<T>(T? value, string msg = "") where T : class
    {
        if (value == null)
            throw new Exception($"Value is null [{msg}]");
        return value;
    }
}