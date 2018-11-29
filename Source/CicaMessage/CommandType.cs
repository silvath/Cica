using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaMessage
{
    public enum CommandType
    {
        Unknown,
        Error,
        Close,
        Connect,
        ConnectedCommand,
        ConnectedData,
        Disconnect,
        Talk,
        SessionCreate,
        SessionCreated,
        SessionDestroy,
        SessionList,
        SessionListed,
        SessionStart,
        SessionStarted,
        SessionStop,
        SessionJoin,
        SessionJoined,
        SessionLeave,
        SessionUsers,
        SessionResourcesList,
        SessionResourcesListed,
        ResourceSynchronize,
        ResourceSynchronized,
        ResourceRetrieve,
        ResourceRetrieved,
        DataConnect,
        DataConnected
    }
}
