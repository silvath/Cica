using Cica.CicaResource;

namespace Cica.CicaEngine
{
    public class ResourceManager
    {
        #region Properties
            public ResourceCollection Resources { set; get; }
        #endregion
        #region Constructor
            public ResourceManager() 
            {
                this.Resources = new ResourceCollection();
            }
        #endregion
        #region Load
            //public static Character LoadCharacter(string characterName)
            //{
            //    string characterFile = string.Format(@"{0}{1}.cica", Config.CharactersFolder, characterName);
            //    if (!File.Exists(characterFile))
            //        return (null);
            //    return (LoadCharacterFile(characterFile));
            //}

            //private static Character LoadCharacterFile(string characterFilePath)
            //{
            //    string characterFolder = string.Format("{0}{1}", Config.TempFolder, Path.GetFileNameWithoutExtension(characterFilePath));
            //    //Decompress
            //    if (!DecompressCharacter(characterFilePath, characterFolder))
            //        return (null);
            //    //Retrieve
            //    return (RetrieveCharacter(characterFolder));
            //}

            //private static Character RetrieveCharacter(string characterFolder)
            //{
            //    //Deserialize
            //    string characterDefinition = string.Format(@"{0}\Definition.xml", characterFolder);
            //    if (!File.Exists(characterDefinition))
            //        return (null);
            //    Character character = DeserializeCharacter(File.ReadAllText(characterDefinition));
            //    //Images
            //    foreach (CharacterImage charaterImage in character.Images)
            //        charaterImage.Data = File.ReadAllBytes(string.Format(@"{0}\{1}.img", characterFolder, charaterImage.ImageCode.ToString("0000")));
            //    return (character);
            //}

            //private static bool DecompressCharacter(string characterFilePath, string sDir)
            //{
            //    using (FileStream inFile = new FileStream(characterFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
            //    {
            //        using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
            //        {
            //            while (DecompressFile(sDir, zipStream)) ;
            //        }
            //    }
            //    return (true);
            //}

            //private static bool DecompressFile(string sDir, GZipStream zipStream)
            //{
            //    //Decompress file name
            //    byte[] bytes = new byte[sizeof(int)];
            //    int Readed = zipStream.Read(bytes, 0, sizeof(int));
            //    if (Readed < sizeof(int))
            //        return false;
            //    int iNameLen = BitConverter.ToInt32(bytes, 0);
            //    bytes = new byte[sizeof(char)];
            //    StringBuilder sb = new StringBuilder();
            //    for (int i = 0; i < iNameLen; i++)
            //    {
            //        zipStream.Read(bytes, 0, sizeof(char));
            //        char c = BitConverter.ToChar(bytes, 0);
            //        sb.Append(c);
            //    }
            //    string sFileName = sb.ToString();
            //    //Decompress file content
            //    bytes = new byte[sizeof(int)];
            //    zipStream.Read(bytes, 0, sizeof(int));
            //    int iFileLen = BitConverter.ToInt32(bytes, 0);
            //    bytes = new byte[iFileLen];
            //    zipStream.Read(bytes, 0, bytes.Length);
            //    string sFilePath = Path.Combine(sDir, sFileName);
            //    string sFinalDir = Path.GetDirectoryName(sFilePath);
            //    if (!Directory.Exists(sFinalDir))
            //        Directory.CreateDirectory(sFinalDir);
            //    using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            //        outFile.Write(bytes, 0, iFileLen);
            //    return true;
            //}

            //private static Character DeserializeCharacter(string data)
            //{
            //    DataContractSerializer serializer = new DataContractSerializer(typeof(Character), null, int.MaxValue, false, true, null);
            //    MemoryStream memoryStream = new MemoryStream((new UTF8Encoding()).GetBytes(data));
            //    return ((Character)serializer.ReadObject(memoryStream));
            //}
        #endregion
        #region Save
            //public static bool SaveCharacter(Character character)
            //{
            //    return (SaveCharacter(character, Config.CharactersFolder));
            //}

            //public static bool SaveCharacter(Character character, string charactersPath)
            //{
            //    return (SaveCharacter(character, charactersPath, Config.TempFolder));
            //}

            //private static bool SaveCharacter(Character character, string charactersPath, string tempPath)
            //{
            //    string characterPath = string.Format("{0}{1}", tempPath, character.Name);
            //    //Persist
            //    if (!PersistCharacter(character, characterPath))
            //        return (false);
            //    //Compress
            //    if (!CompressCharacter(character, charactersPath, characterPath))
            //        return (false);
            //    return (true);
            //}

            //private static bool PersistCharacter(Character character, string characterPath)
            //{
            //    //Directory
            //    if (!Directory.Exists(characterPath))
            //        Directory.CreateDirectory(characterPath);
            //    foreach (string file in Directory.GetFiles(characterPath))
            //        File.Delete(file);
            //    //Character 
            //    File.WriteAllText(string.Format(@"{0}\Definition.xml", characterPath), SerializeCharacter(character));
            //    //Images
            //    foreach (CharacterImage image in character.Images)
            //        File.WriteAllBytes(string.Format(@"{0}\{1}.img", characterPath, image.ImageCode.ToString("0000")), image.Data);
            //    return (true);
            //}

            //private static string SerializeCharacter(Character character)
            //{
            //    MemoryStream memoryStream = new MemoryStream();
            //    DataContractSerializer serializer = new DataContractSerializer(typeof(Character), null, int.MaxValue, false, true, null);
            //    serializer.WriteObject(memoryStream, character);
            //    return (new UTF8Encoding().GetString(memoryStream.ToArray()));
            //}

            //private static bool CompressCharacter(Character character, string charactersPath, string characterPath)
            //{
            //    //Character File
            //    string characterFile = string.Format("{0}{1}.cica", charactersPath, character.Name);
            //    if (File.Exists(characterFile))
            //        File.Delete(characterFile);
            //    //Files
            //    string[] files = Directory.GetFiles(characterPath, "*.*", SearchOption.AllDirectories);
            //    int length = characterPath[characterPath.Length - 1] == Path.DirectorySeparatorChar ? characterPath.Length : characterPath.Length + 1;
            //    using (FileStream characterFileStream = new FileStream(characterFile, FileMode.Create, FileAccess.Write, FileShare.None))
            //    {
            //        using (GZipStream characterFileZipStream = new GZipStream(characterFileStream, CompressionMode.Compress))
            //        {
            //            foreach (string file in files)
            //                CompressFile(characterPath, file.Substring(length), characterFileZipStream);
            //        }
            //    }
            //    return (true);
            //}

            //private static void CompressFile(string directory, string path, GZipStream zipStream)
            //{
            //    //Compress file name
            //    char[] chars = path.ToCharArray();
            //    zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
            //    foreach (char c in chars)
            //        zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));
            //    //Compress file content
            //    byte[] bytes = File.ReadAllBytes(Path.Combine(directory, path));
            //    zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
            //    zipStream.Write(bytes, 0, bytes.Length);
            //}
        #endregion
    }
}
