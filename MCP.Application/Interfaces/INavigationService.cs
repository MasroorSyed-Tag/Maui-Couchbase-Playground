using MCP.Application.MVVM;
using System.Reflection;

namespace MCP.Application.Interfaces

{
    public interface INavigationService
    {
        void AutoRegister(Assembly asm);
        void Register(Type viewModelType, Type viewType);
        Task PopAsync();
        Task PushAsync(BaseViewModel viewModel);
        Task PopToRootAsync(bool animate);
        void ReplaceRoot<T>(bool withNavigationEnabled = true) where T : BaseViewModel;
        void ReplaceRoot(BaseViewModel viewModel, bool withNavigationEnabled = true);
    }
}
