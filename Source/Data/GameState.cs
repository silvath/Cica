using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class GameState
    {
        #region Properties
            public Screen Screen { set; get; }
            public Resource Game { set; get; }
            public Resource Map { set; get; }
            public ResourceStateCollection ResourceStates { set; get; }
            public AnimationStateCollection AnimationStates { set; get; }
            public TriggerStateCollection TriggerStates { set; get; }
            public PropertyCollection Properties { set; get; }
            public TextCollection Texts { set; get; }
        #endregion
        #region Constructors
            public GameState() 
            {
                this.Reset(true);
            } 
        #endregion

        #region Reset
            public void Reset(bool includeProperties) 
            {
                this.ResourceStates = new ResourceStateCollection();
                this.AnimationStates = new AnimationStateCollection();
                this.TriggerStates = new TriggerStateCollection();
                if (includeProperties)
                    this.Properties = new PropertyCollection();
                this.Texts = new TextCollection();
            }
        #endregion
        #region Players
            public List<ResourceState> ExtractResourceStatePlayers() 
            {
                List<ResourceState> resourceStatePlayers = new List<ResourceState>();
                foreach (ResourceState resourceState in this.ResourceStates) 
                    if (resourceState.Player != null)
                        resourceStatePlayers.Add(resourceState);
                return (resourceStatePlayers);
            }

            public void DefineResourceStatePlayers(List<ResourceState> resourceStates)
            {
                foreach (ResourceState resourceState in this.ResourceStates) 
                {
                    if (resourceState.Player == null)
                        continue;
                    foreach(ResourceState resourceStatePrevious in resourceStates)
                    {
                        if ((resourceStatePrevious.GroupName != resourceState.GroupName) || (resourceStatePrevious.GroupIndex != resourceState.GroupIndex))
                            continue;
                        resourceState.Code = resourceStatePrevious.Code;
                        resourceState.Player = resourceStatePrevious.Player;
                        break;
                    }
                }
            }
        #endregion
        #region Clone
            public GameState Clone() 
            {
                GameState clone = new GameState();
                //Properties
                clone.Properties = this.Properties.Clone();
                //Texts
                clone.Texts = this.Texts.Clone();
                return (clone);
            }
        #endregion
    }
}
