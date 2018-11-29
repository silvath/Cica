using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public interface IQuery
    {
        string Get(QueryType type);
    }
}
