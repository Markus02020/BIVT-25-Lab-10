using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Lab10.Green
{
    public class GreenJsonFileManager : GreenFileManager
    {
        public GreenJsonFileManager(string name) : base(name) { }

        public GreenJsonFileManager(string name, string folder, string fileName, string extension = "json")
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
            ChangeFileFormat("json");
        }

        public override void Serialize<T>(T obj)
        {
            if (obj == null) return;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(obj, obj.GetType(), options);

            if (json.StartsWith("{"))
            {
                json = json.Insert(1, $"\n  \"TypeName\": \"{obj.GetType().FullName}\",");
            }
            File.WriteAllText(FullPath, json);
        }

        public override T Deserialize<T>()
        {
            if (!File.Exists(FullPath)) return null;
            string json = File.ReadAllText(FullPath);
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    string typeName = root.TryGetProperty("TypeName", out var typeEl) ? typeEl.GetString() : null;
                    Type actualType = typeof(T);

                    if (!string.IsNullOrEmpty(typeName))
                    {
                        var foundType = AppDomain.CurrentDomain.GetAssemblies()
                            .Select(a => a.GetType(typeName))
                            .FirstOrDefault(t => t != null);
                        if (foundType != null) actualType = foundType;
                    }

                    var dict = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var prop in root.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == JsonValueKind.String)
                            dict[prop.Name] = prop.Value.GetString();
                        else
                            dict[prop.Name] = prop.Value.GetRawText();
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
                                try
                                {
                                    if (pInfos[i].ParameterType == typeof(string))
                                        args[i] = dict[key];
                                    else
                                        args[i] = JsonSerializer.Deserialize(dict[key], pInfos[i].ParameterType);
                                }
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
            catch { }
            return null;
        }
    }
}