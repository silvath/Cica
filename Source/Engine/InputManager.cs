using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class InputManager
    {
        #region Properties
            public int BufferSize { set; get; }
            public List<List<InputType>> Buffer { set; get; }
            private List<InputType> Current { set; get; }
            private int Delay { set; get; }
            private int DelayReset { set; get; }
        #endregion
        #region Constructors
            public InputManager() 
            {
                this.BufferSize = 20;
                this.Buffer = new List<List<InputType>>();
                this.Current = new List<InputType>();
                this.Delay = 0;
                this.DelayReset = 60;
            }
        #endregion

        #region Add
            public void Add(InputType input) 
            {
                if(!this.Current.Contains(input))
                    this.Current.Add(input);
                this.Delay = 0;
            }
        #endregion
        #region Remove
            public void Remove(InputType input) 
            {
                if (this.Current.Contains(input))
                    this.Current.Remove(input);
                this.Delay = 0;
            }
        #endregion
        #region Snapshot
            public void Snapshot() 
            {
                this.Buffer.Add(new List<InputType>(this.Current));
                while (this.Buffer.Count > this.BufferSize)
                    this.Buffer.RemoveAt(0);
                if (this.Delay > 0)
                    this.Delay--;
            }
        #endregion

        #region Command
            public bool IsCommandMatch(Command command) 
            {
                if (this.Delay > 0)
                    return (false);
                foreach (Sequence sequence in command.Sequences)
                    if (IsSequenceMatch(sequence))
                        return (true);
                return (false);
            }

            public bool IsSequenceMatch(Sequence sequence)
            {
                //TODO: Just Simple for now
                if (sequence.Inputs.Count == 1) 
                {
                    foreach (InputType input in sequence.Inputs[0])
                        if (!this.Current.Contains(input))
                            return (false);
                    this.Delay = this.DelayReset;
                    return (true);
                }
                return (false);
            }
        #endregion
    }
}
