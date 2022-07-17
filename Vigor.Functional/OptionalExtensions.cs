namespace Vigor.Functional
{
    public static class FuncExtensions
    {
        public static Func<T, bool> Not<T>(this Func<T, bool> original) => x => !original(x);

        public static T Identity<T>(T x) => x;
    }
}