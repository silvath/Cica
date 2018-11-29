using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceBuilderWindows
{
    public class ResourceGenerator
    {
        private int _incremental = 0;
        private string _prefix = string.Empty;
        public ResourceGenerator(string prefix) 
        {
            this._prefix = prefix;
        }

        public string GetCode() 
        {
            this._incremental++;
            return(string.Format("{0}{1}",this._prefix, this._incremental));
        }
    }
}
