using MCP.Application.Interfaces;
using MCP.Application.MVVM;

namespace MCP.Application.Forms
{
    public abstract class BaseContentPage<T> : ContentPage, IViewFor<T> where T : BaseViewModel
    {
        T _viewModel;

        public T ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                BindingContext = _viewModel = value;

                if (_viewModel != null)
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            await _viewModel.InitAsync();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    });
                }
            }
        }

        object IViewFor.ViewModel
        {
            get => _viewModel;
            set => ViewModel = (T)value;
        }

        protected BaseContentPage()
        { }

        async void Init() => await ViewModel.InitAsync();
    }
}
