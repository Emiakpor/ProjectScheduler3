using CriticalPathApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CriticalPathApp.ViewModels
{
   public class CriticalPathResultViewModel : BaseViewModel
    {
        public CriticalPathResultViewModel()
        {
            ActivityDataStore = DependencyService.Resolve<IActivityDataStore>();
            CriticalPath = ActivityDataStore.CriticalPath;
            TotalDuration = ActivityDataStore.TotalDuration;
        }
        private string criticalPath;

        public string CriticalPath
        {
            get { return criticalPath; }
            set { criticalPath = value; OnPropertyChanged(); }
        }

        private long? totalDuration;

        public long? TotalDuration
        {
            get { return totalDuration; }
            set { totalDuration = value; OnPropertyChanged(); }
        }
        public IActivityDataStore ActivityDataStore { get; set; }

    }
}
