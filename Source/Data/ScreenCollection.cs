using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class ScreenCollection : List<Screen>
    {
        public Screen Create(ScreenType type, string name) 
        {
            Screen screen = new Screen();
            screen.Type = type;
            screen.Name = name;
            this.Add(screen);
            return (screen);
        }

        public Screen GetScreen(ScreenType type) 
        {
            foreach (Screen screen in this)
                if (screen.Type == type)
                    return (screen);
            return (type != ScreenType.Game ? GetScreen(ScreenType.Game) : null);
        }

        public Screen GetScreen(string name)
        {
            foreach (Screen screen in this)
                if (screen.Name == name)
                    return (screen);
            return (null);
        }
    }
}
