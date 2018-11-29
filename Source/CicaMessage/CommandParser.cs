using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaMessage
{
    public class CommandParser
    {
        public static Command Parse(string message)
        {
            if (string.IsNullOrEmpty(message))
                return (null);
            string[] messages = message.Split(' ');
            if(messages.Length == 0)
                return (null);
            CommandType type = GetParseCommandType(messages[0]);
            if (type == CommandType.Unknown)
                return (null);
            Command command = new Command(type);
            for (int i = 1; i < messages.Length; i++)
                command.Parameters.Add(messages[i]);
            return (command);

            if (message.ToUpper() == "CONNECT")
                return (new Command(CommandType.Connect));
            else if (message.ToUpper() == "DISCONNECT")
                return (new Command(CommandType.Disconnect));
            else if (message.ToUpper() == "CLOSE")
                return (new Command(CommandType.Close));
            return (null);
        }

        private static CommandType GetParseCommandType(string message)
        {
            CommandType command;
            if (Enum.TryParse<CommandType>(message, true, out command))
                return (command);
            return (CommandType.Unknown);
        }
    }
}
