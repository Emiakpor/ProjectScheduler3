using CriticalPathApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CriticalPathApp.ViewModels
{
    public class ConfirmationPageViewModel : BaseViewModel
    {
        public ConfirmationPageViewModel()
        {
            ActivityDataStore = DependencyService.Resolve<IActivityDataStore>();
            Username = ActivityDataStore.Username;
            Email = ActivityDataStore.Email;

        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        public IActivityDataStore ActivityDataStore { get; set; }

    }
}
