namespace Vigor.Functional.EventProvider
{
    namespace EventProviderGeneric
    {

        public interface IAcceptingHandlers<T, TNextState> where TNextState : IAcceptingHandlers<T, TNextState>
        {
            public TNextState AddHandler(Action<T> handler);
        }

        public interface IEventProviderEmpty<T> : IAcceptingHandlers<T, IEventProviderReadyToStart<T>>
        {
            IEventProviderReadyToStart<T> AllowEmptyStart();
        }


        public interface
            IEventProviderReadyToStart<T> : IAcceptingHandlers<T, IEventProviderReadyToStart<T>>
        {
            IEventProviderActive<T> Start(CancellationToken token);
        }

        public interface
            IEventProviderActive<T> : IAcceptingHandlers<T, IEventProviderActive<T>>
        {

        }

        public delegate void InitializationHandler<out T>(Action<T> eventInvoke, CancellationToken token);

        public class EventProvider<T>
        {
            public static IEventProviderEmpty<T> Create(InitializationHandler<T> initializer)
            {
                return new EventProviderEmpty<T>(initializer);
            }
        }

        public record EventProviderEmpty<T>
            (InitializationHandler<T> Initializer) : IEventProviderEmpty<T>
        {
            public IEventProviderReadyToStart<T> AddHandler(Action<T> handler)
            {
                return new EventProviderWithHandlers<T>(Initializer, new []{handler});
            }

            public IEventProviderReadyToStart<T> AllowEmptyStart()
            {
                return new EventProviderWithHandlers<T>(Initializer, Array.Empty<Action<T>>());
            }
        }

        public class EventProviderWithHandlersBase<T>
        {
            protected IEnumerable<Action<T>> _handlers;

            public EventProviderWithHandlersBase(IEnumerable<Action<T>> handlers)
            {
                _handlers = handlers;
            }
        }

        public class EventProviderWithHandlers<T>: EventProviderWithHandlersBase<T>, IEventProviderReadyToStart<T>
        {
            private readonly InitializationHandler<T> _initializer;

            public EventProviderWithHandlers(InitializationHandler<T> initializer, Action<T>[] handlers) : base(handlers)
            {
                _initializer = initializer;
            }

            public IEventProviderReadyToStart<T> AddHandler(Action<T> handler)
            {
                _handlers = _handlers.Append(handler);
                return this;
            }

            public IEventProviderActive<T> Start(CancellationToken token)
            {
                void InvokeAllHandlers(T arg) => _handlers.ToList().ForEach(h => h.Invoke(arg));
                _initializer.Invoke(InvokeAllHandlers, token);
                return new EventProviderActive<T>(_handlers);
            }
        }

        public class EventProviderActive<T>: EventProviderWithHandlersBase<T>, IEventProviderActive<T>
        {

            public EventProviderActive(IEnumerable<Action<T>> handlers) : base(handlers)
            {

            }

            public IEventProviderActive<T> AddHandler(Action<T> handler)
            {
                _handlers = _handlers.Append(handler);
                return this;
            }
        }


    }

}

