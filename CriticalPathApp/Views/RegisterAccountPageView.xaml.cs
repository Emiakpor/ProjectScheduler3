using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CriticalPathApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterAccountPageView : ContentPage
    {
        public RegisterAccountPageView()
        {
            InitializeComponent();
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfirmationPageView());
        }

        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}