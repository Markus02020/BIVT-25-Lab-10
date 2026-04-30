//using Lab10;
//using System.Text.Json;
//using System.IO;

//namespace Lab10Test.Green
//{
//	[TestClass]
//	public sealed class GeneralTest
//	{
//		private class TestManager : Lab10.Green.GreenFileManager
//		{
//			public TestManager(string name) : base(name) { }
//			public TestManager(string name, string folder, string fileName, string ext = "txt")
//				: base(name, folder, fileName, ext) { }

//			public override void Serialize<T>(T obj) { }
//			public override T Deserialize<T>() => null;
//		}

//		[TestMethod]
//		public void Test_00_OOP_IFileManager()
//		{
//			var fileManagerInterface = typeof(IFileManager);
//			Assert.IsTrue(fileManagerInterface.IsInterface);
//			Assert.IsNotNull(fileManagerInterface.GetProperty("FolderPath"));
//			Assert.IsNotNull(fileManagerInterface.GetProperty("FileName"));
//			Assert.IsNotNull(fileManagerInterface.GetProperty("FileExtension"));
//			Assert.IsNotNull(fileManagerInterface.GetProperty("FullPath"));
//			Assert.IsNotNull(fileManagerInterface.GetMethod("SelectFolder", new[] { typeof(string) }));
//			Assert.IsNotNull(fileManagerInterface.GetMethod("ChangeFileName", new[] { typeof(string) }));
//			Assert.IsNotNull(fileManagerInterface.GetMethod("ChangeFileFormat", new[] { typeof(string) }));
//		}

//		[TestMethod]
//		public void Test_01_OOP_IFileLifeController()
//		{
//			var lifeInterface = typeof(IFileLifeController);
//			Assert.IsTrue(lifeInterface.IsInterface);
//			Assert.IsNotNull(lifeInterface.GetMethod("CreateFile", Type.EmptyTypes));
//			Assert.IsNotNull(lifeInterface.GetMethod("EditFile", new[] { typeof(string) }));
//			Assert.IsNotNull(lifeInterface.GetMethod("ChangeFileExtension", new[] { typeof(string) }));
//			Assert.IsNotNull(lifeInterface.GetMethod("DeleteFile", Type.EmptyTypes));
//		}

//		[TestMethod]
//		public void Test_02_OOP_ISerializer()
//		{
//			var serializerInterface = typeof(ISerializer);
//			Assert.IsTrue(serializerInterface.IsInterface);

//			var serialize = serializerInterface.GetMethod("Serialize");
//			Assert.IsNotNull(serialize, "Serialize method is missing");
//			Assert.IsTrue(serialize.IsGenericMethod, "Serialize must be generic");

//			var deserialize = serializerInterface.GetMethod("Deserialize");
//			Assert.IsNotNull(deserialize, "Deserialize method is missing");
//			Assert.IsTrue(deserialize.IsGenericMethod, "Deserialize must be generic");

//			var serializeParams = serialize.GetParameters();
//			Assert.AreEqual(1, serializeParams.Length);
//			Assert.IsTrue(serializeParams[0].ParameterType.IsGenericParameter);

//			var deserializeReturn = deserialize.ReturnType;
//			Assert.IsTrue(deserializeReturn.IsGenericParameter);
//		}

//		[TestMethod]
//		public void Test_03_OOP_MyFileManager()
//		{
//			var managerType = typeof(MyFileManager);

//			Assert.IsTrue(managerType.IsClass, "MyFileManager must be a class");
//			Assert.IsTrue(managerType.IsAbstract, "MyFileManager must be abstract");

//			Assert.IsNotNull(managerType.GetConstructor(new[] { typeof(string) }),
//				"Constructor MyFileManager(string) is missing");

//			Assert.IsNotNull(managerType.GetConstructor(
//				new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
//				"Constructor MyFileManager(string, string, string, string) is missing");

//			var nameProp = managerType.GetProperty("Name");
//			Assert.IsNotNull(nameProp, "Property Name is missing");
//			Assert.IsTrue(nameProp.CanRead);
//            Assert.IsTrue(!nameProp.CanWrite || !nameProp.SetMethod!.IsPublic, "Name should be read-only");

//            var folderProp = managerType.GetProperty("FolderPath");
//			Assert.IsNotNull(folderProp);
//			Assert.IsTrue(folderProp.CanRead);
//			Assert.IsTrue(folderProp.SetMethod?.IsPrivate ?? false);

//			Assert.IsTrue(typeof(IFileManager).IsAssignableFrom(managerType));
//			Assert.IsTrue(typeof(IFileLifeController).IsAssignableFrom(managerType));
//		}

//		[TestMethod]
//		public void Test_04_OOP_GreenFileManager()
//		{
//			var greenType = typeof(Lab10.Green.GreenFileManager);
//			var myFileManagerType = typeof(MyFileManager);
//			var serializerInterface = typeof(ISerializer);

//			Assert.IsTrue(greenType.IsClass);
//			Assert.IsTrue(greenType.IsAbstract);
//			Assert.IsTrue(greenType.IsSubclassOf(myFileManagerType));

//			Assert.IsTrue(serializerInterface.IsAssignableFrom(greenType));
			
//			var serialize = greenType.GetMethod("Serialize");
//			Assert.IsNotNull(serialize);
//			Assert.IsTrue(serialize.IsAbstract);
//			Assert.IsTrue(serialize.IsGenericMethod);

//			var deserialize = greenType.GetMethod("Deserialize");
//			Assert.IsNotNull(deserialize);
//			Assert.IsTrue(deserialize.IsAbstract);
//			Assert.IsTrue(deserialize.IsGenericMethod);

//			var edit = greenType.GetMethod("EditFile", new[] { typeof(string) });
//			Assert.IsNotNull(edit);
//			Assert.IsTrue(edit.IsVirtual);
//			Assert.IsFalse(edit.IsAbstract);
//			Assert.AreEqual(greenType, edit.DeclaringType);

//			var changeExt = greenType.GetMethod("ChangeFileExtension", new[] { typeof(string) });
//			Assert.IsNotNull(changeExt);
//			Assert.IsTrue(changeExt.IsVirtual);
//			Assert.IsFalse(changeExt.IsAbstract);
//			Assert.AreEqual(greenType, changeExt.DeclaringType);
//		}

//		[TestMethod]
//		public void Test_05_FileManager_Setup()
//		{
//			var manager = (IFileManager)new TestManager("test");
//			var folder = Directory.GetCurrentDirectory();
//			manager.SelectFolder(folder);
//			manager.ChangeFileName("task");
//			Assert.AreEqual(folder, manager.FolderPath);
//			Assert.AreEqual("task", manager.FileName);
//			Assert.IsTrue(manager.FullPath.Contains("task"));
//		}

//		[TestMethod]
//		public void Test_06_FileCreation()
//		{
//			var manager = (IFileManager)new TestManager("test");
//			var folder = Directory.GetCurrentDirectory();
//			manager.SelectFolder(folder);
//			manager.ChangeFileName("task");
//			((IFileLifeController)manager).CreateFile();
//			Assert.IsTrue(File.Exists(manager.FullPath));
//		}

//		[TestMethod]
//		public void Test_07_ChangeFileFormat()
//		{
//			var manager = (IFileManager)new TestManager("test");
//			var folder = Path.Combine(Directory.GetCurrentDirectory(), "GreenFormatTest");
//			Directory.CreateDirectory(folder);
//			manager.SelectFolder(folder);
//			manager.ChangeFileName("task");
//			manager.ChangeFileFormat("json");
//			Assert.AreEqual("json", manager.FileExtension);
//			Assert.IsTrue(File.Exists(manager.FullPath));
//			Directory.Delete(folder, true);
//		}

//		[TestMethod]
//		public void Test_08_EditFile()
//		{
//			var manager = new TestManager("test");
//			var fileManager = (IFileManager)manager;
//			var folder = Path.Combine(Directory.GetCurrentDirectory(), "GreenEditTest");
//			Directory.CreateDirectory(folder);

//			fileManager.SelectFolder(folder);
//			fileManager.ChangeFileName("task");
//			manager.CreateFile();
//			manager.EditFile("HELLO");

//			var content = File.ReadAllText(fileManager.FullPath);
//			Assert.AreEqual("HELLO", content);

//			Directory.Delete(folder, true);
//		}

//		[TestMethod]
//		public void Test_09_ChangeFileExtension()
//		{
//			var manager = new TestManager("test");
//			var fileManager = (IFileManager)manager;
//			var folder = Path.Combine(Directory.GetCurrentDirectory(), "GreenExtTest");
//			Directory.CreateDirectory(folder);

//			fileManager.SelectFolder(folder);
//			fileManager.ChangeFileName("task");
//			manager.CreateFile();
//			manager.EditFile("DATA");
//			manager.ChangeFileExtension("json");

//			Assert.AreEqual("json", fileManager.FileExtension);
//			Assert.IsTrue(File.Exists(fileManager.FullPath));

//			var content = File.ReadAllText(fileManager.FullPath);
//			Assert.AreEqual("DATA", content);

//			Directory.Delete(folder, true);
//		}

//		[TestMethod]
//		public void Test_10_DeleteFile()
//		{
//			var manager = new TestManager("test");
//			var fileManager = (IFileManager)manager;
//			var folder = Path.Combine(Directory.GetCurrentDirectory(), "GreenDeleteTest");
//			Directory.CreateDirectory(folder);

//			fileManager.SelectFolder(folder);
//			fileManager.ChangeFileName("task");
//			manager.CreateFile();

//			var path = fileManager.FullPath;
//			manager.DeleteFile();

//			Assert.IsFalse(File.Exists(path));

//			Directory.Delete(folder, true);
//		}

//		[TestMethod]
//		public void Test_11_WhiteFileManager_Overrides()
//		{
//			var folder = Path.Combine(Directory.GetCurrentDirectory(), "GreenOverrideTest");
//			Directory.CreateDirectory(folder);

//			try
//			{
//				var manager = new TestManager("test", folder, "task", "txt");
//				var fileManager = (IFileManager)manager;
//				var lifeController = (IFileLifeController)manager;

//				fileManager.SelectFolder(folder);
//				fileManager.ChangeFileName("task");

//				lifeController.CreateFile();
//				lifeController.EditFile("Original Green Content");

//				string originalPath = fileManager.FullPath;

//				lifeController.ChangeFileExtension("json");

//				string newPath = fileManager.FullPath;

//				Assert.AreEqual("json", fileManager.FileExtension);
//				Assert.IsTrue(File.Exists(newPath));
//				Assert.IsFalse(File.Exists(originalPath));

//				var content = File.ReadAllText(newPath);
//				Assert.AreEqual("Original Green Content", content);

//				lifeController.EditFile("Updated Green Content");
//				Assert.AreEqual("Updated Green Content", File.ReadAllText(newPath));
//			}
//			finally
//			{
//				if (Directory.Exists(folder))
//					Directory.Delete(folder, true);
//			}
//		}
//	}
//}