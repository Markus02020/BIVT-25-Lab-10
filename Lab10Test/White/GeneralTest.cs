//using Lab10;
//using Lab10.White;
//using Microsoft.VisualStudio.TestPlatform.Utilities;
//using System.Text.Json;
//namespace Lab10Test.White
//{
//    [TestClass]
//    public sealed class GeneralTest
//    {
//        private class TestManager : WhiteFileManager
//        {
//            public TestManager(string name) : base(name)
//            {
//            }
//			public TestManager(string name, string folder, string fileName, string ext = "txt")
//			: base(name, folder, fileName, ext) { }

//			public override void Serialize(Lab9.White.White obj)
//            {
//            }
//            public override Lab9.White.White Deserialize()
//            {
//                return null;
//            }
//        }
//        [TestMethod]
//        public void Test_00_OOP_IFileManager()
//        {
//            var fileManagerInterface = typeof(Lab10.IFileManager);
//            Assert.IsTrue(fileManagerInterface.IsInterface, "IFileManager must be interface");
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
//            var lifeInterface = typeof(Lab10.IFileLifeController);
//            Assert.IsTrue(lifeInterface.IsInterface, "IFileLifeController must be interface");
//            Assert.IsNotNull(lifeInterface.GetMethod("CreateFile", Type.EmptyTypes));
//            Assert.IsNotNull(lifeInterface.GetMethod("EditFile", new[] { typeof(string) }));
//            Assert.IsNotNull(lifeInterface.GetMethod("ChangeFileExtension", new[] { typeof(string) }));
//            Assert.IsNotNull(lifeInterface.GetMethod("DeleteFile", Type.EmptyTypes));
//        }
//        [TestMethod]
//        public void Test_02_OOP_IWhiteSerializer()
//        {
//            var serializerInterface = typeof(Lab10.IWhiteSerializer);

//            Assert.IsTrue(serializerInterface.IsInterface, "IWhiteSerializer must be interface");

//            Assert.IsNotNull(serializerInterface.GetMethod("Serialize", new[] { typeof(Lab9.White.White) }));
//            Assert.IsNotNull(serializerInterface.GetMethod("Deserialize", Type.EmptyTypes));
//        }
//        [TestMethod]
//        public void Test_03_OOP_MyFileManager()
//        {
//            var managerType = typeof(Lab10.MyFileManager);

//            Assert.IsTrue(managerType.IsClass, "MyFileManager must be class");
//            Assert.IsTrue(managerType.IsAbstract, "MyFileManager must be abstract");

//            Assert.IsNotNull(
//                managerType.GetConstructor(new[] { typeof(string) }),
//                "Constructor MyFileManager(string) missing"
//            );

//            Assert.IsNotNull(
//                managerType.GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
//                "Constructor MyFileManager(string, string, string, string) missing"
//            );

//			var nameProp = managerType.GetProperty("Name");
//			Assert.IsNotNull(nameProp, "Property 'Name' is missing in MyFileManager");
//			Assert.IsTrue(nameProp.CanRead, "Name must have a getter");
//            Assert.IsTrue(!nameProp.CanWrite || !nameProp.SetMethod!.IsPublic, "Name should be read-only");

//            var folderProp = managerType.GetProperty("FolderPath");
//            Assert.IsNotNull(folderProp);
//            Assert.IsTrue(folderProp.CanRead, "FolderPath must have getter");
//            Assert.IsTrue(!folderProp.CanWrite || folderProp.SetMethod!.IsPrivate, "FolderPath setter must be private or absent");

//			var fileNameProp = managerType.GetProperty("FileName");
//			Assert.IsNotNull(fileNameProp);
//			Assert.IsTrue(fileNameProp.CanRead, "FileName must have a getter");
//			Assert.IsTrue(!fileNameProp.CanWrite || fileNameProp.SetMethod!.IsPrivate, "FileName setter must be private or absent");

//			var extProp = managerType.GetProperty("FileExtension"); 
//            Assert.IsNotNull(extProp);
//			Assert.IsTrue(extProp.CanRead, "FileExtension must have a getter");
//            Assert.IsTrue(!extProp.CanWrite || extProp.SetMethod!.IsPrivate, "FileExtension setter must be private or absent");

//            var fullPathProp = managerType.GetProperty("FullPath"); 
//            Assert.IsNotNull(fullPathProp);
//			Assert.IsTrue(fullPathProp.CanRead, "FullPath must have a getter");
//            Assert.IsTrue(!fullPathProp.CanWrite || fullPathProp.SetMethod!.IsPrivate, "FullPath setter must be private or absent");

//			Assert.IsNotNull(managerType.GetMethod("SelectFolder", new[] { typeof(string) }));
//			Assert.IsNotNull(managerType.GetMethod("ChangeFileName", new[] { typeof(string) }),
//				"Method ChangeFileName(string) is missing");
//			Assert.IsNotNull(managerType.GetMethod("ChangeFileFormat", new[] { typeof(string) }),
//				"Method ChangeFileFormat(string) is missing");

//			Assert.IsNotNull(managerType.GetMethod("CreateFile", Type.EmptyTypes));
//			Assert.IsNotNull(managerType.GetMethod("DeleteFile", Type.EmptyTypes));

//			var editMethod = managerType.GetMethod("EditFile", new[] { typeof(string) });
//			Assert.IsNotNull(editMethod, "EditFile(string) is missing");
//			Assert.IsTrue(editMethod.IsVirtual, "EditFile must be virtual in MyFileManager");

//			var changeExtMethod = managerType.GetMethod("ChangeFileExtension", new[] { typeof(string) });
//			Assert.IsNotNull(changeExtMethod, "ChangeFileExtension(string) is missing");
//			Assert.IsTrue(changeExtMethod.IsVirtual, "ChangeFileExtension must be virtual in MyFileManager");
//		}
//        [TestMethod]
//        public void Test_04_OOP_WhiteFileManager()
//        {
//			var baseType = typeof(Lab10.MyFileManager);
//			var whiteType = typeof(Lab10.White.WhiteFileManager);
//			var serializerInterface = typeof(Lab10.IWhiteSerializer);

//			Assert.IsTrue(whiteType.IsClass, "WhiteFileManager must be a class");
//			Assert.IsTrue(whiteType.IsAbstract, "WhiteFileManager must be abstract");
//			Assert.IsTrue(whiteType.IsSubclassOf(baseType),
//				"WhiteFileManager must inherit from MyFileManager");

//			Assert.IsTrue(serializerInterface.IsAssignableFrom(whiteType),
//				"WhiteFileManager must implement IWhiteSerializer");

//			Assert.IsNotNull(whiteType.GetConstructor(new[] { typeof(string) }),
//			"WhiteFileManager(string name) constructor is missing");

//			Assert.IsNotNull(whiteType.GetConstructor(
//				new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
//				"WhiteFileManager(string, string, string, string) constructor is missing");

//			var serializeMethod = whiteType.GetMethod("Serialize", new[] { typeof(Lab9.White.White) });
//			Assert.IsNotNull(serializeMethod, "Serialize method is missing");
//			Assert.IsTrue(serializeMethod.IsAbstract, "Serialize must be abstract in WhiteFileManager");

//			var deserializeMethod = whiteType.GetMethod("Deserialize", Type.EmptyTypes);
//			Assert.IsNotNull(deserializeMethod, "Deserialize method is missing");
//			Assert.IsTrue(deserializeMethod.IsAbstract, "Deserialize must be abstract in WhiteFileManager");

//			var editOverride = whiteType.GetMethod("EditFile", new[] { typeof(string) });
//			Assert.IsNotNull(editOverride, "EditFile must be overridden in WhiteFileManager");
//			Assert.IsTrue(editOverride.IsVirtual, "EditFile must remain virtual");
//			Assert.IsFalse(editOverride.IsAbstract, "EditFile must NOT be abstract in WhiteFileManager");
//			Assert.AreEqual(whiteType, editOverride.DeclaringType,
//				"EditFile must be overridden in WhiteFileManager (not inherited without override)");

//			var changeExtOverride = whiteType.GetMethod("ChangeFileExtension", new[] { typeof(string) });
//			Assert.IsNotNull(changeExtOverride, "ChangeFileExtension must be overridden in WhiteFileManager");
//			Assert.IsTrue(changeExtOverride.IsVirtual, "ChangeFileExtension must remain virtual");
//			Assert.IsFalse(changeExtOverride.IsAbstract, "ChangeFileExtension must NOT be abstract");
//			Assert.AreEqual(whiteType, changeExtOverride.DeclaringType,
//				"ChangeFileExtension must be overridden in WhiteFileManager");
//		}
//        [TestMethod]
//        public void Test_05_Inheritance()
//        {
//            var fileManagerInterface = typeof(Lab10.IFileManager);
//            var lifeInterface = typeof(Lab10.IFileLifeController);
//            var serializerInterface = typeof(Lab10.IWhiteSerializer);
//            var managerType = typeof(Lab10.MyFileManager);

//            Assert.IsTrue(fileManagerInterface.IsAssignableFrom(managerType),
//                "MyFileManager must implement IFileManager");

//            Assert.IsTrue(lifeInterface.IsAssignableFrom(managerType),
//                "MyFileManager must implement IFileLifeController");
//        }
//        [TestMethod]
//        public void Test_06_FileManager_Setup()
//        {
//            var manager = (IFileManager)new TestManager("test");

//            var folder = Directory.GetCurrentDirectory();
//            manager.SelectFolder(folder);
//            manager.ChangeFileName("task");

//            Assert.AreEqual(folder, manager.FolderPath,
//                "FolderPath not set");

//            Assert.AreEqual("task", manager.FileName,
//                "FileName not set");

//            Assert.IsTrue(manager.FullPath.Contains("task"),
//                "FullPath incorrect");
//        }
//        [TestMethod]
//        public void Test_07_FileCreation()
//        {
//            var manager = (IFileManager)new TestManager("test");

//            var folder = Directory.GetCurrentDirectory();
//            Directory.CreateDirectory(folder);

//            manager.SelectFolder(folder);
//            manager.ChangeFileName("task");

//            ((IFileLifeController)manager).CreateFile();

//            Assert.IsTrue(File.Exists(manager.FullPath),
//                "File not created");
//        }
//        [TestMethod]
//        public void Test_08_ChangeFileFormat()
//        {
//            var manager = (IFileManager)new TestManager("test");

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "FormatTest");
//            Directory.CreateDirectory(folder);

//            manager.SelectFolder(folder);
//            manager.ChangeFileName("task");

//            manager.ChangeFileFormat("json");

//            Assert.AreEqual("json", manager.FileExtension,
//                "Format not changed");

//            Assert.IsTrue(File.Exists(manager.FullPath),
//                "File not created after format change");

//            Directory.Delete(folder, true);
//        }
//        [TestMethod]
//        public void Test_09_EditFile()
//        {
//            var manager = new TestManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "EditTest");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");

//            manager.CreateFile();
//            manager.EditFile("HELLO");

//            var content = File.ReadAllText(fileManager.FullPath);

//            Assert.AreEqual("HELLO", content,
//                "EditFile failed");

//            Directory.Delete(folder, true);
//        }
//        [TestMethod]
//        public void Test_10_ChangeFileExtension()
//        {
//            var manager = new TestManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "ExtTest");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");

//            manager.CreateFile();
//            manager.EditFile("DATA");

//            manager.ChangeFileExtension("json");

//            Assert.AreEqual("json", fileManager.FileExtension,
//                "Extension not changed");

//            Assert.IsTrue(File.Exists(fileManager.FullPath),
//                "File with new extension not created");

//            var content = File.ReadAllText(fileManager.FullPath);

//            Assert.AreEqual("DATA", content,
//                "Content lost after extension change");

//            Directory.Delete(folder, true);
//        }
//        [TestMethod]
//        public void Test_11_DeleteFile()
//        {
//            var manager = new TestManager("test");
//            var fileManager = (IFileManager)manager;

//            var folder = Path.Combine(Directory.GetCurrentDirectory(), "DeleteTest");
//            Directory.CreateDirectory(folder);

//            fileManager.SelectFolder(folder);
//            fileManager.ChangeFileName("task");

//            manager.CreateFile();

//            Assert.IsTrue(File.Exists(fileManager.FullPath),
//                "File not created");

//            var path = fileManager.FullPath;
//            manager.DeleteFile();

//            Assert.IsFalse(File.Exists(path),
//                "File not deleted");

//            Directory.Delete(folder, true);
//        }
//		[TestMethod]
//		public void Test_12_WhiteFileManager_Overrides()
//		{
//			var folder = Path.Combine(Directory.GetCurrentDirectory(), "OverrideTest");
//			Directory.CreateDirectory(folder);

//			try
//			{
//				var manager = new TestManager("test", folder, "task", "txt");

//				var fileManager = (IFileManager)manager;
//				var lifeController = (IFileLifeController)manager;

//				fileManager.SelectFolder(folder);
//				fileManager.ChangeFileName("task");

//				lifeController.CreateFile();
//				lifeController.EditFile("Original Content");

//				string originalPath = fileManager.FullPath;
//				Assert.IsTrue(File.Exists(originalPath), "Base CreateFile logic failed");

//				lifeController.ChangeFileExtension("json");

//				string newPath = fileManager.FullPath;
//				Assert.AreEqual("json", fileManager.FileExtension);
//				Assert.IsTrue(File.Exists(newPath), "File with new extension was not created");
//				Assert.IsFalse(File.Exists(originalPath), "Old file was not removed after extension change");

//				var content = File.ReadAllText(newPath);
//				Assert.AreEqual("Original Content", content,
//					"Content was lost after ChangeFileExtension in WhiteFileManager");

//				lifeController.EditFile("Updated Content");
//				Assert.AreEqual("Updated Content", File.ReadAllText(newPath),
//					"EditFile in WhiteFileManager did not work correctly");
//			}
//			finally
//			{
//				if (Directory.Exists(folder))
//					Directory.Delete(folder, true);
//			}
//		}
//	}
//}