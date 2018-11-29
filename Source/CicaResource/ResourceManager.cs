using Cica.CicaResource;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaResource
{
    public class ResourceManager
    {
        #region Properties
        private string PathResources 
        {
            get 
            {
                return(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\")));
            } 
        }

        private string PathTemporary 
        {
            get 
            {
                return (Path.GetTempPath());
            }
        }
        #endregion
        #region Constructor
        public ResourceManager()
        {
        }
        #endregion
        #region Clear
            public void Clear() 
            {
                //Delete
                foreach (string resource in this.ListRepositoryResources())
                    File.Delete(resource);
            }
        #endregion
        #region List
            public List<string> ListRepositoryResourcesNames() 
            {
                List<string> resourcesnames = new List<string>();
                foreach (string resource in this.ListRepositoryResources())
                    resourcesnames.Add(Path.GetFileName(resource));
                return (resourcesnames);
            }

            public List<string> ListRepositoryResourcesNames(ResourceType type) 
            {
                return (this.ListRepositoryResourcesNames().FindAll(r => r.EndsWith(Resource.GetResourceTypeExtension(type))));
            } 

            public List<string> ListRepositoryResources() 
            {
                return(new List<string>(Directory.GetFiles(this.PathResources)));
            }

            public List<string> ListRepositoryResources(ResourceType type)
            {
                return (new List<string>(Directory.GetFiles(this.PathResources)).FindAll(r => r.EndsWith(Resource.GetResourceTypeExtension(type))));
            }
        #endregion
        #region Load
            public string LoadData(string resourceFullNameWithExtension) 
            {
                string fileResource = string.Format(@"{0}\{1}", this.PathResources, resourceFullNameWithExtension);
                if (!File.Exists(fileResource))
                    return (string.Empty);
                return (this.GetString(File.ReadAllBytes(fileResource)));
            }

            public Resource Load(string resourceName)
            {
                return (Load(resourceName, this.PathResources, this.PathTemporary));
            }

            public Resource Load(string resourceName, string pathResources, string pathTemporary) 
            {
                string pathTemporaryResource = string.Format("{0}{1}", pathTemporary, resourceName);
                //Decompress
                if (!DecompressResource(string.Format("{0}{1}", pathResources, resourceName), pathTemporaryResource))
                    return (null);
                //Retrieve
                return (RetrieveResource(resourceName, pathTemporaryResource));
            }

            private Resource RetrieveResource(string resourceName, string resourceFolder)
            {
                //Deserialize
                string characterDefinition = string.Format(@"{0}\Definition.xml", resourceFolder);
                if (!File.Exists(characterDefinition))
                    return (null);
                Resource resource = this.DeserializeResource(resourceName, File.ReadAllText(characterDefinition));
                //Sprites
                foreach (Sprite sprite in resource.Sprites)
                    sprite.Data = new List<byte>(File.ReadAllBytes(string.Format(@"{0}\{1}.img", resourceFolder, sprite.Code.ToString("00000"))));
                return (resource);
            }

            private bool DecompressResource(string resourceFilePath, string sDir)
            {
                if (!File.Exists(resourceFilePath))
                    return (false);
                using (FileStream inFile = new FileStream(resourceFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                    {
                        while (DecompressFile(sDir, zipStream)) ;
                    }
                }
                return (true);
            }

            private bool DecompressFile(string sDir, GZipStream zipStream)
            {
                //Decompress file name
                byte[] bytes = new byte[sizeof(int)];
                int Readed = zipStream.Read(bytes, 0, sizeof(int));
                if (Readed < sizeof(int))
                    return false;
                int iNameLen = BitConverter.ToInt32(bytes, 0);
                bytes = new byte[sizeof(char)];
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < iNameLen; i++)
                {
                    zipStream.Read(bytes, 0, sizeof(char));
                    char c = BitConverter.ToChar(bytes, 0);
                    sb.Append(c);
                }
                string sFileName = sb.ToString();
                //Decompress file content
                bytes = new byte[sizeof(int)];
                zipStream.Read(bytes, 0, sizeof(int));
                int iFileLen = BitConverter.ToInt32(bytes, 0);
                bytes = new byte[iFileLen];
                zipStream.Read(bytes, 0, bytes.Length);
                string sFilePath = Path.Combine(sDir, sFileName);
                string sFinalDir = Path.GetDirectoryName(sFilePath);
                if (!Directory.Exists(sFinalDir))
                    Directory.CreateDirectory(sFinalDir);
                using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    outFile.Write(bytes, 0, iFileLen);
                return true;
            }

            private Resource DeserializeResource(string fileName, string data)
            {
                DataContractSerializer serializer = new DataContractSerializer(Resource.GetResourceTypeByExtension(fileName), null, int.MaxValue, false, true, null);
                MemoryStream memoryStream = new MemoryStream((new UTF8Encoding()).GetBytes(data));
                return ((Resource)serializer.ReadObject(memoryStream));
            }
        #endregion
        #region Save
            public bool Save(string resourceFullNameWithExtension, string data) 
            {
                string fileResource = string.Format(@"{0}\{1}", this.PathResources, resourceFullNameWithExtension);
                if (File.Exists(fileResource))
                    File.Delete(fileResource);
                File.WriteAllBytes(fileResource, GetBytes(data));
                return (true);
            } 

            public bool Save(Resource resource) 
            {
                return(Save(resource, this.PathResources, this.PathTemporary));
            }

            private bool Save(Resource resource, string pathResources, string pathTemporary)
            {
                string pathTemporaryResource = string.Format("{0}{1}", pathTemporary, resource.Code);
                //Persist
                if (!PersistResource(resource, pathTemporaryResource))
                    return (false);
                //Compress
                if (!CompressResource(resource, pathResources, pathTemporaryResource))
                    return (false);
                return (true);
            }

            private bool PersistResource(Resource resource, string pathTemporaryResource)
            {
                //Directory
                if (!Directory.Exists(pathTemporaryResource))
                    Directory.CreateDirectory(pathTemporaryResource);
                foreach (string file in Directory.GetFiles(pathTemporaryResource))
                    File.Delete(file);
                //Resource
                File.WriteAllText(string.Format(@"{0}\Definition.xml", pathTemporaryResource), SerializeResource(resource));
                //Sprites
                foreach (Sprite sprite in resource.Sprites)
                    File.WriteAllBytes(string.Format(@"{0}\{1}.img", pathTemporaryResource, sprite.Code.ToString("00000")), sprite.Data.ToArray());
                return (true);
            }

            private string SerializeResource(Resource resource)
            {
                MemoryStream memoryStream = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(resource.GetType(), null, int.MaxValue, false, true, null);
                serializer.WriteObject(memoryStream, resource);
                return (new UTF8Encoding().GetString(memoryStream.ToArray()));
            }

            private bool CompressResource(Resource resource, string pathResources, string pathResource)
            {
                //Resource File
                string fileResource = string.Format(@"{0}\{1}", pathResources, resource.FullNameWithExtension);
                if (File.Exists(fileResource))
                    File.Delete(fileResource);
                //Files
                string[] files = Directory.GetFiles(pathResource, "*.*", SearchOption.AllDirectories);
                int length = pathResource[pathResource.Length - 1] == Path.DirectorySeparatorChar ? pathResource.Length : pathResource.Length + 1;
                using (FileStream characterFileStream = new FileStream(fileResource, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (GZipStream characterFileZipStream = new GZipStream(characterFileStream, CompressionMode.Compress))
                    {
                        foreach (string file in files)
                            this.CompressFile(pathResource, file.Substring(length), characterFileZipStream);
                    }
                }
                return (true);
            }

            private void CompressFile(string directory, string path, GZipStream zipStream)
            {
                //Compress file name
                char[] chars = path.ToCharArray();
                zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
                foreach (char c in chars)
                    zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));
                //Compress file content
                byte[] bytes = File.ReadAllBytes(Path.Combine(directory, path));
                zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
                zipStream.Write(bytes, 0, bytes.Length);
            }
        #endregion

        #region Bytes
            internal byte[] GetBytes(string str)
            {
                byte[] bytes = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
                return bytes;
            }

            internal string GetString(byte[] bytes)
            {
                char[] chars = new char[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
                return new string(chars);
            }
        #endregion
    }
}
