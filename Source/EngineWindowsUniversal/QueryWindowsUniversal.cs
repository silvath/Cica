﻿using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindowsUniversal
{
    public class QueryWindowsUniversal : IQuery
    {
        #region IQuery
        string IQuery.Get(QueryType type)
        {
            string value = string.Empty;
            switch (type)
            {
                case QueryType.PlayerCode:
                    break;
                case QueryType.PlayerName:
                    value = "Xbox User";
                    break;
                case QueryType.PlayerPassword:
                    break;
                case QueryType.MasterHost:
                    value = "cica.cloudapp.net";
                    break;
                case QueryType.MasterPort:
                    value = "21000";
                    break;
            }
            return (value);
        }
        #endregion
    }
}
