using CriticalPathApp.Services;
using CriticalPathApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CriticalPathApp
{
    public partial class App : Application
    {

        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();

            DependencyService.RegisterSingleton<IActivityDataStore>(new ActivityDataStore());
            MainPage = new NavigationPage(new HomePageView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
