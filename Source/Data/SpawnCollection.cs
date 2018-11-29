using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class SpawnCollection :  List<Spawn>
    {
        public Spawn Create(int life, int x, int y, int velocityX, int velocityY, string animationName, string groupName) 
        {
            Spawn spawn = new Spawn();
            spawn.Life = life;
            spawn.X = x;
            spawn.Y = y;
            spawn.VelocityX = velocityX;
            spawn.VelocityY = velocityY;
            spawn.Y = y;
            spawn.AnimationName = animationName;
            spawn.GroupName = groupName;
            this.Add(spawn);
            return (spawn);
        }
    }
}
