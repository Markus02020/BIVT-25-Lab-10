using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Green
{
    public class Green
    {
        public Lab9.Green.Green[] Tasks { get; private set; }
        public GreenFileManager Manager { get; private set; }

        public Green(Lab9.Green.Green[] tasks_ins = null)
        {
            if (tasks_ins == null || tasks_ins.Length == 0)
                Tasks = new Lab9.Green.Green[0];
            else
                Tasks = tasks_ins.ToArray();
        }

        public Green(Lab10.Green.GreenFileManager manager_ins, Lab9.Green.Green[] tasks_ins = null)
        {
            Manager = manager_ins;
            if (tasks_ins == null || tasks_ins.Length == 0)
                Tasks = new Lab9.Green.Green[0];
            else
                Tasks = tasks_ins.ToArray();
        }

        public Green(Lab9.Green.Green[] tasks_ins, Lab10.Green.GreenFileManager manager_ins)
        {
            Manager = manager_ins;
            if (tasks_ins == null || tasks_ins.Length == 0)
                Tasks = new Lab9.Green.Green[0];
            else
                Tasks = tasks_ins.ToArray();
        }

        public void Add(Lab9.Green.Green task)
        {
            if (task == null) return;
            Lab9.Green.Green[] upd_Tasks = new Lab9.Green.Green[Tasks.Length + 1];
            Array.Copy(Tasks, upd_Tasks, Tasks.Length);
            upd_Tasks[Tasks.Length] = task;
            Tasks = upd_Tasks;
        }

        public void Add(Lab9.Green.Green[] tasks)
        {
            if (tasks == null) return;
            for (int i = 0; i < tasks.Length; i++)
                Add(tasks[i]);
        }

        public void Remove(Lab9.Green.Green task)
        {
            if (task == null || Tasks == null) return;
            Tasks = Tasks.Where(t => t != task).ToArray();
        }

        public void Clear()
        {
            Tasks = new Lab9.Green.Green[0];
            if (Manager != null && !string.IsNullOrEmpty(Manager.FolderPath) && System.IO.Directory.Exists(Manager.FolderPath))
            {
                System.IO.Directory.Delete(Manager.FolderPath, true);
            }
        }

        public void SaveTasks()
        {
            if (Manager == null || Tasks == null) return;
            for (int i = 0; i < Tasks.Length; i++)
            {
                Manager.ChangeFileName($"task_{i}");
                Manager.Serialize(Tasks[i]);
            }
        }

        public void LoadTasks()
        {
            if (Manager == null || Tasks == null) return;
            for (int i = 0; i < Tasks.Length; i++)
            {
                Manager.ChangeFileName($"task_{i}");
                Tasks[i] = Manager.Deserialize<Lab9.Green.Green>();
            }
        }

        public void ChangeManager(GreenFileManager newManager)
        {
            if (newManager == null) return;
            Manager = newManager;

            if (!System.IO.Directory.Exists(Manager.Name))
                System.IO.Directory.CreateDirectory(Manager.Name);

            Manager.SelectFolder(Manager.Name);
        }
    }
}