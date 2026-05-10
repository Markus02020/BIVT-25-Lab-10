using Lab9.Green;
using System;
using System.IO;

namespace Lab10.Green
{
    public class Green
    {
        private GreenFileManager _manager;
        private Green[] _tasks;

        public GreenFileManager Manager => _manager;
        public Green[] Tasks => (Green[])_tasks.Clone();

        public Green(Green[] tasks = null)
        {
            _tasks = tasks != null ? (Green[])tasks.Clone() : new Green[0];
        }

        public Green(GreenFileManager manager, Green[] tasks = null) : this(tasks)
        {
            _manager = manager;
        }

        public Green(Green[] tasks, GreenFileManager manager) : this(tasks)
        {
            _manager = manager;
        }

        public void Add(Green task)
        {
            if (task == null) return;
            Array.Resize(ref _tasks, _tasks.Length + 1);
            _tasks[_tasks.Length - 1] = task;
        }

        public void Add(Green[] tasks)
        {
            if (tasks == null) return;
            foreach (var t in tasks) Add(t);
        }

        public void Remove(Green task)
        {
            if (task == null || _tasks.Length == 0) return;
            var list = new System.Collections.Generic.List<Green>(_tasks);
            list.Remove(task);
            _tasks = list.ToArray();
        }

        public void Clear()
        {
            _tasks = new Green[0];
            if (_manager != null && Directory.Exists(_manager.FolderPath))
                Directory.Delete(_manager.FolderPath, true);
        }

        public void SaveTasks()
        {
            if (_manager == null || _tasks.Length == 0) return;
            for (int i = 0; i < _tasks.Length; i++)
            {
                _manager.ChangeFileName($"Task{i + 1}");
                _manager.Serialize(_tasks[i]);
            }
        }

        public void LoadTasks()
        {
            if (_manager == null || _tasks.Length == 0) return;
            for (int i = 0; i < _tasks.Length; i++)
            {
                _manager.ChangeFileName($"Task{i + 1}");
                _tasks[i] = _manager.Deserialize<Green>();
            }
        }

        public void ChangeManager(GreenFileManager manager)
        {
            _manager = manager;
            if (!Directory.Exists(manager.FolderPath))
                Directory.CreateDirectory(manager.FolderPath);
            manager.SelectFolder(manager.FolderPath);
        }
    }
}
