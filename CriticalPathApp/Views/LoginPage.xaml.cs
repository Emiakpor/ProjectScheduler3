using CriticalPathApp.ViewModels;
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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            this.BindingContext = loginViewModel;
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if(loginViewModel.ValidateUser())
                await Navigation.PushAsync(new ActivityView());
        }
        private LoginViewModel loginViewModel;
    }
}