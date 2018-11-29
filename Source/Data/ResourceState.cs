using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class ResourceState
    {
        #region Properties
            public string Code { set; get; }
            public Resource Resource { set; get; }
            public AnimationState AnimationState { set; get; } 
            public int Life { set; get; }
            public int X { set; get; }
            public int Y { set; get; }
            public int VelocityX {set; get;}
            public int VelocityY { set; get; }
            public IPlayer Player { set; get; }
            public string GroupName { set; get; }
            public int GroupIndex { set; get; }
            public bool IsPlayer
            {
                get
                {
                    return (!string.IsNullOrEmpty(this.GroupName));
                }
            }
        #endregion
        #region Constructors
            public ResourceState(string code)
            {
                this.Code = code;
            }
        #endregion
    }
}
