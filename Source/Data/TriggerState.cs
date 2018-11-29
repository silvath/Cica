using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class TriggerState
    {
        #region Properties
            public Trigger Trigger { set; get; }
            public int Activation = Int32.MaxValue;
            public Resource ResourceRoot { set; get; }
            public Resource Resource { set; get; }
            public AnimationState AnimationState { set; get; }
        #endregion

        #region Reset
            public void Reset() 
            {
                this.Activation = this.Trigger.Activation.Value;
                this.AnimationState = null;
            }
        #endregion
    }
}
