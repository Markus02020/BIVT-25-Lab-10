using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Green
{
    public abstract class GreenFileManager : MyFileManager, ISerializer
    {
        public GreenFileManager(string name) : base(name) { }

        public GreenFileManager(string name, string folder, string fileName, string extension = "txt")
            : base(name, folder, fileName, extension) { }

        public override void EditFile(string input) { base.EditFile(input); }

        public override void ChangeFileExtension(string input) { base.ChangeFileExtension(input); }

        public abstract void Serialize<T>(T obj) where T : Lab9.Green.Green;

        public abstract T Deserialize<T>() where T : Lab9.Green.Green;
    }
}