using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Maui_Couchbase_Playground
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}