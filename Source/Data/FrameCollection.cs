using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class FrameCollection : List<Frame>
    {
        public Frame Create(string spriteMapName) 
        {
            return(Create(spriteMapName, 1));
        }

        public Frame Create(string spriteMapName, int frames) 
        {
            return (Create(spriteMapName, frames, 0, 0));
        }

        public Frame Create(string spriteMapName, int frames, int x, int y)
        {
            Frame frame = new Frame();
            frame.SpriteMapName = spriteMapName;
            frame.Frames = frames;
            frame.X = x;
            frame.Y = y;
            this.Add(frame);
            return (frame);
        }
    }
}
