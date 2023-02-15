using CriticalPathApp.Models;
using CriticalPathApp.Services;
using CriticalPathApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using static Android.Content.ClipData;

namespace CriticalPathApp.ViewModels
{
    public class ActivityViewModel : BaseViewModel
    {
        public ActivityViewModel()
        {
            ActivityDataStore = DependencyService.Resolve<IActivityDataStore>();

            RefreshCommand = new Command(CmdRefresh);
            CriticalPathCommand = new Command(FindCriticalPath);
            LoadItemsCommand = new Command(LoadActiviies);
            AddCommand = new Command(AddActivity); 
            RemoveCommand = new Command(RemoveActivity);
            ComputeActivityTimeCommand = new Command(ComputeStartAndCompletionTimeHandler);
            SampleDataCommand = new Command(SampleDataCommandHandler); 
            ClearCommand = new Command(ClearActivities); 
            SetActivities();
            Mode = "Add";
            IsCriticalEnabled = false;
            IsAddEnabled = false;

        }

        private void SampleDataCommandHandler()
        {
            SetActivities();

        }

        private List<string> GetActivityIds()
        {
            var activityIds = ActivityDataStore.Activities.Select(a => a.Activity).ToList();
            return activityIds;
        }
        private void ComputeStartAndCompletionTimeHandler()
        {
            LoadStoreActivities();
            StartCommandHandler();
            FinishCommandHandler();
            Slack();
            FindCriticalPath();
        }
        private void FindCriticalPath()
        {
            var _activities = CriticalPathCalculationService.GetActivities(ActivityDataStore.Activities.ToList());
            CriticalPathCalculationService criticalPathCalculationService = new CriticalPathCalculationService(_activities);
            var criticalPath = criticalPathCalculationService.GetCriticalPaths();
            var totalDuration = criticalPathCalculationService.GetTotalDuration();
            ActivityDataStore.AddCriticalPath(criticalPath);
            ActivityDataStore.AddTotalDuration(totalDuration);
            if(ActivityDataStore.CriticalPath != string.Empty)
                IsCriticalEnabled = true;

        }
        private void StartCommandHandler()
        {
           var activityIds = GetActivityIds();
            activityIds.Sort();
            for(int i=0; i<activityIds.Count; i++)
            {
                var _activity = ActivityDataStore.Activities.FirstOrDefault(a => a.Id == activityIds[i]);

                if (i == 0)
                {
                    _activity.EarliestStart = 0;
                    _activity.EarliestFinish = _activity.EarliestStart + _activity.Duration;
                }
                else
                {
                    var predecessors = _activity.Predecessor.Split(',');
                    int np = predecessors.Length;
                    var earliestStart = 0;
                    foreach(var item in predecessors)
                    {
                        var _activityIn = ActivityDataStore.Activities.FirstOrDefault(a => a.Id == item.Trim());
                        if (earliestStart < _activityIn.EarliestFinish) earliestStart = int.Parse(_activityIn.EarliestFinish.ToString());

                    }
                    _activity.EarliestStart = earliestStart;
                    _activity.EarliestFinish = _activity.EarliestStart + _activity.Duration;

                }
            }
        }

        private void FinishCommandHandler()
        {
            SetSuccessor();
            var activityIds = GetActivityIds();
            activityIds.Sort();

            string firstActivities = activityIds[0];
            List<string> firstActivitiesSuccessor = new List<string>();
            for (int i = activityIds.Count-1; i >=0 ; i--)
            {
                var _activity = ActivityDataStore.Activities.FirstOrDefault(a => a.Id == activityIds[i]);

                if (i == activityIds.Count - 1)
                {
                    _activity.LatestFinish = _activity.EarliestFinish;
                    _activity.LatestStart = _activity.LatestFinish - _activity.Duration;
                }
                else
                {
                    var predecessors = _activity.Successor.Split(',');
                    int np = predecessors.Length;
                    int latestFinish = 0;
                    if(np>0)
                    {
                        latestFinish = ActivityDataStore.Activities
                            .FirstOrDefault(a => a.Id ==predecessors.FirstOrDefault(x => x != string.Empty).Trim()).LatestStart.Value;
                    }

                    foreach (var item in predecessors)
                    {
                        var _activityIn = ActivityDataStore.Activities.FirstOrDefault(a => a.Id == item);
                        if (latestFinish > _activityIn.LatestStart) latestFinish = int.Parse(_activityIn.LatestStart.ToString());

                    }
                    _activity.LatestFinish = latestFinish;
                    _activity.LatestStart = _activity.LatestFinish - _activity.Duration;

                }
            }
        }

        private void Slack()
        {
            foreach(var _activity in ActivityDataStore.Activities)
            {
                _activity.StartSlack = int.Parse(Math.Abs(decimal.Parse((_activity.EarliestStart.Value - _activity.LatestStart.Value).ToString())).ToString());
                _activity.FinishSlack = int.Parse(Math.Abs(decimal.Parse((_activity.EarliestFinish.Value - _activity.LatestFinish.Value).ToString())).ToString());
            }
        }

        private void SetSuccessor()
        {
            var activityIds = GetActivityIds();
            activityIds.Sort();
            var lastActivity = activityIds[activityIds.Count-1];
            foreach (var _activity in ActivityDataStore.Activities)
            {
                if(_activity.Id != lastActivity)
                {
                    foreach (var _activityIns in ActivityDataStore.Activities)
                    {
                        if (_activity.Activity != _activityIns.Activity)
                        {
                            var predecessors = _activityIns.Predecessor.Split(',');
                            int np = predecessors.Length;
                            if (predecessors.Length == 1 && predecessors[0] == string.Empty)
                                np = 0;

                            if (np > 0 && predecessors.Contains(_activity.Activity))
                            {
                                if (_activity.Successor == null || _activity.Successor == string.Empty)
                                {
                                    _activity.Successor = _activityIns.Activity;
                                }
                                else
                                {
                                    _activity.Successor += "," + _activityIns.Activity;
                                }
                            }
                        }
                    }
                }
                
            }
        }

        private void AddActivity()
        {
            if (Mode == "Add")
            {
               AddNewActivity();
            }
            else if (Mode == "Edit")
            {
                EditActivity();
            }
        }

        private void AddNewActivity()
        {
            ActivityModel newActivity = new ActivityModel
            {
                Sno = Activities.Count + 1,
                Id = Activity,
                Activity = Activity,
                Predecessor = Predecessor,
                Duration = Duration
            };

            Activities.Add(newActivity);
            ClearActivity();
        }

        private void EditActivity()
        {
            var _activity = Activities.FirstOrDefault(a => a.Id == SelectedActivity.Id);
            _activity.Activity = Activity;
            _activity.Predecessor = Predecessor;
            _activity.Duration = Duration;
            ClearActivity();

        }

        private void ClearActivity()
        {
            Activity = string.Empty;
            Predecessor = string.Empty;
            Duration = null;
        }

        private void SetActivity()
        {
            Activity = SelectedActivity.Activity;
            Predecessor = SelectedActivity.Predecessor;
            Duration = SelectedActivity.Duration;
            Mode = "Edit";
        }
        private void RemoveActivity()
        {
            Activities.RemoveAt(Activities.Count- 1);
        }

        private async void CmdRefresh(object obj)
        {
            IsRefreshing= true;
            await Task.Delay(2000);
            IsRefreshing= false;
        }
        private void SetActivities()
        {
           Activities = new ObservableCollection<ActivityModel>
            {
                new ActivityModel("A", 1, "A", "", 3, null, null),
                new ActivityModel("B", 2, "B", "A", 3, null, null),
                new ActivityModel("C", 3, "C", "A", 2, null, null),
                new ActivityModel("D", 4, "D", "B", 8,  null, null),
                new ActivityModel("E", 5, "E", "B", 8, null, null),
                new ActivityModel("F", 6, "F", "C,D", 6,  null, null),
                new ActivityModel("G", 7, "G", "E", 5,  null, null),
                new ActivityModel("H", 8, "H", "G", 24,  null, null),
                new ActivityModel("I", 9, "I", "H", 6, null, null),
                new ActivityModel("J", 10,"J","I,F", 3, null, null),
                new ActivityModel("K", 11,"K","J", 3, null, null),
                new ActivityModel("L", 12,"L","K", 2,  null, null),
                new ActivityModel("M", 13,"M", "L", 1,  null, null)
            };
            
        }
        private void ClearActivities()
        {
            Activities.Clear();
            Activities = new ObservableCollection<ActivityModel>();
        }

        private async void LoadActiviies()
        {
            var _activities = await DataStore.GetItemsAsync(true);
            foreach (var item in _activities)
            {
                Activities.Add(item);
            }
        }

        private void LoadStoreActivities()
        {
            ActivityDataStore.Activities = new List<ActivityModel>();
            foreach (ActivityModel activityModel in Activities)
            {
                ActivityDataStore.AddItemAsync(activityModel);
            }
        }

        private void SetIsAddEnabled()
        {
            IsAddEnabled = false;
            if(Activity != string.Empty && Duration.HasValue && Duration.ToString() != string.Empty)
                IsAddEnabled = true;

        }

        private ObservableCollection<ActivityModel> activities;

        public ObservableCollection<ActivityModel> Activities
        {
            get { return activities; }
            set { activities = value; OnPropertyChanged(); }
        }

        private ActivityModel selectedActivity;

        public ActivityModel SelectedActivity
        {
            get { return selectedActivity; }
            set { selectedActivity = value; OnPropertyChanged(); SetActivity(); }
        }

        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        private bool isCriticalEnabled;

        public bool IsCriticalEnabled
        {
            get { return isCriticalEnabled; }
            set { isCriticalEnabled = value; OnPropertyChanged(); }
        }

        private string activity;

        public string Activity
        {
            get { return activity; }
            set { activity = value; OnPropertyChanged(); SetIsAddEnabled(); }
        }

        private string predecessor;

        public string Predecessor
        {
            get { return predecessor; }
            set { predecessor = value; OnPropertyChanged(); }
        }

        private int? duration;

        public int? Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(); SetIsAddEnabled(); }
        }

        private string mode;

        public string Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged(); }
        }

        private bool isAddEnable;

        public bool IsAddEnabled
        {
            get { return isAddEnable; }
            set { isAddEnable = value; OnPropertyChanged(); }
        }


        public ICommand RefreshCommand { get; set; }
        public ICommand CriticalPathCommand { get; set; }
        public ICommand AddCommand { get; set; }
        
        public ICommand RemoveCommand { get; set; }
        public ActivityModel ActivityModel { get; set; }
        public Command LoadItemsCommand { get; }
        public Command ClearCommand { get; }
        public Command ComputeActivityTimeCommand { get; }
        public Command SampleDataCommand { get; }

        public IActivityDataStore ActivityDataStore { get; set; }


    }
}
