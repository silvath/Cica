using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class Package
    {
        #region Properties
            public PackageType Type { set; get; }
            public List<PackageItem> Items { set; get; }
        #endregion
        #region Constructors
            public Package() 
            {
                this.Items = new List<PackageItem>();
            }
        #endregion
    }
}
