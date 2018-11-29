using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class Touch
    {
        #region Properties
            public int? Code { set; get; }
            public int X { set; get; }
            public int Y { set; get; }
            public TouchButton TouchButton { set; get; }
        #endregion
        #region Constructors
            public Touch(int? code, int x, int y, TouchButton touchButton) 
            {
                this.Code = code;
                this.X = x;
                this.Y = y;
                this.TouchButton = touchButton;
            }
        #endregion
    }
}
