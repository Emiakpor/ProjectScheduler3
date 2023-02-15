using CriticalPathApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CriticalPathApp.Services
{
    public class ActivityDataStore : IActivityDataStore
    {
        public IList<ActivityModel> Activities { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CriticalPath { get; set; }
        public long? TotalDuration { get; set; }

        public void AddCriticalPath(string criticalPath)
        {
            CriticalPath = criticalPath;
        }

        public void AddEmail(string email)
        {
            Email = email;
        }

        public async Task<bool> AddItemAsync(ActivityModel item)
        {
            Activities.Add(item);

            return await Task.FromResult(true);
        }

        public void AddPassword(string password)
        {
            Password = password;
        }

        public void AddSurname(string username)
        {
            Username = username;
        }

        public void AddTotalDuration(long totalDuration)
        {
            TotalDuration = totalDuration;
        }

        public async Task<bool> ClearAllItemAsync()
        {
            Activities.Clear();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = Activities.Where((ActivityModel arg) => arg.Id == id).FirstOrDefault();
            Activities.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ActivityModel> GetItemAsync(string id)
        {
            return await Task.FromResult(Activities.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ActivityModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Activities);
        }

        public async Task<bool> UpdateItemAsync(ActivityModel item)
        {
            var oldItem = Activities.Where((ActivityModel arg) => arg.Id == item.Id).FirstOrDefault();
            Activities.Remove(oldItem);
            Activities.Add(item);

            return await Task.FromResult(true);
        }
    }
}
