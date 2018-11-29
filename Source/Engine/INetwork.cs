using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public interface INetwork
    {
        bool IsConnected();
        bool Connect(string server, int port);
        void Disconnect();
        void Send(byte[] data);
        byte[] Receive();
    }
}
