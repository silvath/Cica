using Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindows
{
    public class QueryWindows : IQuery
    {
        #region IQuery
        string IQuery.Get(QueryType type)
        {
            return (ConfigurationManager.AppSettings[type.ToString()]);
        }
        #endregion
    }
}
