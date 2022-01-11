using MCP.Application.Interfaces;
using MCP.Domain;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Maui_Couchbase_Playground.ViewModels
{
    public class LoginViewModel : BaseNavigationViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                SetPropertyChanged(ref _username, value);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetPropertyChanged(ref _password, value);
            }
        }


        private ICommand _signInCommand;
        public ICommand SignInCommand
        {
            get
            {
                if (_signInCommand == null)
                {
                    _signInCommand = new Command(SignIn);
                }

                return _signInCommand;
            }
        }

        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        private void SignIn()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                AppInstance.User = new UserCredentials
                {
                    Username = Username,
                    Password = Password
                };

                //Navigation.ReplaceRoot<UserProfileViewModel>();
            }
        }
    }
}
