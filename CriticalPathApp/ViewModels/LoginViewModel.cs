using Android.Text;
using CriticalPathApp.Services;
using CriticalPathApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CriticalPathApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            ActivityDataStore = DependencyService.Resolve<IActivityDataStore>();
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged();}
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged();}
        }

        private string response;

        public string Response
        {
            get { return response; }
            set { response = value; OnPropertyChanged(); }
        }

        public bool ValidateUser()
        {
            Response = "";

            if (Username == ActivityDataStore.Username && Password == ActivityDataStore.Password)
                return true;

            Response = "Invalid Login Details";
            return false;
        }
        public IActivityDataStore ActivityDataStore { get; set; }

    }
}
