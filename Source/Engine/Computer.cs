using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Computer : IPlayer
    {
        private Random _random = new Random(DateTime.Now.Millisecond);
        private int _commandNext = 0;
        Command IPlayer.GetCommand(GameState gameState, ResourceState resourceState)
        {
            if (this._commandNext > 0) 
            {
                this._commandNext--;
                return (null);
            }
            this._commandNext = this._random.Next(10, 30);
            List<Command> commands = resourceState.Resource.Commands.GetCommands(resourceState.AnimationState.Animation.Name);
            int index = this._random.Next(0, commands.Count + 1);
            if (index >= commands.Count)
                return (null);
            return (commands[index]);
        }

        bool IPlayer.IsHuman()
        {
            return (false);
        }

        string IPlayer.GetName() 
        {
            return ("CPU");
        }
    }
}
