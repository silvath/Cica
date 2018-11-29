using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class TouchManager
    {
        #region Properties
            private InputManager Inputs{set;get;}
            private List<Touch> _touches = new List<Touch>();
        #endregion
        #region Constructors
            public TouchManager(InputManager inputs) 
            {
                this.Inputs = inputs;
            }
        #endregion

        #region Clear
            public void Clear() 
            {
                for (int i = this._touches.Count - 1; i >= 0; i--)
                    this.Remove(this._touches[i]);
            }
        #endregion
        #region Add
            public void Add(Touch touch) 
            {
                this._touches.Add(touch);
                this.Inputs.Add(touch.TouchButton.Input);
            }
        #endregion
        #region Remove
            public void Remove(int touchCode) 
            {
                Touch touch = this.GetTouch(touchCode);
                if (touch != null)
                    this.Remove(touch);
            }

            private void Remove(Touch touch)
            {
                this.Inputs.Remove(touch.TouchButton.Input);
                this._touches.Remove(touch);
            }
        #endregion
        #region Get
            public Touch GetTouch(int touchCode) 
            {
                foreach (Touch touch in this._touches)
                    if ((touch.Code.HasValue) && (touch.Code.Value == touchCode))
                        return (touch);
                return (null);
            }
        #endregion
    }
}
