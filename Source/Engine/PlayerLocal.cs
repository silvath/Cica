using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class PlayerLocal : IPlayer
    {
        #region Properties
            private InputManager Inputs { set; get; }
        #endregion
        #region Constructors
            public PlayerLocal(InputManager inputs) 
            {
                this.Inputs = inputs;
            }
        #endregion

        #region Command
            string IPlayer.GetName() 
            {
                return ("YOU");
            }

            bool IPlayer.IsHuman() 
            {
                return (true);
            }

            Command IPlayer.GetCommand(GameState gameState, ResourceState resourceState)
            {
                List<Command> commands = resourceState.Resource.Commands.GetCommands(resourceState.AnimationState.Animation.Name);
                foreach (Command command in commands)
                    if (this.Inputs.IsCommandMatch(command))
                        return (command);
                return (null);
            }
        #endregion
    }
}
