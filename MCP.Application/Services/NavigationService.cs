using MCP.Application.Interfaces;
using MCP.Application.MVVM;
using System.Reflection;
using Microsoft.Maui.Controls;

namespace MCP.Application.Services
{
    public class NavigationService : INavigationService
    {
        INavigation FormsNavigation => Microsoft.Maui.Controls.Application.Current.MainPage.Navigation;

        readonly Dictionary<Type, Type> _viewModelViewDictionary = new Dictionary<Type, Type>();

        public NavigationService()
        {
        }

        public void AutoRegister(Assembly asm)
        {
            foreach (var type in asm.DefinedTypes.Where(dt => !dt.IsAbstract &&
                        dt.ImplementedInterfaces.Any(ii => ii == typeof(IViewFor))))
            {
                var viewForType = type.ImplementedInterfaces.FirstOrDefault(
                    ii => ii.IsConstructedGenericType &&
                    ii.GetGenericTypeDefinition() == typeof(IViewFor<>));

                Register(viewForType.GenericTypeArguments[0], type.AsType());

                ServiceContainer.Register(viewForType.GenericTypeArguments[0], viewForType.GenericTypeArguments[0], true);
            }
        }

        public void Register(Type viewModelType, Type viewType)
        {
            if (!_viewModelViewDictionary.ContainsKey(viewModelType))
            {
                _viewModelViewDictionary.Add(viewModelType, viewType);
            }
        }

        public Task PopAsync() => FormsNavigation.PopAsync(true);

        public Task PopToRootAsync(bool animate) => FormsNavigation.PopToRootAsync(animate);

        public Task PushAsync(BaseViewModel viewModel) => FormsNavigation.PushAsync((Page)InstantiateView(viewModel));

        public void ReplaceRoot<T>(bool withNavigationEnabled = true) where T : BaseViewModel
        {
            ReplaceRoot(ServiceContainer.GetInstance<T>(), withNavigationEnabled);
        }

        public void ReplaceRoot(BaseViewModel viewModel, bool withNavigationEnabled = true)
        {
            if (InstantiateView(viewModel) is Page view)
            {
                if (withNavigationEnabled)
                {
                    Microsoft.Maui.Controls.Application.Current.MainPage = new NavigationPage(view);
                }
                else
                {
                    Microsoft.Maui.Controls.Application.Current.MainPage = view;
                }
            }
        }

        IViewFor InstantiateView(BaseViewModel viewModel)
        {
            var viewModelType = viewModel.GetType();

            var viewType = _viewModelViewDictionary[viewModelType];

            var view = (IViewFor)Activator.CreateInstance(viewType);

            view.ViewModel = viewModel;

            return view;
        }
    }
}
