//using Lab10;
//using Lab10.Blue;
//using Newtonsoft.Json.Linq;
//using System.Text.Json;

//namespace Lab10Test.Blue
//{
//    [TestClass]
//    public sealed class TxtTest
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

//        [TestMethod]
//        public void Test_00_OOP_TxtManager()
//        {
//            var type = typeof(BlueTxtFileManager<Lab9.Blue.Blue>);

//            Assert.IsTrue(type.IsClass, "BlueTxtFileManager<T> must be class");
//            Assert.IsFalse(type.IsAbstract, "BlueTxtFileManager<T> must NOT be abstract");

//            Assert.IsTrue(typeof(BlueFileManager<Lab9.Blue.Blue>).IsAssignableFrom(type),
//                "Must inherit from BlueFileManager<T>");

//            Assert.IsTrue(typeof(ISerializer<Lab9.Blue.Blue>).IsAssignableFrom(type),
//                "Must implement ISerializer<T>");

//            Assert.IsNotNull(type.GetConstructor(new[] { typeof(string) }),
//                "Constructor (string) missing");

//            Assert.IsNotNull(type.GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
//                "Constructor (string, string, string, string) missing");

//            Assert.IsNotNull(type.GetMethod("Serialize", new[] { typeof(Lab9.Blue.Blue) }));
//            Assert.IsNotNull(type.GetMethod("Deserialize", Type.EmptyTypes));
//            Assert.IsNotNull(type.GetMethod("EditFile", new[] { typeof(string) }));
//            Assert.IsNotNull(type.GetMethod("ChangeFileExtension", new[] { typeof(string) }));
//        }

//        [TestMethod]
//        public void Test_01_Serialize_Txt()
//        {
//            ISerializer<Lab9.Blue.Blue> manager = new BlueTxtFileManager<Lab9.Blue.Blue>("test");
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
//                Assert.IsFalse(string.IsNullOrWhiteSpace(content),
//                    $"Empty file for task {i}");

//                Assert.IsTrue(content.Contains(_input[i]),
//                    $"Input text not found in TXT file for task {i}");

//                Assert.IsTrue(content.Contains("Type:"),
//                    $"Type field not found in TXT for task {i}");
//            }

//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_02_Deserialize_Txt()
//        {
//            ISerializer<Lab9.Blue.Blue> manager = new BlueTxtFileManager<Lab9.Blue.Blue>("test");
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

//                Assert.AreEqual(_tasks[i].Input, result.Input, $"Input mismatch for task {i}");
//                Assert.AreEqual(_tasks[i].ToString(), result.ToString(), $"Output mismatch for task {i}");
//            }

//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_03_EditFile()
//        {
//            ISerializer<Lab9.Blue.Blue> manager = new BlueJsonFileManager<Lab9.Blue.Blue>("test");
//            var fileManager = (IFileManager)manager;
//            var controller = (IFileLifeController)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "JsonEdit");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");

//            for (int i = 0; i < 4; i++)
//            {
//                Init(i, true);

//                manager.Serialize(_tasks[i]);
//                controller.EditFile("NEW_TEXT");

//                var result = manager.Deserialize();

//                Assert.AreEqual("NEW_TEXT", result.Input, "EditFile failed");
//            }

//            Directory.Delete(folder, true);
//        }

//        private void Init(int i, bool review = false)
//        {
//            _tasks = new Lab9.Blue.Blue[]
//            {
//                 new Lab9.Blue.Task1(_input[i]),
//                 new Lab9.Blue.Task2(_input[i], _sequence[i]),
//                 new Lab9.Blue.Task3(_input[i]),
//                 new Lab9.Blue.Task4(_input[i])
//            };

//            if (review)
//                foreach (var t in _tasks)
//                    t.Review();
//        }
//    }
//}