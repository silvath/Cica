using Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindowsPhone
{
    public class StorageWindowsPhone : IStorage
    {
        #region Properties
            public Assembly Assembly { set; get; }
        #endregion
        #region Constructors
            public StorageWindowsPhone(Assembly assembly) 
            {
                this.Assembly = assembly;
            }
        #endregion

        #region Read
            Dictionary<string, byte[]> IStorage.ReadAll(bool loadData)
            {
                Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>();
                foreach (string resourceName in this.Assembly.GetManifestResourceNames()) 
                {
                    byte[] data = null;
                    if (loadData)
                    {
                        Stream stream = this.Assembly.GetManifestResourceStream(resourceName);
                        data = new byte[stream.Length];
                        stream.Read(data,0, data.Length);
                        stream.Dispose();
                    }
                    string resourceFileName = GetResourceFileName(resourceName);
                    if (string.IsNullOrEmpty(resourceFileName))
                        continue;
                    resources.Add(resourceFileName, data);
                }
                return (resources);
            }

            private string GetResourceFileName(string resourceName) 
            {
                string[] names = resourceName.Split('.');
                if (names.Length < 2)
                    return (string.Empty);
                return (string.Format("{0}.{1}", names[names.Length - 2], names[names.Length - 1]));
            }
        #endregion
        #region Write
            void IStorage.Write(string name, byte[] data)
            {
            }
        #endregion
        #region Delete
            void IStorage.Delete(string name) 
            {
            }
        #endregion
    }
}
