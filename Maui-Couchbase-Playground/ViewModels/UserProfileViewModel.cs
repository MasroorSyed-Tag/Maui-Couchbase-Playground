using MCP.Application.Interfaces;
using MCP.Domain;
using MCP.Persistence.Interfaces;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui_Couchbase_Playground.ViewModels
{
    public class UserProfileViewModel : BaseNavigationViewModel
    {
        private IUserProfileRepository UserProfileRepository { get; set; }

        private string UserProfileDocId => $"user::{AppInstance.User.Username}";

        private string _name;
        public string Name
        {
            get => _name;
            set => SetPropertyChanged(ref _name, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetPropertyChanged(ref _email, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetPropertyChanged(ref _address, value);
        }

        private byte[] _imageData;
        public byte[] ImageData
        {
            get => _imageData;
            set => SetPropertyChanged(ref _imageData, value);
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(async () => await Save());
                }

                return _saveCommand;
            }
        }

        ICommand _selectImageCommand;
        public ICommand SelectImageCommand
        {
            get
            {
                if (_selectImageCommand == null)
                {
                    _selectImageCommand = new Command(async () => await SelectImage());
                }

                return _selectImageCommand;
            }
        }

        private ICommand _logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                {
                    _logoutCommand = new Command(Logout);
                }

                return _logoutCommand;
            }
        }


        public UserProfileViewModel(INavigationService navigationService, IUserProfileRepository userProfileRepository) : base(navigationService)
        {
            UserProfileRepository = userProfileRepository;
        }

        public override async Task InitAsync()
        {
            await UserProfileRepository.StartReplicationForCurrentUser(AppInstance.User).ConfigureAwait(false);

            var userProfile = await Task.Run(async () =>
            {
                var up = await UserProfileRepository?.GetAsync(UserProfileDocId, UpdateUserProfile);

                if (up == null)
                {
                    up = new UserProfile
                    {
                        Id = UserProfileDocId,
                        Email = AppInstance.User.Username
                    };
                }

                return up;
            });

            if (userProfile != null)
            {
                UpdateUserProfile(userProfile);
            }
        }

        private void UpdateUserProfile(UserProfile userProfile)
        {
            Name = userProfile.Name;
            Email = userProfile.Email;
            Address = userProfile.Address;
            ImageData = userProfile.ImageData;
        }

        async Task Save()
        {
            var userProfile = new UserProfile
            {
                Id = UserProfileDocId,
                Name = Name,
                Email = Email,
                Address = Address,
                ImageData = ImageData,
            };

            var success = await UserProfileRepository.SaveAsync(userProfile).ConfigureAwait(false);

            if (success)
            {
                //await AlertService.ShowMessage(null, "Successfully updated profile!", "OK");
            }
            else
            {
                //await AlertService.ShowMessage(null, "Error updating profile!", "OK");
            }
        }

        async Task SelectImage()
        {
            //var imageData = await MediaService.PickPhotoAsync();

            //if (imageData != null)
            //{
            //    ImageData = imageData;
            //}
        }

        void Logout()
        {
            UserProfileRepository.Dispose();

            AppInstance.User = null;

            Navigation.ReplaceRoot<LoginViewModel>(false);
        }
    }
}
