using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class PackageManager
    {
        #region Attributes
            private GameState _gameStateLast = null;
            private List<byte> _dataPackage = null;
        #endregion
        #region Constructors
            public PackageManager() 
            {
            }
        #endregion

        #region Create
            public Package CreateError(string message) 
            {
                return (Create(PackageType.Error, PackageItemType.Message, message)); 
            }

            public Package CreateRequestServers(string version, string game) 
            {
                Package package = new Package();
                package.Type = PackageType.RequestServers;
                PackageItem packageItem = new PackageItem();
                packageItem.Type = PackageItemType.Version;
                packageItem.Data.Add(version);
                package.Items.Add(packageItem);
                packageItem = new PackageItem();
                packageItem.Type = PackageItemType.Game;
                packageItem.Data.Add(game);
                package.Items.Add(packageItem);
                return (package);
            } 

            public Package Create(PackageType type) 
            {
                Package package = new Package();
                package.Type = type;
                return (package);
            } 

            public Package Create(PackageType type, PackageItemType itemType, params string[] parameters) 
            {
                Package package = new Package();
                package.Type = type;
                PackageItem packageItem = new PackageItem();
                packageItem.Type = itemType;
                foreach (string parameter in parameters)
                    packageItem.Data.Add(parameter);
                package.Items.Add(packageItem);
                return (package);
            }

            public Package Create(List<byte> buffer) 
            {
                //Minimum Size
                if (buffer.Count < 5)
                    return (null);
                //Size
                int size = GetInt(buffer, 0);
                if (buffer.Count < size)
                    return (null);
                //Package
                Package package = new Package();
                package.Type = (PackageType)buffer[4];
                if (package.Type == PackageType.PackageDiff)
                {
                    if (_dataPackage != null) 
                    {
                        int offset = 0;
                        for (int i = 5; i < size; i = i + 2) 
                        {
                            offset += GetIntByte(buffer, i);
                            _dataPackage[offset] = buffer[i + 1];
                        }
                        package = Create(this._dataPackage);
                    }
                }else{
                    //Package Items
                    int offset = 5;
                    while (offset < size)
                        offset += this.CreatePackageItem(buffer, offset, package);
                    _dataPackage = buffer.GetRange(0, size);
                }
                //Buffer extract
                buffer.RemoveRange(0, size);
                return (package);
            }

            private int CreatePackageItem(List<byte> buffer, int offset, Package package) 
            {
                //Size
                int size = GetInt(buffer, offset);
                PackageItem packageItem = new PackageItem();
                packageItem.Type = (PackageItemType)buffer[offset + 4];
                //Items
                int offsetItem = offset + 5;
                while (offsetItem < (offset + size)) 
                {
                    int sizeItem = this.GetIntByte(buffer, offsetItem);
                    offsetItem += 1;
                    packageItem.Data.Add(this.GetString(buffer, offsetItem, sizeItem));
                    offsetItem += sizeItem;
                }
                package.Items.Add(packageItem);
                return (size);
            }

            public PackageItem CreatePackageItem(ServerState server) 
            {
                PackageItem packageItem = new PackageItem();
                packageItem.Type = PackageItemType.Server;
                packageItem.Data.Add(server.Version);
                packageItem.Data.Add(server.Game);
                packageItem.Data.Add(server.Name);
                packageItem.Data.Add(server.Address);
                packageItem.Data.Add(server.Port);
                return (packageItem);
            }

            public ServerState CreateServerState(PackageItem packageItem) 
            {
                ServerState server = new ServerState();
                server.Version = packageItem.Data[0];
                server.Game = packageItem.Data[1];
                server.Name = packageItem.Data[2];
                server.Address = packageItem.Data[3];
                server.Port = packageItem.Data[4];
                return (server);
            }
        #endregion
        #region GameState
            public Package CreateGameState(GameState gameState)
            {
                Package package = this.Create(PackageType.ResponseGameState);
                //GameState
                package.Items.Add(this.CreatePackageItem(gameState));
                //Animations
                foreach (AnimationState animationState in gameState.AnimationStates)
                    package.Items.Add(this.CreatePackageItem(animationState));
                //Properties
                if ((this._gameStateLast == null) || (!this._gameStateLast.Properties.IsEqual(gameState.Properties)))
                {
                    package.Items.Add(this.CreatePackageItemPropertyClear());
                    foreach (Property property in gameState.Properties)
                        package.Items.Add(this.CreatePackageItem(property));
                }
                //Texts
                if ((this._gameStateLast == null) || (!this._gameStateLast.Texts.IsEqual(gameState.Texts)))
                {
                    package.Items.Add(this.CreatePackageItemTextClear());
                    foreach (Text text in gameState.Texts)
                        package.Items.Add(this.CreatePackageItem(text));
                }
                this._gameStateLast = gameState.Clone();
                return (package);
            }

            private PackageItem CreatePackageItem(GameState gameState) 
            {
                PackageItem packateItem = new PackageItem();
                packateItem.Type = PackageItemType.GameState;
                //Screen
                packateItem.Data.Add(gameState.Screen.Name);
                return (packateItem);
            }

            private PackageItem CreatePackageItem(AnimationState animationState)
            {
                PackageItem packateItem = new PackageItem();
                packateItem.Type = PackageItemType.AnimationState;
                //Resource Root
                packateItem.Data.Add(animationState.ResourceRoot.Code.ToString());
                //Resource
                packateItem.Data.Add(animationState.Resource.Code.ToString());
                //Animation
                packateItem.Data.Add(animationState.Resource.Animations.IndexOf(animationState.Animation).ToString());
                //FrameCurrent
                packateItem.Data.Add(animationState.FrameCurrent.ToString());
                //FrameCount
                packateItem.Data.Add(animationState.FrameCount.ToString());
                //Code
                packateItem.Data.Add(animationState.ResourceState != null ? animationState.ResourceState.Code : string.Empty);
                //X
                packateItem.Data.Add(animationState.ResourceState != null ? animationState.ResourceState.X.ToString() : "0");
                //Y
                packateItem.Data.Add(animationState.ResourceState != null ? animationState.ResourceState.Y.ToString() : "0");
                return (packateItem);

            }

            private PackageItem CreatePackageItemPropertyClear() 
            {
                PackageItem packateItem = new PackageItem();
                packateItem.Type = PackageItemType.PropertyClear;
                return (packateItem);
            }

            private PackageItem CreatePackageItem(Property property) 
            {
                PackageItem packateItem = new PackageItem();
                packateItem.Type = PackageItemType.Property;
                //AttributeName
                packateItem.Data.Add(property.AttributeName ?? string.Empty);
                //PropertyName
                packateItem.Data.Add(property.PropertyName ?? string.Empty);
                //Value
                packateItem.Data.Add(property.Value ?? string.Empty);
                return (packateItem);
            }

            private PackageItem CreatePackageItemTextClear() 
            {
                PackageItem packateItem = new PackageItem();
                packateItem.Type = PackageItemType.TextClear;
                return (packateItem);
            }

            private PackageItem CreatePackageItem(Text text)
            {
                PackageItem packateItem = new PackageItem();
                packateItem.Type = PackageItemType.Text;
                //FontName
                packateItem.Data.Add(text.FontName ?? string.Empty);
                //Value
                packateItem.Data.Add(text.Value ?? string.Empty);
                //X
                packateItem.Data.Add(text.X.ToString());
                //Y
                packateItem.Data.Add(text.Y.ToString());
                //Align
                packateItem.Data.Add(((int)text.Align).ToString());
                return (packateItem);
            }

            public GameState CreateGameState(Resource game, Package package, ResourceManager resources, out bool clearProperties, out bool clearTexts) 
            {
                clearProperties = clearTexts = false;
                GameState gameState = new GameState();
                foreach (PackageItem item in package.Items) 
                {
                    if (item.Type == PackageItemType.AnimationState)
                        CreateGameStateAnimationState(gameState, resources, item);
                    else if (item.Type == PackageItemType.PropertyClear)
                        clearProperties = true;
                    else if (item.Type == PackageItemType.Property)
                        CreateGameStateProperty(gameState, item);
                    else if (item.Type == PackageItemType.TextClear)
                        clearTexts = true;
                    else if (item.Type == PackageItemType.Text)
                        CreateGameStateText(gameState, item);
                    else if (item.Type == PackageItemType.GameState)
                        UpdateGameState(game, gameState, resources, item); 
                }  
                return (gameState);
            }

            private void UpdateGameState(Resource game, GameState gameState, ResourceManager resources, PackageItem item) 
            {
                //Screen
                gameState.Screen = game.Screens.GetScreen(item.Data[0]);
            }

            private void CreateGameStateAnimationState(GameState gameState, ResourceManager resources, PackageItem item) 
            {
                AnimationState animationState = new AnimationState();
                //Resource Root
                animationState.ResourceRoot = resources.GetResource(item.Data[0]);
                //Resource
                animationState.Resource = item.Data[0] != item.Data[1] ? animationState.ResourceRoot.Resources.GetResourceByCode(item.Data[1]) : animationState.ResourceRoot;
                //Animation
                animationState.Animation = animationState.Resource.Animations[Int32.Parse(item.Data[2])];
                //FrameCurrent
                animationState.FrameCurrent = Int32.Parse(item.Data[3]);
                //FrameCount
                animationState.FrameCount = Int32.Parse(item.Data[4]);
                //ResourceState
                ResourceState resourceState = new ResourceState(item.Data[5]);
                resourceState.X = Int32.Parse(item.Data[6]);
                resourceState.Y = Int32.Parse(item.Data[7]);
                animationState.ResourceState = resourceState;
                resourceState.AnimationState = animationState;
                resourceState.Resource = animationState.Resource;
                gameState.AnimationStates.Add(animationState);
            }

            private void CreateGameStateProperty(GameState gameState, PackageItem item)
            {
                Property property = new Property();
                //AttributeName
                property.AttributeName = item.Data[0];
                //PropertyName
                property.PropertyName = item.Data[1];
                //Value
                property.Value = item.Data[2];
                gameState.Properties.Add(property);
            }

            private void CreateGameStateText(GameState gameState, PackageItem item)
            {
                Text text = new Text();
                //FontName
                text.FontName = item.Data[0];
                //Value
                text.Value = item.Data[1];
                //X
                text.X = Int32.Parse(item.Data[2]);
                //Y
                text.Y = Int32.Parse(item.Data[3]);
                //Align
                text.Align = (TextAlign)Int32.Parse(item.Data[4]);
                gameState.Texts.Add(text);
            }

            public bool Update(AnimationState animationStateBase, AnimationState animationState)
            {
                bool updated = false;
                //Resource Root
                if (animationStateBase.ResourceRoot.Code != animationState.ResourceRoot.Code)
                {
                    animationStateBase.ResourceRoot = animationState.ResourceRoot;
                    updated = true;
                }
                //Resource
                if (animationStateBase.Resource.Code != animationState.Resource.Code)
                {
                    animationStateBase.Resource = animationState.Resource;
                    updated = true;
                }
                //Animation
                if (animationStateBase.Animation != animationState.Animation)
                {
                    animationStateBase.Animation = animationState.Animation;
                    updated = true;
                }
                //FrameCurrent
                if (animationStateBase.FrameCurrent != animationState.FrameCurrent)
                {
                    animationStateBase.FrameCurrent = animationState.FrameCurrent;
                    updated = true;
                }
                //FrameCount
                if (animationStateBase.FrameCount != animationState.FrameCount)
                {
                    animationStateBase.FrameCount = animationState.FrameCount;
                    updated = true;
                }
                //ResourceState
                if (animationStateBase.ResourceState != animationState.ResourceState)
                {
                    animationStateBase.ResourceState = animationState.ResourceState;
                    updated = true;
                }
                else if (animationStateBase.ResourceState != null)
                {
                    if (animationStateBase.ResourceState.Code != animationState.ResourceState.Code)
                    {
                        animationStateBase.ResourceState.Code = animationState.ResourceState.Code;
                        updated = true;
                    }
                    if (animationStateBase.ResourceState.X != animationState.ResourceState.X)
                    {
                        animationStateBase.ResourceState.X = animationState.ResourceState.X;
                        updated = true;
                    }
                    if (animationStateBase.ResourceState.Y != animationState.ResourceState.Y)
                    {
                        animationStateBase.ResourceState.Y = animationState.ResourceState.Y;
                        updated = true;
                    }
                }
                return (updated);
            }

            public bool Update(Property propertyBase, Property property)
            {
                bool updated = false;
                //Attribute Name
                if (propertyBase.AttributeName != property.AttributeName)
                {
                    propertyBase.AttributeName = property.AttributeName;
                    updated = true;
                }
                //Property Name
                if (propertyBase.PropertyName != property.PropertyName)
                {
                    propertyBase.PropertyName = property.PropertyName;
                    updated = true;
                }
                //Value
                if (propertyBase.Value != property.Value)
                {
                    propertyBase.Value = property.Value;
                    updated = true;
                }
                return (updated);
            }
        #endregion

        #region Bytes
            public List<byte> GetBytes(Package package) 
            {
                List<List<byte>> items = new List<List<byte>>();
                int length = 5;
                foreach(PackageItem item in package.Items)
                {
                    List<byte> itemData = GetBytes(item);
                    items.Add(itemData);
                    length += itemData.Count;
                }
                List<byte> bytes = new List<byte>();
                //Size
                bytes.AddRange(GetBytes(length));
                //Type
                bytes.Add((byte)package.Type);
                //Children
                foreach (List<byte> item in items)
                    bytes.AddRange(item);
                return (bytes);
            }

            private List<byte> GetBytes(PackageItem packageItem) 
            {
                List<List<byte>> items = new List<List<byte>>();
                int length = 5;
                foreach (string data in packageItem.Data)
                {
                    List<byte> itemData = GetBytes(data);
                    items.Add(itemData);
                    length += itemData.Count + 1;
                }
                List<byte> bytes = new List<byte>();
                //Size
                bytes.AddRange(GetBytes(length));
                //Type
                bytes.Add((byte)packageItem.Type);
                //Children
                foreach (List<byte> item in items)
                {
                    bytes.Add(GetByte(item.Count));
                    bytes.AddRange(item);
                }
                return (bytes);
            }

            public List<byte> GetBytes(int value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return (new List<byte>(bytes));
            }

            public byte GetByte(int value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return (bytes[3]);
            }

            private List<byte> GetBytes(string data)
            {
                List<byte> bytes = new List<byte>();
                foreach (char character in data)
                    bytes.Add((byte)character);
                return (bytes);
            }

            private int GetInt(List<byte> data, int offset)
            {
                byte[] bytes = { data[offset], data[offset + 1], data[offset + 2], data[offset + 3] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return (BitConverter.ToInt32(bytes, 0));
            }

            private int GetIntByte(List<byte> data, int offset) 
            {
                byte[] bytes = { 0, 0, 0, data[offset]};
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return (BitConverter.ToInt32(bytes, 0));
            }

            private string GetString(List<byte> data, int offset, int length)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = offset; i < offset + length; i++)
                    builder.Append((char)data[i]);
                return (builder.ToString());
            }
        #endregion
    }
}
