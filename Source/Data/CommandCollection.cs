using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class CommandCollection : List<Command>
    {
        public Command Create(string name, string animationName, int velocityX, int velocityY) 
        {
            return (Create(name, animationName, velocityX, velocityY, string.Empty)); 
        }

        public Command Create(string name, string animationName, int velocityX, int velocityY, string animationNamePrerequisite) 
        {
            return (Create(name, animationName, velocityX, velocityY, animationNamePrerequisite, string.Empty, 0,0,0)); 
        }

        public Command Create(string name, string animationName, int velocityX, int velocityY, string animationNamePrerequisite, string factoryName, int life, int x, int y) 
        {
            Command command = new Command();
            command.Name = name;
            command.AnimationName = animationName;
            command.X = x;
            command.Y = y;
            command.VelocityX = velocityX;
            command.VelocityY = velocityY;
            command.AnimationNamePrerequisite = animationNamePrerequisite;
            command.FactoryName = factoryName;
            command.Life = life;
            this.Add(command);
            return (command);
        }

        public List<Command> GetCommands(string animationNamePrerequisite) 
        {
            List<Command> commands = new List<Command>();
            foreach(Command command in this)
                if ((string.IsNullOrEmpty(command.AnimationNamePrerequisite)) || (command.AnimationNamePrerequisite == animationNamePrerequisite))
                    commands.Add(command);
            return (commands);
        }

        public Command GetCommand(string commandName)
        {
            foreach (Command command in this)
                if (command.Name == commandName)
                    return (command);
            return(null);
        }

    }
}
