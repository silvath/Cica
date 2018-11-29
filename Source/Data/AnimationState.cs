using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class AnimationState
    {
        #region Properties
            public Animation Animation { set; get; }
            public int FrameCurrent { set; get; }
            public int FrameCount { set; get; }
            public Resource ResourceRoot { set; get; }
            public Resource Resource { set; get; }
            public ResourceState ResourceState { set; get; }
            public TriggerState TriggerState { set; get; }
        #endregion
    }
}
