using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Engine
{
    public class ResourceManager
    {
        #region Attributes
            private ResourceCollection _resources;
            private IStorage _disk;
        #endregion
        #region Constructor
            public ResourceManager(IStorage disk) 
            {
                this._resources = new ResourceCollection();
                this._disk = disk;
            }
        #endregion

        #region Clear
            public void Clear() 
            {
                foreach (KeyValuePair<string, byte[]> entry in this._disk.ReadAll(false))
                    this._disk.Delete(entry.Key);
                this._resources.Clear();
            }
        #endregion
        #region Create
            public Resource Create(ResourceType type, string code, string name) 
            {
                return(this._resources.Create(null, type, code, name));
            }
        #endregion

        #region Load
            public void LoadAll() 
            {
                this._resources.Clear();
                foreach(KeyValuePair<string,byte[]> entry in this._disk.ReadAll(true))
                    this._resources.Add(this.Deserialize(entry.Value));
            }

            public Resource Load(string storageID)
            {
                this.LoadAll();
                return (this._resources.GetResourceByStorageID(storageID));
            }
        #endregion
        #region Save
            public void SaveAll() 
            {
                foreach (Resource resource in this._resources)
                    Save(resource);
            }

            public void Save(Resource resource)
            {
                _disk.Write(resource.StorageID, Serialize(resource));
            }
        #endregion

        #region Serialize
            private byte[] Serialize(Resource resource) 
            {
                MemoryStream memoryStream = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(resource.GetType());
                serializer.WriteObject(memoryStream, resource);
                return (memoryStream.ToArray());
            }
        #endregion
        #region Deserialize
            private Resource Deserialize(byte[] data) 
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Resource));
                MemoryStream memoryStream = new MemoryStream(data);
                return ((Resource)serializer.ReadObject(memoryStream));
            }
        #endregion

        #region Search
            public List<Resource> GetResources() 
            {
                return (new List<Resource>(this._resources));
            }

            public List<Resource> GetResources(ResourceType type)
            {
                return (new List<Resource>(this._resources.Where(r => r.Type == type)));
            }

            public Resource GetResource(ResourceType type, string name)
            {
                List<Resource> resources = new List<Resource>(this._resources.Where(r => (r.Type == type) && (r.Name == name)));
                if((resources != null) && (resources.Count == 1))
                    return(resources[0]);
                return (null);
            }

            public Resource GetResource(string code) 
            {
                return (this._resources.First(r => r.Code.ToString() == code));
            }
        #endregion
    }
}
