using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindows
{
    public class EngineConfiguration
    {
        #region Properties
            public string Title{get;set;}
            public int Width{get;set;}
            public int Height{get;set;}
            public bool WaitVerticalBlanking{get;set;}
        #endregion
        #region Constructors
            public EngineConfiguration() : this("Cica") 
            {
            }   

            public EngineConfiguration(string title): this(title, 800, 600)
            {
            }

            public EngineConfiguration(string title, int width, int height) 
            {
                Title = title;
                Width = width;
                Height = height;
                WaitVerticalBlanking = false;
            }
        #endregion
    }
}
