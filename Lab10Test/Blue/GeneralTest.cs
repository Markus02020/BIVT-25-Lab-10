//using Lab10;
//using System.IO;

//namespace Lab10Test.Blue
//{
//    [TestClass]
//    public sealed class GeneralTest
//    {
//        private class TestManager : Lab10.Blue.BlueFileManager<Lab9.Blue.Blue>
//        {
//            public TestManager(string name) : base(name) { }

//            public TestManager(string name, string folder, string fileName, string ext = "txt")
//                : base(name, folder, fileName, ext) { }

//            public override void Serialize(Lab9.Blue.Blue obj) { }
//            public override Lab9.Blue.Blue Deserialize() => null;
//        }

//        [TestMethod]
//        public void Test_00_OOP_IFileManager()
//        {
//            var fileManagerInterface = typeof(IFileManager);
//            Assert.IsTrue(fileManagerInterface.IsInterface);
//            Assert.IsNotNull(fileManagerInterface.GetProperty("FolderPath"));
//            Assert.IsNotNull(fileManagerInterface.GetProperty("FileName"));
//            Assert.IsNotNull(fileManagerInterface.GetProperty("FileExtension"));
//            Assert.IsNotNull(fileManagerInterface.GetProperty("FullPath"));
//            Assert.IsNotNull(fileManagerInterface.GetMethod("SelectFolder", new[] { typeof(string) }));
//            Assert.IsNotNull(fileManagerInterface.GetMethod("ChangeFileName", new[] { typeof(string) }));
//            Assert.IsNotNull(fileManagerInterface.GetMethod("ChangeFileFormat", new[] { typeof(string) }));
//        }

//        [TestMethod]
//        public void Test_01_OOP_IFileLifeController()
//        {
//            var lifeInterface = typeof(IFileLifeController);
//            Assert.IsTrue(lifeInterface.IsInterface);
//            Assert.IsNotNull(lifeInterface.GetMethod("CreateFile", Type.EmptyTypes));
//            Assert.IsNotNull(lifeInterface.GetMethod("EditFile", new[] { typeof(string) }));
//            Assert.IsNotNull(lifeInterface.GetMethod("ChangeFileExtension", new[] { typeof(string) }));
//            Assert.IsNotNull(lifeInterface.GetMethod("DeleteFile", Type.EmptyTypes));
//        }

//        [TestMethod]
//        public void Test_02_OOP_ISerializer_T()
//        {
//            var serializerInterface = typeof(ISerializer<>);
//            Assert.IsTrue(serializerInterface.IsInterface, "ISerializer<T> must be an interface");

//            var serializeMethod = serializerInterface.GetMethod("Serialize");
//            Assert.IsNotNull(serializeMethod, "Serialize method is missing in ISerializer<T>");
//            Assert.AreEqual(1, serializeMethod.GetParameters().Length, "Serialize must take one parameter");

//            var deserializeMethod = serializerInterface.GetMethod("Deserialize");
//            Assert.IsNotNull(deserializeMethod, "Deserialize method is missing in ISerializer<T>");
//            Assert.AreEqual(0, deserializeMethod.GetParameters().Length, "Deserialize must take no parameters");
//        }

//        [TestMethod]
//        public void Test_03_OOP_MyFileManager()
//        {
//            var managerType = typeof(MyFileManager);

//            Assert.IsTrue(managerType.IsClass, "MyFileManager must be a class");
//            Assert.IsTrue(managerType.IsAbstract, "MyFileManager must be abstract");

//            Assert.IsNotNull(managerType.GetConstructor(new[] { typeof(string) }),
//                "Constructor MyFileManager(string name) is missing");

//            Assert.IsNotNull(managerType.GetConstructor(
//                new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
//                "Constructor MyFileManager(string, string, string, string) is missing");

//            var nameProp = managerType.GetProperty("Name");
//            Assert.IsNotNull(nameProp, "Property 'Name' is missing");
//            Assert.IsTrue(nameProp.CanRead, "Name must have getter");
//            Assert.IsTrue(!nameProp.CanWrite || !nameProp.SetMethod!.IsPublic, "Name should be read-only");

//            Assert.IsTrue(typeof(IFileManager).IsAssignableFrom(managerType));
//            Assert.IsTrue(typeof(IFileLifeController).IsAssignableFrom(managerType));
//        }

//        [TestMethod]
//        public void Test_04_OOP_BlueFileManager()
//        {
//            var blueType = typeof(Lab10.Blue.BlueFileManager<>);
//            var myFileManagerType = typeof(MyFileManager);

//            Assert.IsTrue(blueType.IsClass, "BlueFileManager<T> must be a class");
//            Assert.IsTrue(blueType.IsAbstract, "BlueFileManager<T> must be abstract");
//            Assert.IsTrue(blueType.IsSubclassOf(myFileManagerType),
//                "BlueFileManager<T> must inherit from MyFileManager");

//            var implementedInterfaces = blueType.GetInterfaces();
//            Assert.IsTrue(implementedInterfaces.Any(i =>
//                i.IsGenericType &&
//                i.GetGenericTypeDefinition() == typeof(ISerializer<>)),
//                "BlueFileManager<T> must implement ISerializer<T>");

//            var serialize = blueType.GetMethod("Serialize");
//            Assert.IsNotNull(serialize, "Serialize method is missing");
//            Assert.IsTrue(serialize.IsAbstract, "Serialize must be abstract");

//            var deserialize = blueType.GetMethod("Deserialize");
//            Assert.IsNotNull(deserialize, "Deserialize method is missing");
//            Assert.IsTrue(deserialize.IsAbstract, "Deserialize must be abstract");

//            var edit = blueType.GetMethod("EditFile", new[] { typeof(string) });
//            Assert.IsNotNull(edit, "EditFile must be overridden");
//            Assert.IsTrue(edit.IsVirtual, "EditFile must be virtual");
//            Assert.IsFalse(edit.IsAbstract, "EditFile must not be abstract");
//            Assert.AreEqual(blueType, edit.DeclaringType);

//            var changeExt = blueType.GetMethod("ChangeFileExtension", new[] { typeof(string) });
//            Assert.IsNotNull(changeExt, "ChangeFileExtension must be overridden");
//            Assert.IsTrue(changeExt.IsVirtual, "ChangeFileExtension must be virtual");
//            Assert.IsFalse(changeExt.IsAbstract, "ChangeFileExtension must not be abstract");
//            Assert.AreEqual(blueType, changeExt.DeclaringType);
//        }

//        [TestMethod]
//        public void Test_05_FileManager_Setup()
//        {
//            var manager = (IFileManager)new TestManager("test");
//            var folder = Directory.GetCurrentDirectory();
//            manager.SelectFolder(folder);
//            manager.ChangeFileName("task");
//            Assert.AreEqual(folder, manager.FolderPath);
//            Assert.AreEqual("task", manager.FileName);
//            Assert.IsTrue(manager.FullPath.Contains("task"));
//        }

//        [TestMethod]
//        public void Test_06_FileCreation()
//        {
//            var manager = (IFileManager)new TestManager("test");
//            var folder = Directory.GetCurrentDirectory();
//            manager.SelectFolder(folder);
//            manager.ChangeFileName("task");
//            ((IFileLifeController)manager).CreateFile();
//            Assert.IsTrue(File.Exists(manager.FullPath));
//        }

//        [TestMethod]
//        public void Test_07_ChangeFileFormat()
//        {
//            var manager = (IFileManager)new TestManager("test");
//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "BlueFormatTest");
//            Directory.CreateDirectory(folder);
//            manager.SelectFolder(folder);
//            manager.ChangeFileName("task");
//            manager.ChangeFileFormat("json");
//            Assert.AreEqual("json", manager.FileExtension);
//            Assert.IsTrue(File.Exists(manager.FullPath));
//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_08_EditFile()
//        {
//            var manager = new TestManager("test");
//            var fileManager = (IFileManager)manager;
//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "BlueEditTest");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");
//            manager.CreateFile();
//            manager.EditFile("HELLO");

//            var content = File.ReadAllText(fileManager.FullPath);
//            Assert.AreEqual("HELLO", content);

//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_09_ChangeFileExtension()
//        {
//            var manager = new TestManager("test");
//            var fileManager = (IFileManager)manager;
//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "BlueExtTest");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");
//            manager.CreateFile();
//            manager.EditFile("DATA");
//            manager.ChangeFileExtension("json");

//            Assert.AreEqual("json", fileManager.FileExtension);
//            Assert.IsTrue(File.Exists(fileManager.FullPath));

//            var content = File.ReadAllText(fileManager.FullPath);
//            Assert.AreEqual("DATA", content);

//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_10_DeleteFile()
//        {
//            var manager = new TestManager("test");
//            var fileManager = (IFileManager)manager;
//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "BlueDeleteTest");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");
//            manager.CreateFile();

//            var path = fileManager.FullPath;
//            manager.DeleteFile();

//            Assert.IsFalse(File.Exists(path));

//            Directory.Delete(folder, true);
//        }

//        [TestMethod]
//        public void Test_11_BlueFileManager_Overrides()
//        {
//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "BlueOverrideTest");
//            Directory.CreateDirectory(folder);

//            try
//            {
//                var manager = new TestManager("test", folder, "task", "txt");
//                var fileManager = (IFileManager)manager;
//                var lifeController = (IFileLifeController)manager;

//                fileManager.SelectFolder(folder);
//                fileManager.ChangeFileName("task");

//                lifeController.CreateFile();
//                lifeController.EditFile("Original Blue Content");

//                string originalPath = fileManager.FullPath;

//                lifeController.ChangeFileExtension("json");

//                Assert.AreEqual("json", fileManager.FileExtension);
//                Assert.IsTrue(File.Exists(fileManager.FullPath));
//                Assert.IsFalse(File.Exists(originalPath));

//                var content = File.ReadAllText(fileManager.FullPath);
//                Assert.AreEqual("Original Blue Content", content);

//                lifeController.EditFile("Updated Blue Content");
//                Assert.AreEqual("Updated Blue Content", File.ReadAllText(fileManager.FullPath));
//            }
//            finally
//            {
//                if (Directory.Exists(folder))
//                    Directory.Delete(folder, true);
//            }
//        }
//    }
//}