//using Lab10.Blue;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Text.Json;

//namespace Lab10Test.Blue
//{
//    [TestClass]
//    public sealed class BlueManagerTest
//    {
//        private Lab9.Blue.Blue[] _tasks;
//        private string[] _input;
//        private string[] _sequence;

//        [TestInitialize]
//        public void LoadData()
//        {
//            var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
//            var file = Path.Combine(folder, "Lab10Test", "Blue", "data.json");

//            var json = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(file));

//            _input = json.GetProperty("Task2").GetProperty("input").Deserialize<string[]>();
//            _sequence = json.GetProperty("Task2").GetProperty("inputSequence").Deserialize<string[]>();
//        }

//        private void Init(int i)
//        {
//            _tasks = new Lab9.Blue.Blue[]
//            {
//                 new Lab9.Blue.Task1(_input[i]),
//                 new Lab9.Blue.Task2(_input[i], _sequence[i]),
//                 new Lab9.Blue.Task3(_input[i]),
//                 new Lab9.Blue.Task4(_input[i])
//            };

//            foreach (var t in _tasks)
//                t.Review();
//        }

//        [TestMethod]
//        public void Test_00_OOP()
//        {
//            var type = typeof(Lab10.Blue.Blue<Lab9.Blue.Blue>);

//            Assert.IsTrue(type.IsClass, "Blue<T> must be class");

//            Assert.IsNotNull(type.GetProperty("Manager"));
//            Assert.IsNotNull(type.GetProperty("Tasks"));

//            Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab9.Blue.Blue) }));
//            Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab9.Blue.Blue[]) }));
//            Assert.IsNotNull(type.GetMethod("Remove", new[] { typeof(Lab9.Blue.Blue) }));
//            Assert.IsNotNull(type.GetMethod("Clear"));
//            Assert.IsNotNull(type.GetMethod("SaveTasks"));
//            Assert.IsNotNull(type.GetMethod("LoadTasks"));
//            Assert.IsNotNull(type.GetMethod("ChangeManager"));
//        }

//        [TestMethod]
//        public void Test_01_Add_Remove_AllTasks()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var w = new Lab10.Blue.Blue<Lab9.Blue.Blue>();

//                w.Add(_tasks);
//                Assert.AreEqual(_tasks.Length, w.Tasks.Length, $"Add failed test {i}");

//                w.Remove(_tasks[0]);
//                Assert.IsFalse(w.Tasks.Contains(_tasks[0]), $"Remove failed test {i}");
//            }
//        }

//        [TestMethod]
//        public void Test_02_Save_Load_Txt()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var manager = new BlueTxtFileManager<Lab9.Blue.Blue>("txt");
//                var folder = Path.Combine(Path.GetTempPath(), $"BlueTxt_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Blue.Blue<Lab9.Blue.Blue>(manager, _tasks);

//                w.SaveTasks();
//                w.LoadTasks();

//                for (int j = 0; j < _tasks.Length; j++)
//                {
//                    Assert.AreEqual(_tasks[j].Input, w.Tasks[j].Input,
//                        $"TXT load mismatch test {i} task {j}");
//                }

//                Directory.Delete(folder, true);
//            }
//        }

//        [TestMethod]
//        public void Test_03_Save_Load_Json()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var manager = new BlueJsonFileManager<Lab9.Blue.Blue>("json");
//                var folder = Path.Combine(Path.GetTempPath(), $"BlueJson_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Blue.Blue<Lab9.Blue.Blue>(manager, _tasks);

//                w.SaveTasks();
//                w.LoadTasks();

//                for (int j = 0; j < _tasks.Length; j++)
//                {
//                    Assert.AreEqual(_tasks[j].Input, w.Tasks[j].Input,
//                        $"JSON load mismatch test {i} task {j}");
//                }

//                Directory.Delete(folder, true);
//            }
//        }

//        [TestMethod]
//        public void Test_04_ChangeManager_And_Format()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var txtManager = new BlueTxtFileManager<Lab9.Blue.Blue>("txt");
//                var jsonManager = new BlueJsonFileManager<Lab9.Blue.Blue>("json");

//                var folder = Path.Combine(Path.GetTempPath(), $"BlueMix_{i}");
//                Directory.CreateDirectory(folder);

//                txtManager.SelectFolder(folder);

//                var w = new Lab10.Blue.Blue<Lab9.Blue.Blue>(txtManager, _tasks);

//                w.SaveTasks();

//                w.ChangeManager(jsonManager);
//                w.LoadTasks();

//                bool allNull = true;
//                bool allSame = true;
//                for (int j = 0; j < w.Tasks.Length; j++)
//                {
//                    if (w.Tasks[j] != null)
//                    {
//                        allNull = false;
//                        if (w.Tasks[j].Input == _tasks[j].Input)
//                            allSame = false;
//                    }
//                }
//                Assert.IsTrue(allNull, "Change manager and format crash deserialization");
//                if (!allNull)
//                    Assert.IsFalse(allSame, "ChangeManager should affect deserialization due to format difference");

//                Directory.Delete(folder, true);
//            }
//        }

//        [TestMethod]
//        public void Test_05_Clear_All()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var manager = new BlueTxtFileManager<Lab9.Blue.Blue>("clear");
//                var folder = Path.Combine(Path.GetTempPath(), $"BlueClear_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Blue.Blue<Lab9.Blue.Blue>(manager, _tasks);

//                w.Clear();

//                Assert.AreEqual(0, w.Tasks.Length, $"Clear failed test {i}");
//                Assert.IsFalse(Directory.Exists(folder), $"Folder not deleted test {i}");
//            }
//        }
//    }
//}