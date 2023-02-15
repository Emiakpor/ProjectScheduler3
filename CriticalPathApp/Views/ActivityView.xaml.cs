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
    public partial class ActivityView : ContentPage
    {
        public ActivityView()
        {
            InitializeComponent();
        }

        private async void btnCriticalPath_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CriticalPathResultView());

        }
    }
}