using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class TriggerCollection : List<Trigger>
    {
        public Trigger CreateAnimation(string animationName, IntRandom activation)
        {
            return (Create(TriggerType.Animation,string.Empty, animationName, activation, null,null, null, null,null));
        }

        public Trigger CreateFactory(string animationName, IntRandom activation, IntRandom life, IntRandom x, IntRandom y, IntRandom velocityX, IntRandom velocityY) 
        {
            return (Create(TriggerType.Factory, string.Empty, animationName, activation, life, x, y, velocityX, velocityY));
        }

        public Trigger Create(TriggerType type, string resourceName, string animationName, IntRandom activation, IntRandom life, IntRandom x, IntRandom y, IntRandom velocityX, IntRandom velocityY)
        {
            Trigger trigger = new Trigger();
            trigger.Type = type;
            trigger.ResourceName = resourceName;
            trigger.AnimationName = animationName;
            trigger.Activation = activation;
            trigger.Life = life;
            trigger.X = x;
            trigger.Y = y;
            trigger.VelocityX = velocityX;
            trigger.VelocityY = velocityY;
            this.Add(trigger);
            return (trigger);
        }
    }
}
