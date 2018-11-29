using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class AnimationCollection : List<Animation>
    {
        public Animation Create(string name) 
        {
            return (this.Create(name, true));
        } 

        public Animation Create(string name, bool loop) 
        {
            Animation animation = new Animation();
            animation.Name = name;
            animation.Loop = loop;
            this.Add(animation);
            return (animation);
        }

        public Animation GetAnimation(string name) 
        {
            foreach (Animation animation in this)
                if (animation.Name == name)
                    return (animation);
            return (null);
        }
    }
}
