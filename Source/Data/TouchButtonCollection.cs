using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class TouchButtonCollection : List<TouchButton>
    {
        #region Create
            public TouchButton Create(int? top, int? down, int? left, int? right, int width, int height, InputType input, string animationName) 
            {
                TouchButton touchButton = new TouchButton();
                touchButton.Top = top;
                touchButton.Down = down;
                touchButton.Left = left;
                touchButton.Right = right;
                touchButton.Width = width;
                touchButton.Height = height;
                touchButton.Input = input;
                touchButton.AnimationName = animationName;
                this.Add(touchButton);
                return (touchButton);
            }
        #endregion
        #region Get
            public TouchButton GetTouchButton(int viewWidth, int viewHeight, int x, int y, bool proximity) 
            {
                TouchButton touchButtonproximity = null;
                int distance = 0;
                foreach (TouchButton touchButton in this) 
                {
                    int tbX = touchButton.Left ?? viewWidth - touchButton.Right.Value;
                    int tbY = touchButton.Top ?? viewHeight - touchButton.Down.Value;
                    if ((tbX <= x) && (tbY <= y) && (x <= (tbX + touchButton.Width)) && (y < (tbY + touchButton.Height)))
                        return (touchButton);
                    if (!proximity)
                        continue;
                    int distanceX = Math.Abs(tbX + (touchButton.Width / 2) - x);
                    int distanceY = Math.Abs(tbY + (touchButton.Height / 2) - y);
                    int touchButtonDistance = distanceX + distanceY;
                    if ((touchButtonproximity != null) && (distance < touchButtonDistance))
                        continue;
                    touchButtonproximity = touchButton;
                    distance = touchButtonDistance;
                }
                if (proximity)
                    return (touchButtonproximity);
                return (null);
            }
        #endregion
    }
}
