//using Lab10;
//using Lab10.Green;
//using System.Text.Json;

//namespace Lab10Test.Green
//{
//    [TestClass]
//    public sealed class TxtTest
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
//        public void Test_00_OOP_TxtManager()
//        {
//            var type = typeof(GreenTxtFileManager);

//            Assert.IsTrue(type.IsClass, "GreenTxtFileManager must be class");
//            Assert.IsFalse(type.IsAbstract, "GreenTxtFileManager must NOT be abstract");

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
//        public void Test_01_Serialize_Txt()
//        {
//            ISerializer manager = new GreenTxtFileManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "TxtTest1");
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

//                Assert.IsTrue(content.Contains(_input[i]),
//                    $"No Input field in TXT for task {i}");
//            }
//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_02_Deserialize_Txt()
//        {
//            ISerializer manager = new GreenTxtFileManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "TxtTest2");
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

//                Assert.AreEqual(_tasks[i].ToString(), result.ToString(), $"No Output match in TXT for task {i}");
//            }

//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_03_EditFile_Txt()
//        {
//            var manager = new GreenTxtFileManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "TxtEdit");
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