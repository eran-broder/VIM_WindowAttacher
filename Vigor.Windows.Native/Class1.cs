using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vigor.Functional;

namespace Vigor.Windows.Native
{
    public static class EventHookWrapperFactory
    {

        public interface IHookPrecursor
        {
            BoundedEvent Add(Native.WinEvents eventId, Native.WinEventDelegate callback);
        }

        public record EmptyPrecursor(uint idProcess, uint idThread, Native.WinEventFlags dwFlags) : IHookPrecursor
        {
            public BoundedEvent Add(Native.WinEvents eventId, Native.WinEventDelegate callback)
            {
                return new(idProcess, idThread, dwFlags ,new Dictionary<Native.WinEvents, Native.WinEventDelegate>()
                {
                    {
                        eventId, callback
                    }
                });
            }
        }

        public static EmptyPrecursor Create(uint idProcess, uint idThread, Native.WinEventFlags dwFlags)
        {
            return new (idProcess, idThread, dwFlags);
        }

        public record BoundedEvent(uint idProcess, uint idThread, Native.WinEventFlags dwFlags, IReadOnlyDictionary<Native.WinEvents, Native.WinEventDelegate> EventList): EmptyPrecursor(idProcess, idThread, dwFlags)
        {
            public BoundedEvent Add(Native.WinEvents eventId, Native.WinEventDelegate callback)
            {
                return this with { EventList = EventList.Add(eventId, callback) };
            }

            public Action Start(Func<IntPtr, bool>? handleFilter = null)
            {

                void OnHookCalled(IntPtr hook, uint type, IntPtr hwnd, int idObject, int child, uint thread, uint time)
                {
                    if (handleFilter == null || handleFilter(hwnd))
                    {
                        //TODO: what do you want to do in case of an exception at the handler level?
                        EventList.GetOrNone((Native.WinEvents)type).MatchSome(callback => callback(hook, type, hwnd, idObject, child, thread, time));
                    }

                }

                var minHook = EventList.Keys.Min();
                var maxHook = EventList.Keys.Max();
                var hookId = Native.SetWinEventHook(
                    minHook,
                    maxHook,
                    IntPtr.Zero,
                    OnHookCalled,
                    idProcess,
                    idThread,
                    Native.WinEventFlags.WINEVENT_OUTOFCONTEXT);

                return () => Native.UnhookWinEvent(hookId);
            }
        }
    }
}
