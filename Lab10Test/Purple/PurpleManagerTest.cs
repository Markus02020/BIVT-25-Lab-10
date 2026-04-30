//using Lab10.Purple;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Text.Json;

//namespace Lab10Test.Purple
//{
//    [TestClass]
//    public sealed class PurpleManagerTest
//    {
//        private Lab9.Purple.Purple[] _tasks;
//        private string[] _input;
//        private (string pair, char code)[][] _codes;

//        [TestInitialize]
//        public void LoadData()
//        {
//            var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
//            var file = Path.Combine(folder, "Lab10Test", "Purple", "data.json");

//            var json = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(file));

//            _input = json.GetProperty("Task4").GetProperty("input").Deserialize<string[]>();

//            var codesJson = json.GetProperty("Task4").GetProperty("codes");
//            var codesArray = codesJson.Deserialize<CodePair[][]>();

//            _codes = new (string pair, char code)[codesArray.Length][];

//            for (int i = 0; i < codesArray.Length; i++)
//            {
//                _codes[i] = new (string, char)[codesArray[i].Length];
//                for (int j = 0; j < codesArray[i].Length; j++)
//                {
//                    _codes[i][j] = (codesArray[i][j].pair, codesArray[i][j].code[0]);
//                }
//            }
//        }

//        private void Init(int i)
//        {
//            var taskCodes = _codes[i];

//            _tasks = new Lab9.Purple.Purple[]
//            {
//                 new Lab9.Purple.Task1(_input[i]),
//                 new Lab9.Purple.Task2(_input[i]),
//                 new Lab9.Purple.Task3(_input[i]),
//                 new Lab9.Purple.Task4(_input[i], taskCodes)
//            };

//            foreach (var t in _tasks)
//                t.Review();
//        }

//        [TestMethod]
//        public void Test_00_OOP()
//        {
//            var type = typeof(Lab10.Purple.Purple<Lab9.Purple.Purple>);

//            Assert.IsTrue(type.IsClass, "Purple<T> must be class");

//            Assert.IsNotNull(type.GetProperty("Manager"));
//            Assert.IsNotNull(type.GetProperty("Tasks"));

//            Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab9.Purple.Purple) }));
//            Assert.IsNotNull(type.GetMethod("Add", new[] { typeof(Lab9.Purple.Purple[]) }));
//            Assert.IsNotNull(type.GetMethod("Remove", new[] { typeof(Lab9.Purple.Purple) }));
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

//                var w = new Lab10.Purple.Purple<Lab9.Purple.Purple>();

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

//                var manager = new PurpleTxtFileManager<Lab9.Purple.Purple>("txt");
//                var folder = Path.Combine(Path.GetTempPath(), $"PurpleTxt_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Purple.Purple<Lab9.Purple.Purple>(manager, _tasks);

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

//                var manager = new PurpleJsonFileManager<Lab9.Purple.Purple>("json");
//                var folder = Path.Combine(Path.GetTempPath(), $"PurpleJson_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Purple.Purple<Lab9.Purple.Purple>(manager, _tasks);

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
//        public void Test_04_Save_Load_Xml()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var manager = new PurpleXmlFileManager<Lab9.Purple.Purple>("xml");
//                var folder = Path.Combine(Path.GetTempPath(), $"PurpleXml_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Purple.Purple<Lab9.Purple.Purple>(manager, _tasks);

//                w.SaveTasks();
//                w.LoadTasks();

//                for (int j = 0; j < _tasks.Length; j++)
//                {
//                    Assert.AreEqual(_tasks[j].Input, w.Tasks[j].Input,
//                        $"XML load mismatch test {i} task {j}");
//                }

//                Directory.Delete(folder, true);
//            }
//        }

//        [TestMethod]
//        public void Test_05_ChangeManager_And_Format()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var txtManager = new PurpleTxtFileManager<Lab9.Purple.Purple>("txt");
//                var jsonManager = new PurpleJsonFileManager<Lab9.Purple.Purple>("json");
//                var xmlManager = new PurpleXmlFileManager<Lab9.Purple.Purple>("xml");

//                var folder = Path.Combine(Path.GetTempPath(), $"PurpleMix_{i}");
//                Directory.CreateDirectory(folder);

//                txtManager.SelectFolder(folder);

//                var w = new Lab10.Purple.Purple<Lab9.Purple.Purple>(txtManager, _tasks);

//                w.SaveTasks();

//                // Меняем с TXT на JSON
//                w.ChangeManager(jsonManager);
//                w.LoadTasks();

//                int loadedCount = w.Tasks.Count(t => t != null);
//                Assert.IsTrue(loadedCount < _tasks.Length,
//                    $"ChangeManager from TXT to JSON should break loading in test {i}");

//                // Меняем с JSON на XML
//                w.ChangeManager(xmlManager);
//                w.LoadTasks();

//                loadedCount = w.Tasks.Count(t => t != null);
//                Assert.IsTrue(loadedCount < _tasks.Length,
//                    $"ChangeManager from JSON to XML should break loading in test {i}");

//                Directory.Delete(folder, true);
//            }
//        }

//        [TestMethod]
//        public void Test_06_Clear_All()
//        {
//            for (int i = 0; i < _input.Length; i++)
//            {
//                Init(i);

//                var manager = new PurpleTxtFileManager<Lab9.Purple.Purple>("clear");
//                var folder = Path.Combine(Path.GetTempPath(), $"PurpleClear_{i}");
//                Directory.CreateDirectory(folder);

//                manager.SelectFolder(folder);

//                var w = new Lab10.Purple.Purple<Lab9.Purple.Purple>(manager, _tasks);

//                w.Clear();

//                Assert.AreEqual(0, w.Tasks.Length, $"Clear failed test {i}");
//                Assert.IsFalse(Directory.Exists(folder), $"Folder not deleted test {i}");
//            }
//        }

//        private class CodePair
//        {
//            public string pair { get; set; } = "";
//            public string code { get; set; } = "";
//        }
//    }
//}