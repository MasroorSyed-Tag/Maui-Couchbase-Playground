using MCP.Application.Interfaces;
using MCP.Application.MVVM;
using System.Threading.Tasks;

namespace Maui_Couchbase_Playground.ViewModels
{
    public abstract class BaseNavigationViewModel : BaseViewModel
    {
        protected INavigationService Navigation { get; set; }

        protected BaseNavigationViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        public Task Dismiss() => Navigation.PopAsync();
    }
}
