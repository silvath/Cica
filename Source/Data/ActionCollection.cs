using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class ActionCollection : List<Action>
    {
        public Action Create(InputType input, ActionType action) 
        {
            return (Create(input, action, string.Empty));
        }

        public Action Create(InputType input, ActionType action, string screenName)
        {
            Action actionNew = new Action();
            actionNew.Input = input;
            actionNew.Type = action;
            actionNew.ScreenName = screenName;
            this.Add(actionNew);
            return (actionNew);
        }

        public Action Create(int x, int y, int width, int height, ActionType action) 
        {
            return (this.Create(x, y, width, height, action, string.Empty));
        }

        public Action Create(int x, int y, int width, int height, ActionType action, string screenName)
        {
            Action actionNew = new Action();
            actionNew.Touch = new Rectangle(x, y, width, height);
            actionNew.Type = action;
            actionNew.ScreenName = screenName;
            this.Add(actionNew);
            return (actionNew);
        }

        public Action GetAction(InputType input) 
        {
            foreach (Action action in this)
                if (action.Input == input)
                    return (action);
            return (null);
        }

        public Action GetAction(float scale, float viewLeft, float viewTop, int x, int y) 
        {
            foreach (Action action in this)
            {
                if (action.Touch == null)
                    continue;
                int xTouch = (int)((action.Touch.X * scale) + viewLeft);
                int yTouch = (int)((action.Touch.Y * scale) + viewTop);
                int widthTouch = (int)(action.Touch.Width * scale);
                int heightTouch = (int)(action.Touch.Height * scale);
                if ((xTouch <= x) && (yTouch <= y) && (x <= (xTouch + widthTouch)) && (y < (yTouch + heightTouch)))
                    return (action);
            }
            return (null);
        }
    }
}
