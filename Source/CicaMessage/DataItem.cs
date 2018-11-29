using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaMessage
{
    public class DataItem
    {
        #region Properties
            public DataItemType Type { set; get; }  
            public int Size
            {
                get
                {
                    return (GetSize(this.Type));
                }
            }
        #endregion

        #region Size
            private static int GetSize(DataItemType type) 
            {
                //TODO: Work over here
                return(12);
            }
        #endregion
        #region Bytes
            public byte[] GetBytes()
            {
                //TODO: Work over here
                List<byte> bytes = new List<byte>();
                bytes.AddRange(GetBytes((int)this.Type));
                bytes.AddRange(GetBytes((int)this.Type));
                bytes.AddRange(GetBytes((int)this.Type));
                return (bytes.ToArray());
            }

            private byte[] GetBytes(int value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                return (bytes);
            }
        #endregion
        #region Create
            public static DataItem Create(List<byte> buffer, int offset, out int length)
            {
                //TODO: Work over here
                length = 0;
                //Type
                if (buffer.Count < (offset + 4))
                    return (null);
                DataItemType type = (DataItemType)GetInt(buffer, offset);
                int size = (offset + 4 + GetSize(type));
                if (buffer.Count < size)
                    return (null);
                DataItem dataItem = new DataItem();
                dataItem.Type = type;
                length = size;
                return (dataItem);
            }

            private static int GetInt(List<byte> data, int offset)
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
    }
}
