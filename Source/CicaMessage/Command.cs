using Cica.CicaResource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaMessage
{
    public class Command
    {
        #region Properties
            public CommandType Type { set; get; }
            public List<string> Parameters { set; get; }
        #endregion
        #region Constructors
            public Command() 
            {
                this.Parameters = new List<string>();
            }

            public Command(CommandType type)
            {
                this.Type = type;
                this.Parameters = new List<string>();
            }
        #endregion

        #region Bytes
            public byte[] GetBytes() 
            {
                List<byte> bytes = new List<byte>();
                //Type
                bytes.AddRange(GetBytes((int)this.Type));
                //Count
                bytes.AddRange(GetBytes(this.Parameters.Count));
                //Sizes
                foreach(string parameter in this.Parameters)
                    bytes.AddRange(GetBytes(parameter.Length));
                //Data
                foreach (string parameter in this.Parameters)
                    bytes.AddRange(GetBytes(parameter));
                return (bytes.ToArray());
            }

            public static byte[] GetBytes(int value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return(bytes);
            }

            private byte[] GetBytes(string data) 
            {
                List<byte> bytes = new List<byte>();
                foreach (char character in data)
                    bytes.Add((byte)character);
                return (bytes.ToArray());
            }
        #endregion
        #region Create
            public static Command Create(List<byte> data, int offset, out int length) 
            {
                length = 0;
                //Type + Count
                if (data.Count < (offset + 8))
                    return (null);
                int count = GetInt(data, offset + 4);
                //Sizes
                if (data.Count < ((offset + 8) + (count * 4)))
                    return (null);
                int sizesSum = 0;
                List<int> sizes = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    int size = GetInt(data, ((offset + 8) + (i * 4)));
                    sizes.Add(size);
                    sizesSum += size;
                }
                if (data.Count < (8 + (count * 4) + sizesSum))
                    return(null);
                Command command = new Command((CommandType)GetInt(data, offset));
                //Parameters
                int parameterOffset = (offset + 8) + (count * 4);
                foreach(int size in sizes)
                {
                    command.Parameters.Add(GetString(data, parameterOffset, size));
                    parameterOffset += size;
                }
                length = 8 + (count * 4) + sizesSum;
                return (command);
            }

            public static int GetInt(List<byte> data, int offset)
            {
                byte[] bytes = { data[offset], data[offset + 1], data[offset + 2], data[offset + 3] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return (BitConverter.ToInt32(bytes, 0));
            }

            private static string GetString(List<byte> data, int offset, int length) 
            {
                StringBuilder builder = new StringBuilder();
                for (int i = offset; i < offset + length; i++)
                    builder.Append((char)data[i]);
                return (builder.ToString());
            }
        #endregion
        #region Parameters
            private bool IsParameterResource() 
            {
                return ((this.Parameters.Count > 0) && (Resource.GetResourceTypeByFileName(this.Parameters[0]) != ResourceType.Unknown));
            }

            public void LoadParametersResources(string resourcesPath) 
            {
                if (!this.IsParameterResource())
                    return;
                string file = string.Format("{0}{1}", resourcesPath, this.Parameters[0]);
                StringBuilder builder = new StringBuilder();
                foreach (byte data in File.ReadAllBytes(file))
                    builder.Append((char)data);
                this.Parameters.Add(builder.ToString());
            }

            public void SaveParametersResources(string resourcesPath)
            {
                if (!this.IsParameterResource())
                    return;
                string file = string.Format("{0}{1}", resourcesPath, this.Parameters[0]);
                List<byte> bytes = new List<byte>();
                foreach (char data in this.Parameters[1])
                    bytes.Add((byte)data);
                File.WriteAllBytes(file, bytes.ToArray());
            }
        #endregion

        #region ToString
            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.Type.ToString());
                for (int i = 0; i < this.Parameters.Count; i++) 
                {
                    string parameter = this.Parameters[i];
                    builder.AppendFormat(" {0}", parameter);
                    if (this.IsParameterResource())
                        break;
                }
                return (builder.ToString());
            }
        #endregion
    }
}
