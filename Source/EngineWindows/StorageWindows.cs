using Data;
using Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindows
{
    public class StorageWindows : IStorage
    {
        #region Attributes
            private static string _pathDefault = string.Empty;
        #endregion
        #region Properties
            public string Path { set; get; }
            public static string PathDefault 
            {
                set 
                {
                    _pathDefault = value;
                }
                get 
                {
                    if (string.IsNullOrEmpty(_pathDefault))
                    {
                        string path = @"C:\Files\Projects\Cica\Games\Airplane";
                        if (Directory.Exists(path))
                            _pathDefault = path;    
                    }
                    return (_pathDefault);
                } 
            }
        #endregion
        #region Constructors
            public StorageWindows(string path) 
            {
                this.Path = path;
            }
        #endregion

        #region Paths
            public static string GetPathFolderResources()
            {
                if (!string.IsNullOrEmpty(PathDefault))
                    return (string.Format(@"{0}\Resources\", PathDefault));
                string path = string.Format(@"{0}\Resources\",Directory.GetCurrentDirectory());
                if (Directory.Exists(path))
                    return (path);
                return (string.Empty);
            }

            public static string GetPathFolderImages()
            {
                if (!string.IsNullOrEmpty(_pathDefault))
                    return (string.Format(@"{0}\Images\", PathDefault));
                string path = string.Format(@"{0}\Images\", Directory.GetCurrentDirectory());
                if (Directory.Exists(path))
                    return (path);
                return (string.Empty);
            }
        #endregion

        #region Read
            Dictionary<string, byte[]> IStorage.ReadAll(bool loadData)
            {
                Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>();
                foreach(string file in Directory.EnumerateFiles(this.Path))
                {
                    string name = System.IO.Path.GetFileName(file);
                    byte[] data = loadData ? File.ReadAllBytes(file) : null;
                    resources.Add(name, data);
                }
                return (resources);
            }
        #endregion
        #region Write
            void IStorage.Write(string name, byte[] data)
            {
                ((IStorage)this).Delete(name);
                string fullName = string.Format("{0}{1}", this.Path, name);
                File.WriteAllBytes(fullName, data);
            }
        #endregion
        #region Delete
            void IStorage.Delete(string name) 
            {
                string fullName = string.Format("{0}{1}", this.Path, name);
                if (File.Exists(fullName))
                    File.Delete(fullName);
            }
        #endregion
    }
}
