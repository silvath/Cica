using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class QueryManager
    {
        #region Attributes
            private IQuery _query = null;
        #endregion
        #region Properties
        #endregion
        #region Constructor
            public QueryManager(IQuery query) 
            {
                this._query = query;
            } 
        #endregion

        #region Get
            public string Get(QueryType type) 
            {
                return (this._query.Get(type));
            }
        #endregion
    }
}
