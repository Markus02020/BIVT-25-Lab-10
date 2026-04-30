//using Lab10;
//using Lab10.Green;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json.Linq;
//using System.Text.Json;

//namespace Lab10Test.Green
//{
//    [TestClass]
//    public sealed class JsonTest
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
//            _pattern = json.GetProperty("Task3").GetProperty("pattern").Deserialize<string[]>();
//        }

//        [TestMethod]
//        public void Test_00_OOP_JsonManager()
//        {
//            var type = typeof(GreenJsonFileManager);

//            Assert.IsTrue(type.IsClass, "GreenJsonFileManager must be class");
//            Assert.IsFalse(type.IsAbstract, "GreenJsonFileManager must NOT be abstract");

//            Assert.IsTrue(typeof(GreenFileManager).IsAssignableFrom(type),
//                "Must inherit from GreenFileManager");

//            Assert.IsTrue(typeof(ISerializer).IsAssignableFrom(type),
//                "Must implement ISerializer");

//            Assert.IsNotNull(type.GetConstructor(new[] { typeof(string) }),
//                "Constructor (string) missing");

//            Assert.IsNotNull(type.GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
//                "Constructor (string, string, string, string) missing");

//            Assert.IsNotNull(type.GetMethod("Serialize"));
//            Assert.IsNotNull(type.GetMethod("Deserialize"));
//            Assert.IsNotNull(type.GetMethod("EditFile"));
//            Assert.IsNotNull(type.GetMethod("ChangeFileExtension"));
//        }

//        [TestMethod]
//        public void Test_01_Serialize_JSON()
//        {
//            ISerializer manager = new GreenJsonFileManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "JsonTest1");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);

//            for (int i = 0; i < 4; i++)
//            {
//                Init(i, true);

//                fileManager.ChangeFileName($"task{i}");
//                manager.Serialize(_tasks[i]);

//                Assert.IsTrue(File.Exists(fileManager.FullPath),
//                    $"File not created for task {i}");

//                var content = File.ReadAllText(fileManager.FullPath);
//                Assert.IsTrue(!string.IsNullOrEmpty(content),
//                    $"Empty file for task {i}");

//                var json = JObject.Parse(content);
//                var input = json["Input"].ToString();

//                Assert.AreEqual(_input[i], input,
//                    $"Input mismatch for task {i}");
//            }
//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_02_Deserialize_JSON()
//        {
//            ISerializer manager = new GreenJsonFileManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "JsonTest2");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);

//            for (int i = 0; i < 4; i++)
//            {
//                Init(i, true);

//                fileManager.ChangeFileName($"task{i}");

//                manager.Serialize(_tasks[i]);
//                var result = manager.Deserialize<Lab9.Green.Green>();

//                Assert.IsNotNull(result, $"Null for task {i}");

//                Assert.AreEqual(_tasks[i].Input, result.Input,
//                    $"Input mismatch for task {i}");

//                Assert.AreEqual(_tasks[i].ToString(), result.ToString(), $"No Output match in JSON for task {i}");
//            }
//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_03_EditFile()
//        {
//            var manager = new GreenJsonFileManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "JsonEdit");
//            Directory.CreateDirectory(folder);

//            for (int i = 0; i < 4; i++)
//            {
//                Init(i, true);

//                fileManager.SelectFolder(folder);
//                fileManager.ChangeFileName("task");

//                manager.Serialize(_tasks[i]);

//                manager.EditFile("NEW_TEXT");

//                var result = manager.Deserialize<Lab9.Green.Green>();

//                Assert.AreEqual("NEW_TEXT", result.Input,
//                    "EditFile failed");
//            }
//            Directory.Delete(folder, true);
//        }

//        private void Init(int i, bool review = false)
//        {
//            _tasks = new Lab9.Green.Green[]
//            {
//                 new Lab9.Green.Task1(_input[i]),
//                 new Lab9.Green.Task2(_input[i]),
//                 new Lab9.Green.Task3(_input[i], _pattern[i]),
//                 new Lab9.Green.Task4(_input[i])
//            };

//            if (review)
//                foreach (var t in _tasks)
//                    t.Review();
//        }
//    }
//}