using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class PlayerNetwork : IPlayer
    {
        #region Attributes
            private string _name;
        #endregion
        #region Properties
            public string CommandName { set; get; }
        #endregion
        #region Constructors
            public PlayerNetwork(string name) 
            {
                this._name = name;
            }
        #endregion

        #region Command
            string IPlayer.GetName() 
            {
                return (this._name);
            }

            bool IPlayer.IsHuman() 
            {
                return (true);
            }

            Command IPlayer.GetCommand(GameState gameState, ResourceState resourceState)
            {
                if (string.IsNullOrEmpty(this.CommandName))
                    return (null);
                string commandName = this.CommandName;
                this.CommandName = null;
                List<Command> commands = resourceState.Resource.Commands.GetCommands(resourceState.AnimationState.Animation.Name);
                foreach (Command command in commands)
                if (command.Name == commandName)
                    return (command);
                return (null);
            }
        #endregion
    }
}
