using Maui_Couchbase_Playground.ViewModels;
using MCP.Application.Interfaces;
using MCP.Application.MVVM;
using MCP.Application.Services;
using MCP.Persistence;
using MCP.Persistence.Interfaces;
using Microsoft.Maui.Controls;
using System;

namespace Maui_Couchbase_Playground
{
    public partial class App : Application
    {
        private INavigationService NavigationService { get; set; }

        public App()
        {
            InitializeComponent();

            RegisterRepositories();

            RegisterServices();

            NavigationService.ReplaceRoot(ServiceContainer.GetInstance<LoginViewModel>(), false);
        }

        private void RegisterRepositories()
        {
            ServiceContainer.Register<IUserProfileRepository>(new UserProfileRepository());
        }

        private void RegisterServices()
        {
            NavigationService = new NavigationService();
            NavigationService.AutoRegister(typeof(App).Assembly);

            ServiceContainer.Register(NavigationService);
        }
    }
}
