using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    class TimeTrackerProcess
    {
        int id;
        int pId;
        string title;
        string exePath;
        string processName;
        int start_time;
        int last_update_time;
        int total_active_time; 

        public TimeTrackerProcess(int id, string title, string exePath, string processName)
        {
            this.pId = id;
            this.title = title;
            this.exePath = exePath;
            this.processName= processName;
        }

        public TimeTrackerProcess(int pId, string title, string exePath, string processName, int start_time, int last_update_time, int total_active_time)
        {
            this.pId = pId;
            this.title = title;
            this.exePath = exePath;
            this.processName = processName;
            this.start_time = start_time;
            this.last_update_time = last_update_time;
            this.total_active_time = total_active_time;
        }

        public TimeTrackerProcess(int id, int pId, string title, string exePath, string processName, int start_time, int last_update_time, int total_active_time)
        {
            this.id= id;
            this.pId = pId;
            this.title = title;
            this.exePath = exePath;
            this.processName = processName;
            this.start_time = start_time;
            this.last_update_time = last_update_time;
            this.total_active_time = total_active_time;
        }

        public int Total_active_time { get => total_active_time; set => total_active_time = value; }
        public int Id { get => id; set => id = value; }
        public int PId { get => pId; set => pId = value; }
        public string Title { get => title; set => title = value; }
        public string ExePath { get => exePath; set => exePath = value; }
        public string ProcessName { get => processName; set => processName = value; }
        public int Start_time { get => start_time; set => start_time = value; }
        public int Last_update_time { get => last_update_time; set => last_update_time = value; }
    }
}
