using CriticalPathApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CriticalPathApp.ViewModels
{
    public class RegisterAccountPageViewModel: BaseViewModel
    {
		public RegisterAccountPageViewModel()
		{
            ActivityDataStore = DependencyService.Resolve<IActivityDataStore>();

        }

		private string username;

		public string Username
		{
			get { return username; }
			set { username = value; OnPropertyChanged(); ActivityDataStore.AddSurname(value); SetIsRegisterCommandEnabled(); }
		}

		private string password;

		public string Password
		{
			get { return password; }
			set { password = value; OnPropertyChanged(); ActivityDataStore.AddPassword(value); SetIsRegisterCommandEnabled(); }
        }

		private string email;

		public string Email
		{
			get { return email; }
			set { email = value; OnPropertyChanged(); ActivityDataStore.AddEmail(value); SetIsRegisterCommandEnabled(); }
        }

		private bool isRegisterCommandEnabled;

		public bool IsRegisterCommandEnabled
        {
			get { return isRegisterCommandEnabled; }
			set { isRegisterCommandEnabled = value; OnPropertyChanged(); }
        }

		private void SetIsRegisterCommandEnabled()
		{
            IsRegisterCommandEnabled = false;

            if (Username != String.Empty && Password != String.Empty && Email != String.Empty)
				IsRegisterCommandEnabled = true;

        }



        public IActivityDataStore ActivityDataStore { get; set; }

	}
}
