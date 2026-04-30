//using Lab10;
//using Lab10.Purple;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json.Linq;
//using System.Text.Json;

//namespace Lab10Test.Purple
//{
//    [TestClass]
//    public sealed class JsonTest
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

//            // Правильная десериализация codes — массив массивов объектов
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

//        [TestMethod]
//        public void Test_00_OOP_JsonManager()
//        {
//            var type = typeof(PurpleJsonFileManager<Lab9.Purple.Purple>);

//            Assert.IsTrue(type.IsClass);
//            Assert.IsFalse(type.IsAbstract);

//            Assert.IsTrue(typeof(PurpleFileManager<Lab9.Purple.Purple>).IsAssignableFrom(type));
//            Assert.IsTrue(typeof(ISerializer<Lab9.Purple.Purple>).IsAssignableFrom(type));

//            Assert.IsNotNull(type.GetConstructor(new[] { typeof(string) }));
//            Assert.IsNotNull(type.GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string) }));

//            Assert.IsNotNull(type.GetMethod("Serialize", new[] { typeof(Lab9.Purple.Purple) }));
//            Assert.IsNotNull(type.GetMethod("Deserialize", Type.EmptyTypes));
//            Assert.IsNotNull(type.GetMethod("EditFile", new[] { typeof(string) }));
//            Assert.IsNotNull(type.GetMethod("ChangeFileExtension", new[] { typeof(string) }));
//        }

//        [TestMethod]
//        public void Test_01_Serialize_JSON()
//        {
//            ISerializer<Lab9.Purple.Purple> manager = new PurpleJsonFileManager<Lab9.Purple.Purple>("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "PurpleJsonTest1");
//            Directory.CreateDirectory(folder);
//            fileManager.SelectFolder(folder);

//            for (int i = 0; i < 4; i++)   // обычно тестируем 4 задачи
//            {
//                Init(i, true);

//                fileManager.ChangeFileName($"task{i}");
//                manager.Serialize(_tasks[i]);

//                Assert.IsTrue(File.Exists(fileManager.FullPath), $"File not created for task {i}");

//                var content = File.ReadAllText(fileManager.FullPath);
//                Assert.IsFalse(string.IsNullOrWhiteSpace(content), $"Empty file for task {i}");

//                var json = JObject.Parse(content);
//                Assert.AreEqual(_input[i], json["Input"]?.ToString(), $"Input mismatch for task {i}");
//            }

//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_02_Deserialize_JSON()
//        {
//            ISerializer<Lab9.Purple.Purple> manager = new PurpleJsonFileManager<Lab9.Purple.Purple>("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "PurpleJsonTest2");
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
//            ISerializer<Lab9.Purple.Purple> manager = new PurpleJsonFileManager<Lab9.Purple.Purple>("test");
//            var fileManager = (IFileManager)manager;
//            var controller = (IFileLifeController)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "PurpleJsonEdit");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");

//            for (int i = 0; i < 4; i++)
//            {
//                Init(i, true);

//                manager.Serialize(_tasks[i]);
//                controller.EditFile("NEW_TEXT");

//                var result = manager.Deserialize();

//                Assert.IsNotNull(result, "Deserialize returned null after EditFile");
//                Assert.AreEqual("NEW_TEXT", result.Input, "EditFile failed");
//            }

//            Directory.Delete(folder, true);
//        }

//        private void Init(int i, bool review = false)
//        {
//            var taskCodes = _codes[i];

//            _tasks = new Lab9.Purple.Purple[]
//            {
//                 new Lab9.Purple.Task1(_input[i]),
//                 new Lab9.Purple.Task2(_input[i]),
//                 new Lab9.Purple.Task3(_input[i]),
//                 new Lab9.Purple.Task4(_input[i], taskCodes)
//            };

//            if (review)
//                foreach (var t in _tasks)
//                    t.Review();
//        }
//        private class CodePair
//        {
//            public string pair { get; set; } = "";
//            public string code { get; set; } = "";
//        }
//    }
//}