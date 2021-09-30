using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace TimeTracker
{
    class DbManager
    {
        private SQLiteConnection _conn;

        public DbManager()
        {
            initConnection();

        }

        private void initConnection()
        {
            string cs = "Data Source=data.db";
            SQLiteConnection conn = new SQLiteConnection(cs);
            conn.Open();
            this._conn = conn;

            var cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS 'processes' (
	            'id'	INTEGER,
	            'p_id'	INTEGER,
	            'title'	TEXT,
	            'exe_path'	TEXT,
	            'process_name'	TEXT,
	            'start_time'	INTEGER,
	            'last_update'	INTEGER,
	            'total_active_time'	INTEGER NOT NULL DEFAULT 0,
	            PRIMARY KEY('id')
            );";
            int success = cmd.ExecuteNonQuery();

            Console.WriteLine($"SQLite init: {success}");
        }


        private List<int> getProcessesIds(List<TimeTrackerProcess> processes)
        {
            List<int> ids = new List<int>();
            foreach (TimeTrackerProcess p in processes)
            {
                ids.Add(p.PId);
            }
            return ids;
        }
        private void insertProcessToDb(List<TimeTrackerProcess> processes, List<int> excludeId)
        {
            var transaction = _conn.BeginTransaction();

            var get_existing_command = _conn.CreateCommand();
            get_existing_command.CommandText = @"
                    INSERT INTO processes 
                        ('p_id','title','exe_path','process_name','start_time','last_update') 
                    VALUES 
                        (@p_id, @title, @exe_path, @process_name, @start_time, @last_update);
                ";


            foreach (TimeTrackerProcess process in processes)
            {
                long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (excludeId.Contains(process.PId))
                {
                    Console.WriteLine($"alerady exist, {process.PId} skip!");
                    var update_command = _conn.CreateCommand();
                    update_command.CommandText = @"
                    UPDATE processes SET 'last_update'= @last_update WHERE p_id = @id;
                ";
                    update_command.Parameters.AddWithValue("@id", process.PId);
                    update_command.Parameters.AddWithValue("@last_update", milliseconds);
                    update_command.ExecuteNonQuery();
                    continue;
                }
                else
                {
                    Console.WriteLine($"add, {process.PId} ");

                }
                get_existing_command.Parameters.AddWithValue("@p_id", process.PId);
                get_existing_command.Parameters.AddWithValue("@title", process.Title);
                get_existing_command.Parameters.AddWithValue("@exe_path", process.ExePath);
                get_existing_command.Parameters.AddWithValue("@process_name", process.ProcessName);
                get_existing_command.Parameters.AddWithValue("@start_time", milliseconds);
                get_existing_command.Parameters.AddWithValue("@last_update", milliseconds);
                get_existing_command.ExecuteNonQuery();
            }

            transaction.Commit();

        }


        public void addProcessesToDb(List<TimeTrackerProcess> processes)
        {
            var ids = getProcessesIds(processes);
            if (ids.Count > 1)
            {
                var get_existing_command = _conn.CreateCommand();
                string p1 = string.Join(",", ids);
                get_existing_command.CommandText = @"SELECT * FROM processes WHERE p_id in (" + p1 + ") group by p_id;";
                var reader = get_existing_command.ExecuteReader();
                List<int> excludeId = new List<int>();
                while (reader.Read())
                {
                    var id = reader.GetInt32(1);
                    excludeId.Add(id);

                    Console.WriteLine($"Hello, {id}!");
                }

                insertProcessToDb(processes, excludeId);

            }
        }
    }
}
