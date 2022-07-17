namespace Vigor.Functional.EventProvider
{
    namespace EventProvider
    {
        

        public interface IEventProviderEmpty : IAcceptingHandlers<IEventProviderReadyToStart>
        {

        }

        public interface IEventProviderReadyToStart : IAcceptingHandlers<IEventProviderReadyToStart>
        {
            IEventProviderActive Start(CancellationToken token);
        }

        public interface IEventProviderActive : IAcceptingHandlers<IEventProviderActive>
        {

        }

        public delegate Action InitializationHandler(Action eventInvoke, CancellationToken token);

        public class EventProvider
        {
            public static IEventProviderEmpty Create(InitializationHandler initializer)
            {
                return new EventProviderEmpty(initializer);
            }
        }

        public record EventProviderEmpty(InitializationHandler Initializer) : IEventProviderEmpty
        {
            public IEventProviderReadyToStart AddHandler(Action handler)
            {
                return new CancelableEventProviderWithHandlers(Initializer, new[] {handler});
            }
        }


        public class CancelableEventProviderBase<TNextState>
        {
            protected InitializationHandler Initializer;
            protected List<Action> Handlers;

            public CancelableEventProviderBase(InitializationHandler initializer, IEnumerable<Action> handlers)
            {
                Initializer = initializer;
                Handlers = handlers.ToList();
            }

            public TNextState AddHandler(Action handler, TNextState returnValue)
            {
                Handlers.Add(handler);
                return returnValue;
            }
        }

        public class CancelableEventProviderWithHandlers :
            CancelableEventProviderBase<IEventProviderReadyToStart>, IEventProviderReadyToStart
        {
            public CancelableEventProviderWithHandlers(InitializationHandler initializer, IEnumerable<Action> handlers)
                : base(initializer, handlers)
            {
            }



            public IEventProviderActive Start(CancellationToken token)
            {
                void InvokeAllHandlers() => Handlers.ForEach(h => h.Invoke());
                var cancelAction = Initializer.Invoke(InvokeAllHandlers, token);
                return new CancelableEventProviderActive(cancelAction, Handlers);
            }

            public IEventProviderReadyToStart AddHandler(Action handler)
            {
                return this.AddHandler(handler, this);
            }
        }

        public record CancelableEventProviderActive
            (Action CancelAction, List<Action> Handlers) : IEventProviderActive
        {
            public IEventProviderActive AddHandler(Action handler)
            {
                return this with {Handlers = Handlers.Concat(new[] {handler}).ToList()};
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }
        }
    }

}
