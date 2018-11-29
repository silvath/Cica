using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface IPlayer
    {
        string GetName();
        bool IsHuman();
        Command GetCommand(GameState gameState, ResourceState resourceState);
    }
}
