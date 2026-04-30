//using Lab10;
//using Lab10.White;
//using System.Text.Json;

//namespace Lab10Test.White
//{
//    [TestClass]
//    public sealed class TxtTest
//    {
//        private Lab9.White.White[] _tasks;
//        private string[] _input;
//        private string[,] _codes;

//        [TestInitialize]
//        public void LoadData()
//        {
//            var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

//            var file = Path.Combine(folder, "Lab10Test", "White", "data.json");

//            var json = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(file));

//            _input = json.GetProperty("Task3").GetProperty("input").Deserialize<string[]>();

//            var codesArray = json.GetProperty("Task3").GetProperty("codes").Deserialize<string[][]>();
//            _codes = new string[codesArray.Length, 2];

//            for (int i = 0; i < codesArray.Length; i++)
//            {
//                _codes[i, 0] = codesArray[i][0];
//                _codes[i, 1] = codesArray[i][1];
//            }
//        }

//        [TestMethod]
//        public void Test_00_OOP_TxtManager()
//        {
//            var type = typeof(WhiteTxtFileManager);

//            Assert.IsTrue(type.IsClass, "WhiteTxtFileManager must be class");
//            Assert.IsFalse(type.IsAbstract, "WhiteTxtFileManager must NOT be abstract");

//            Assert.IsTrue(typeof(WhiteFileManager).IsAssignableFrom(type),
//                "Must inherit from WhiteFileManager");

//            Assert.IsTrue(typeof(IWhiteSerializer).IsAssignableFrom(type),
//                "Must implement IWhiteSerializer");

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
//            IWhiteSerializer manager = new WhiteTxtFileManager("test");
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
//            IWhiteSerializer manager = new WhiteTxtFileManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "TxtTest2");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);

//            for (int i = 0; i < 4; i++)
//            {
//                Init(i, true);

//                fileManager.ChangeFileName($"task{i}");

//                manager.Serialize(_tasks[i]);
//                var result = manager.Deserialize();

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
//            var manager = new WhiteTxtFileManager("test");
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

//                var result = manager.Deserialize();

//                Assert.AreEqual("NEW_TEXT", result.Input,
//                    "EditFile failed");
//            }
//            Directory.Delete(folder, true);
//        }

//        private void Init(int i, bool review = false)
//        {
//            _tasks = new Lab9.White.White[]
//            {
//                 new Lab9.White.Task1(_input[i]),
//                 new Lab9.White.Task2(_input[i]),
//                 new Lab9.White.Task3(_input[i], _codes),
//                 new Lab9.White.Task4(_input[i])
//            };

//            if (review)
//                foreach (var t in _tasks)
//                    t.Review();
//        }
//    }
//}