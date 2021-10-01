using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    class TimeTracker
    {
        private DbManager db;

        public TimeTracker()
        {
            db = new DbManager(); 
        }
        class LogWriter
        {
            public LogWriter() { }
            private static string m_exePath = string.Empty;
            public static void LogWrite(string logMessage)
            {
                m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!File.Exists(m_exePath + "\\" + "log.txt"))
                    File.Create(m_exePath + "\\" + "log.txt");

                try
                {
                    using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                        AppendLog(logMessage, w);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            private static void AppendLog(string logMessage, TextWriter txtWriter)
            {
                try
                {
                    txtWriter.Write("\r\nLog Entry : ");
                    txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                    txtWriter.WriteLine("  :");
                    txtWriter.WriteLine("  :{0}", logMessage);
                    txtWriter.WriteLine("-------------------------------");
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.Message);
                }
            }
        }

        public void updateDbWithProcesses()
        {
            Process[] processlist = Process.GetProcesses();
            List<TimeTrackerProcess> timeTrackerProcesses = new List<TimeTrackerProcess>();
            foreach (Process proc in processlist)
            {

                string title = proc.MainWindowTitle;
                if (title.Length == 0) continue;
                string exePath = "";
                string processName = "";
                try
                {
                    processName = proc.ProcessName;
                    exePath = proc.MainModule.FileName;
                    
                }
                catch(Exception ex)
                {
                    LogWriter.LogWrite("Faild to get data for:" + title);
                }
                int id = proc.Id;
                var i = new TimeTrackerProcess(proc.Id, title, exePath, processName);
                timeTrackerProcesses.Add(i);
                LogWriter.LogWrite(id + " <==> " + title);
                /*Console.Out.Write(id);
                Console.Out.Write(" <==> ");
                Console.Out.Write(title);
                Console.Out.Write("\n");*/

            }
            db.addProcessesToDb(timeTrackerProcesses);
        }
    }
}
