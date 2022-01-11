using MCP.Application.MVVM;

namespace MCP.Application.ViewModels

{
    public abstract class BaseViewModel : BaseNotify
    {
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetPropertyChanged(ref _isBusy, value);
        }

        public virtual Task InitAsync() => Task.FromResult(true);
    }
}
