namespace Vigor.Functional.EventProvider
{
    public interface ICancelable
    {
        void Cancel();
    }

    public interface IAcceptingHandlers<out TNextState> where TNextState : IAcceptingHandlers<TNextState>
    {
        public TNextState AddHandler(Action handler);
    }

}