using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigor.Functional
{
    public static class Safe
    {

        public static void Force(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        public static T Try<T>(Func<T> func, Func<Exception, T> onError)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                return onError(e);
            }
        }

        public static void Try(Action func, Action<Exception> onError)
        {
            try
            {
                func();
            }
            catch (Exception e)
            {
                onError(e);
            }
        }
    }
}
