//using Lab10.Green;
//using System.Text.Json;

//namespace Lab10Test.Green
//{
//    [TestClass]
//    public sealed class GreenManagerTest
//    {
//        private Lab9.Green.Green[] _tasks;
//        private string[] _input;
//        private string[] _pattern;

//        [TestInitialize]
//        public void LoadData()
//        {
//            var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
//            var file = Path.Combine(folder, "Lab10Test", "Green", "data.json");

//            var json = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(file));

//            _input = json.GetProperty("Task3").GetProperty("input").Deserialize<string[]>();
//            _pattern = json.GetProperty("Task3").GetProperty("input").Deserialize<string[]>();
//        }

//        private void Init(int i)
//        {
//            _tasks = new Lab9.Green.Green[]
//            {
//                 new Lab9.Green.Task1(_input[i]),
//                 new Lab9.Green.Task2(_input[i]),
//                 new Lab9.Green.Task3(_input[i], _pattern[i]),
//                 new Lab9.Green.Task4(_input[i])
//            };

//            foreach (var t in _tasks)
//                t.Review();
//        }

//        [TestMethod]
//        public void Test_00_OOP()
//        {
//            var type = typeof(Lab10.Green.Green);

//            Assert.IsTrue(type.IsClass, "Green must be class");

//            Assert.IsNotNull(type.GetProperty("Manager"));
//            Assert.IsNotNull(type.GetProperty("Tasks"));

//            Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab9.Green.Green) }));
//            Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab9.Green.Green[]) }));
//            Assert.IsNotNull(type.GetMethod("Remove"));
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

//                var w = new Lab10.Green.Green();

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

//                var manager = new GreenTxtFileManager("txt");
//                var folder = Path.Combine(Path.GetTempPath(), $"GreenTxt_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Green.Green(manager, _tasks);

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

//                var manager = new GreenJsonFileManager("json");
//                var folder = Path.Combine(Path.GetTempPath(), $"GreenJson_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Green.Green(manager, _tasks);

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

//                var txtManager = new GreenTxtFileManager("txt");
//                var jsonManager = new GreenJsonFileManager("json");

//                var folder = Path.Combine(Path.GetTempPath(), $"GreenMix_{i}");
//                Directory.CreateDirectory(folder);

//                txtManager.SelectFolder(folder);

//                var w = new Lab10.Green.Green(txtManager, _tasks);

//                w.SaveTasks();

//                w.ChangeManager(jsonManager);
//                w.LoadTasks();

//                for (int j = 0; j < w.Tasks.Length; j++)
//                {
//                    Assert.IsTrue(w.Tasks[j] == null || w.Tasks[j].Input != _tasks[j].Input,
//                        $"Format switch should break deserialize test {i} task {j}");
//                }

//                var newFolder = Path.Combine(Directory.GetCurrentDirectory(), "json");
//                if (Directory.Exists(newFolder))
//                    Directory.Delete(newFolder, true);

//                Directory.Delete(folder, true);
//            }
//        }

//        [TestMethod]
//        public void Test_05_Clear_All()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var manager = new GreenTxtFileManager("clear");
//                var folder = Path.Combine(Path.GetTempPath(), $"GreenClear_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Green.Green(manager, _tasks);

//                w.Clear();

//                Assert.AreEqual(0, w.Tasks.Length, $"Clear failed test {i}");
//                Assert.IsFalse(Directory.Exists(folder), $"Folder not deleted test {i}");
//            }
//        }
//    }
//}