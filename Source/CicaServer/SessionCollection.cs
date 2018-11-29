using Cica.CicaMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaServer
{
    public class SessionCollection
    {
        #region Attributes
            private Server _server;
        #endregion
        #region Propeties
            public Server Server 
            {
                get 
                {
                    return (this._server);
                }
            }
            private List<Session> Sessions { set; get; }
        #endregion
        #region Constructors
            public SessionCollection(Server server) 
            {
                this.Sessions = new List<Session>();
                this._server = server;
            }
        #endregion

        #region Create
            public int Create(string name) 
            {
                Session session = new Session(this);
                int sessionCode = 1;
                while (this.Sessions.Find(s => s.Code == sessionCode) != null)
                    sessionCode++;
                session.Code = sessionCode;
                session.Name = name;
                this.Sessions.Add(session);
                return (session.Code);
            }
        #endregion
        #region Retrieve
            public Dictionary<int, string> GetSessions() 
            {
                Dictionary<int, string> sessions = new Dictionary<int, string>();
                foreach (Session session in this.Sessions)
                    sessions.Add(session.Code, session.Name);
                return (sessions);
            }

            internal Session GetSession(int? sessionCode) 
            {
                if (!sessionCode.HasValue)
                    return (null);
                return (this.Sessions.Find(s => s.Code == sessionCode.Value));
            }
        #endregion
        #region Join
            public string Join(Connection connection, int? sessionCode) 
            {
                if (sessionCode.HasValue)
                {
                    Session session = this.Sessions.Find(s => s.Code == sessionCode);
                    if (session == null)
                        return ("Session not available");
                    return (this.Join(connection, session));
                }else {
                    foreach (Session session in this.Sessions) 
                    {
                        string message = this.Join(connection, session);
                        if (string.IsNullOrEmpty(message))
                            return(string.Empty);
                    }
                    return ("There is no session available");
                }
            }

            private string Join(Connection connection, Session session)
            {
                //Max
                if (!session.AcceptNewConnection())
                    return ("There is no slot available in session");
                //Join
                if(!session.AddConnection(connection))
                    return ("Can not join session");
                return (string.Empty);
            } 
        #endregion
        #region Leave
            public string Leave(Connection connection) 
            {
                //TODO: Work over here
                return (string.Empty);
            }
        #endregion
        #region Start
            public string Start(int sessionCode) 
            {
                Session session = this.Sessions.Find(s => s.Code == sessionCode);
                if (session == null)
                    return ("Session not available");
                return(session.Start());
            }
        #endregion
        #region Commands
            public void ExecuteSessionCommandResourcesList(Connection connection, Command command)
            {
                Session session = this.GetSession(connection.SessionCode);
                if (session == null)
                    return;
                session.ExecuteSessionCommandResourcesList(connection, command);
            }

            public void ExecuteSessionCommandResourceRetrieve(Connection connection, Command command)
            {
                Session session = this.GetSession(connection.SessionCode);
                if (session == null)
                    return;
                session.ExecuteSessionCommandResourceRetrieve(connection, command);
            }

            public void ExecuteSessionCommandResourceSynchronized(Connection connection, Command command)
            {
                Session session = this.GetSession(connection.SessionCode);
                if (session == null)
                    return;
                session.ExecuteSessionCommandResourceSynchronized(connection, command);
            }
        #endregion
    }
}
