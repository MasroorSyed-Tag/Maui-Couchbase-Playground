using MCP.Application.MVVM;

namespace MCP.Application.Interfaces
{
    public interface IViewFor
    {
        object ViewModel { get; set; }
    }

    public interface IViewFor<T> : IViewFor where T : BaseViewModel
    {
        new T ViewModel { get; set; }
    }
}
