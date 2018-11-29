using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public enum PackageType
    {
        Error,
        Script,
        RequestRegisterServer,
        ReponseRegisterServer,
        RequestServers,
        ReponseServers,
        RequestSlots,
        ResponseSlots,
        RequestGameJoin,
        ResponseGameJoin,
        RequestGameStart,
        ResponseGameStart,
        RequestGameState,
        ResponseGameState,
        RequestCommand,
        PackageDiff
    }
}
