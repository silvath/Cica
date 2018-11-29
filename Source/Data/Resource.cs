using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Resource
    {
        #region Properties
            [DataMember]
            public string Code { set; get; }
            [DataMember]
            public ResourceType Type { set; get; }
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public int FramesPerSecond { set; get; }
            [DataMember]
            public int InputsPerSecond { set; get; }
            [DataMember]
            public int NetworksPerSecond { set; get; }
            [DataMember]
            public int Width { set; get; }
            [DataMember]
            public int Height { set; get; }
            [DataMember]
            public ImageCollection Images { set; get; }
            [DataMember]
            public SpriteMapCollection SpritesMaps { set; get; }
            [DataMember]
            public ResourceCollection Resources { set; get; }
            [DataMember]
            public AnimationCollection Animations { set; get; }
            [DataMember]
            public TriggerCollection Triggers { set; get; }
            [DataMember]
            public SpawnCollection Spawns { set; get; }
            [DataMember]
            public CommandCollection Commands { set; get; }
            [DataMember]
            public ScreenCollection Screens { set; get; }
            [DataMember]
            public FontCollection Fonts { set; get; }
            [DataMember]
            public int WarnUp { set; get; }
            [DataMember]
            public string WarnUpFontName { set; get; }
            [DataMember]
            public string SystemFontName { set; get; }
            [DataMember]
            public int Rounds { set; get; }
            public string StorageID 
            {
                get 
                {
                    return (string.Format("{0}.{1}", this.Name, (int)this.Type));
                }
            }
        #endregion
        #region Constructors
            public Resource(Resource parent) 
            {
                this.Images = new ImageCollection();
                this.SpritesMaps = new SpriteMapCollection();
                this.Resources = new ResourceCollection();
                this.Animations = new AnimationCollection();
                this.Triggers = new TriggerCollection();
                this.Spawns = new SpawnCollection();
                this.Commands = new CommandCollection();
                this.Screens = new ScreenCollection();
                this.Fonts = new FontCollection();
                this.FramesPerSecond = 30;
            }
        #endregion

        #region Create
            public Resource Create(string code, string name)
            {
                return (this.Resources.Create(this, ResourceType.Internal, code, name));
            }
        #endregion
    }
}
