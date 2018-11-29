using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class TriggerStateCollection : List<TriggerState>
    {
        public TriggerState Create(Trigger trigger, Resource resourceRoot, Resource resource) 
        {
            TriggerState triggerState = new TriggerState();
            triggerState.Trigger = trigger;
            triggerState.Activation = trigger.Activation.Value;
            triggerState.ResourceRoot = resourceRoot;
            triggerState.Resource = resource;
            this.Add(triggerState);
            return (triggerState);
        }
    }
}
