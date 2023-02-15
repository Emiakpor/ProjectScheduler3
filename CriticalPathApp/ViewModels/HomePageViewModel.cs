using Android.App;
using CriticalPathApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace CriticalPathApp.ViewModels
{
    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            ContinueCommand = new Command(ContinueCommandAction);
            ExitCommand = new Command(ExitCommandAction);
        }

        private void ContinueCommandAction(object obj)
        {
            Shell.Current.GoToAsync($"//{nameof(RegisterAccountPageView)}");
        }

        private void ExitCommandAction(object obj)
        {
            Thread.CurrentThread.Abort();
        }

        public ICommand ContinueCommand { get; }
        public ICommand ExitCommand { get; }
    }
}
