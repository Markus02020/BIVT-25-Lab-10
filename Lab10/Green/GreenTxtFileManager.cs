using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Green
{
    public class GreenTxtFileManager : GreenFileManager
    {
        public GreenTxtFileManager(string name) : base(name) { }

        public GreenTxtFileManager(string name, string folder, string fileName, string extension = "txt")
            : base(name, folder, fileName, extension) { }

        public override void EditFile(string input)
        {
            if (File.Exists(FullPath))
            {
                var obj = Deserialize<Lab9.Green.Green>();
                if (obj != null)
                {
                    obj.ChangeText(input);
                    Serialize(obj);
                }
            }
        }

        public override void ChangeFileExtension(string input)
        {
            ChangeFileFormat("txt");
        }

        public override void Serialize<T>(T obj)
        {
            if (obj == null) return;
            string content = $"TypeName:{obj.GetType().FullName}\n";

            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var val = prop.GetValue(obj);
                if (val != null)
                {
                    if (val is string[] arr)
                        content += $"{prop.Name}:[{string.Join("|||", arr)}]\n";
                    else
                        content += $"{prop.Name}:{val.ToString().Replace("\n", "\\n").Replace("\r", "\\r")}\n";
                }
            }
            File.WriteAllText(FullPath, content);
        }

        public override T Deserialize<T>()
        {
            if (!File.Exists(FullPath)) return null;
            string[] lines = File.ReadAllLines(FullPath);
            string typeName = lines.FirstOrDefault(l => l.StartsWith("TypeName:"))?.Substring(9).Trim();

            Type actualType = typeof(T);
            if (!string.IsNullOrEmpty(typeName))
            {
                var found = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(typeName)).FirstOrDefault(x => x != null);
                if (found != null) actualType = found;
            }

            var dict = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string line in lines)
            {
                int colonIndex = line.IndexOf(':');
                if (colonIndex > 0)
                {
                    string key = line.Substring(0, colonIndex).Trim();
                    string value = line.Substring(colonIndex + 1).Trim().Replace("\\n", "\n").Replace("\\r", "\r");
                    dict[key] = value;
                }
            }

            object obj = null;
            var ctors = actualType.GetConstructors().OrderByDescending(c => c.GetParameters().Length);
            foreach (var ctor in ctors)
            {
                var pInfos = ctor.GetParameters();
                var args = new object[pInfos.Length];
                bool ok = true;
                for (int i = 0; i < pInfos.Length; i++)
                {
                    string pName = pInfos[i].Name.ToLower();
                    string key = dict.Keys.FirstOrDefault(k => k.ToLower() == pName || k.ToLower().Contains(pName) || pName.Contains(k.ToLower()));
                    if (key == null && (pName == "text" || pName == "str"))
                        key = dict.Keys.FirstOrDefault(k => k.ToLower().Contains("input"));

                    if (key != null)
                    {
                        try { args[i] = Convert.ChangeType(dict[key], pInfos[i].ParameterType); }
                        catch { ok = false; }
                    }
                    else
                    {
                        args[i] = null;
                    }
                }
                if (ok)
                {
                    try
                    {
                        obj = ctor.Invoke(args);
                        break;
                    }
                    catch { }
                }
            }

            if (obj == null)
            {
                try { obj = Activator.CreateInstance(actualType); }
                catch { obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(actualType); }
            }

            var reviewMethod = obj?.GetType().GetMethod("Review");
            if (reviewMethod != null)
            {
                try { reviewMethod.Invoke(obj, null); } catch { }
            }

            return (T)obj;
        }
    }
}