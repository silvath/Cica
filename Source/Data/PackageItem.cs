using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class PackageItem
    {
        #region Properties
            public PackageItemType Type { set; get; }
            public List<string> Data { set; get; }
        #endregion
        #region Constructors
            public PackageItem() 
            {
                this.Data = new List<string>();
            }
        #endregion
    }
}
