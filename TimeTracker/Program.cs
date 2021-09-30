using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] processlist = Process.GetProcesses();
            List<TimeTrackerProcess> timeTrackerProcesses=new List<TimeTrackerProcess>();
            foreach (Process proc in processlist)
            {
                String title = proc.MainWindowTitle;
                if (title.Length == 0) continue;
                int id = proc.Id;
                var i = new TimeTrackerProcess(proc.Id, title, proc.MainModule.FileName,proc.ProcessName);
                timeTrackerProcesses.Add(i);
                Console.Out.Write(id);
                Console.Out.Write(" <==> ");
                Console.Out.Write(title);
                Console.Out.Write("\n");

            }
            var db = new DbManager();
            db.addProcessesToDb(timeTrackerProcesses);
            Console.In.ReadLine();
        }
    }
}
