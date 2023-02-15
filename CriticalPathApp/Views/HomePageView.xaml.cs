using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CriticalPathApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageView : ContentPage
    {
        public HomePageView()
        {
            InitializeComponent();
        }

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterAccountPageView());
        }

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("", "Exit the application?", "Yes", "No"))
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }
                    else if (Device.RuntimePlatform == Device.iOS)
                    {
                        Thread.CurrentThread.Abort();
                    }
                }
            });
        }
    }
}