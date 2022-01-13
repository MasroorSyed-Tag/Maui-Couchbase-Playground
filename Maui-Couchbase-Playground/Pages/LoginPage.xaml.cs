using Maui_Couchbase_Playground.ViewModels;
using MCP.Application.Forms;

namespace Maui_Couchbase_Playground
{
    public partial class LoginPage : BaseContentPage<LoginViewModel>
	{
		public LoginPage()
		{
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();

            //userNameEntry.Focus();
        }
	}
}