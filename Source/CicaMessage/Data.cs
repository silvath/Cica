using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaMessage
{
    public class Data
    {
        #region Properties
            private int Snapshot { set; get; }
            public List<DataItem> Items { set; get; }
            public int Size 
            {
                get 
                {
                    int size = 0;
                    foreach (DataItem item in this.Items)
                        size += item.Size;
                    return (size);
                }
            }
        #endregion
        #region Constructors
            public Data() 
            {
                this.Items = new List<DataItem>();
            }
        #endregion

        #region Bytes
            public byte[] GetBytes() 
            {
                List<byte> bytes = new List<byte>();
                //Snapshot
                bytes.AddRange(GetBytes((int)this.Snapshot));
                //Size
                bytes.AddRange(GetBytes(this.Size));
                //Items
                foreach (DataItem item in this.Items)
                    bytes.AddRange(item.GetBytes());
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
            public static Data Create(List<byte> buffer, int offset, out int length)
            {
                length = 0;
                if (buffer.Count < (offset + 8))
                    return (null);
                int snapshot = GetInt(buffer, offset);
                int size = GetInt(buffer, offset + 4);
                if (buffer.Count < (offset + 8 + size))
                    return (null);
                Data data = new Data();
                //Snapshot
                data.Snapshot = snapshot;
                //Items
                length = offset + 8 + size;
                int offsetItem = offset + 8;
                int lengthItem = 0;
                while (offsetItem < length) 
                {
                    data.Items.Add(DataItem.Create(buffer, offsetItem, out lengthItem));
                    offsetItem += lengthItem;
                }
                return (data);
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
    }
}
