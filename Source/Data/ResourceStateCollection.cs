using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class ResourceStateCollection : List<ResourceState>
    {
        public string GetCode() 
        {
            int code = 0;
            while (this.Contains(code.ToString()))
                code++;
            return (code.ToString());
        }

        private bool Contains(string code) 
        {
            foreach (ResourceState resorceState in this)
                if (resorceState.Code == code)
                    return (true);
            return (false);
        }    
        
        public ResourceState Create(Resource resource, Spawn spawn, IPlayer player, string groupName) 
        {
            ResourceState resourceState = new ResourceState(this.GetCode());
            resourceState.Resource = resource;
            resourceState.Life = spawn.Life;
            resourceState.X = spawn.X;
            resourceState.Y = spawn.Y;
            resourceState.VelocityX = spawn.VelocityX;
            resourceState.VelocityY = spawn.VelocityY;
            resourceState.Player = player;
            resourceState.GroupName = groupName;
            this.Add(resourceState);
            return (resourceState);
        }

        public ResourceState Create(Resource resource, int life, int x, int y, int velocityX, int velocityY)
        {
            ResourceState resourceState = new ResourceState(this.GetCode());
            resourceState.Resource = resource;
            resourceState.Life = life;
            resourceState.X = x;
            resourceState.Y = y;
            resourceState.VelocityX = x;
            resourceState.VelocityY = y;
            resourceState.Player = null;
            resourceState.GroupName = null;
            this.Add(resourceState);
            return (resourceState);
        }

        public ResourceState Create(TriggerState triggerState, AnimationState animationState, IPlayer player)
        {
            ResourceState resourceState = new ResourceState(this.GetCode());
            resourceState.Resource = animationState.Resource;
            resourceState.Life = triggerState.Trigger.Life.Value;
            resourceState.X = triggerState.Trigger.X.Value;
            resourceState.Y = triggerState.Trigger.Y.Value;
            resourceState.VelocityX = triggerState.Trigger.VelocityX.Value;
            resourceState.VelocityY = triggerState.Trigger.VelocityY.Value;
            resourceState.AnimationState = animationState;
            resourceState.Player = player;
            this.Add(resourceState);
            return (resourceState);
        }

        public List<string> GetGroupsAlive() 
        {
            List<string> groups = new List<string>();
            foreach (ResourceState resourceState in this)
                if ((resourceState.Life > 0) && (!string.IsNullOrEmpty(resourceState.GroupName)) && (!groups.Contains(resourceState.GroupName)))
                    groups.Add(resourceState.GroupName);
            return (groups);
        }
    }
}
