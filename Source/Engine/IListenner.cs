using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public interface IListenner
    {
        string Address { get; }
        int Port { get; }
        bool Start();
        bool Stop();
        INetwork Client();
    }
}
