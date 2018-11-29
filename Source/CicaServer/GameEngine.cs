using Cica.CicaMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaServer
{
    public class GameEngine
    {
        #region Interaction
            internal void Interate(Session session, Connection connection, Data data)
            {
                //TODO: Work over here
            }
        #endregion
        #region Iteration
            internal void Iterate(Session session)
            {
                //TODO: Work over here
            }
        #endregion

        #region Snapshot
            internal Data CreateSnapshot(Session session, Connection connection)
            {
                Data data = new Data();
                //Map
                //data.Items.Add(new DataItem(){ Type = DataItemType.Sprite

                //Actors

                //for (int i = 0; i < 1000; i++)
                //    data.Items.Add(new DataItem());
                return (data);
            }
        #endregion

    }
}
