using CriticalPathApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriticalPathApp.Services
{
    public interface IActivityDataStore: IDataStore<ActivityModel>
    {
        IList<ActivityModel> Activities { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        string CriticalPath { get; set; }
        long? TotalDuration { get; set; }
        void AddSurname(string Username);
        void AddPassword(string password);
        void AddEmail(string email);
        void AddCriticalPath(string criticalPath);
        void AddTotalDuration(long totalDuration);

    }
}
