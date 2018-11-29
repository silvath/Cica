using Data;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CicaClient
    {
        #region Events
            public delegate void LogDelegate(string message);
            public static LogDelegate LogEvent;
        #endregion
        #region Attributes
            private bool _running = false;
        #endregion
        #region Properties
            public bool IsRunning
            {
                get 
                {
                    return (this._running);
                }
            }
            private GameEngine Engine { set; get; }
            private NetworkManager Network { set; get; }
            private ScriptParser Parser { set; get; }
            private PackageManager Packer { set; get; }
        #endregion
        #region Constructors
            public CicaClient(GameEngine engine, NetworkManager network, ScriptParser parser, PackageManager packer) 
            {
                this.Engine = engine;
                this.Network = network;
                this.Parser = parser;
                this.Packer = packer;
            }
        #endregion

        #region Start
            public void Start() 
            {
                this.LogWrite("Starting");
                this._running = true;
                this.Engine.Initialize("Airplane");
                this.LogWrite("Started");
            }
        #endregion
        #region Send
            public Package Send(string command) 
            {
                return (Send(command, false));
            } 

            public Package Send(string command, bool echo) 
            {
                Package request;
                if (this.Parser.TryParse(command, out request))
                {
                    if (request.Type == PackageType.Script)
                    {
                        SendScript(request);
                        return(null);
                    }
                    //Request
                    if(echo)
                        LogWrite(request);
                    this.Network.Send(request);
                    //Response
                    Package response = this.Network.WaitReceive();
                    LogWrite(response);
                    return (response);
                }else{
                    this.LogWrite("Invalid Command");
                }
                return (null);
            }

            private void SendScript(Package request) 
            {
                //TODO: Another scripts here
                SendScriptGameStart();
            }

            private void SendScriptGameStart()
            {
                //Connect
                if (this.Network.Connect("Airplane"))
                {
                    this.LogWrite("Connected");
                }else{
                    this.LogWrite("Fail Connect");
                    return;
                }
                //Join 
                if (this.Network.Join())
                {
                    this.LogWrite("Joined");
                }else {
                    this.LogWrite("Fail Join");
                    return;
                }
                //Frames
                int? secondCurrent = null;
                int secondCount = 0;
                while (true)
                {
                    Package package = this.Network.Receive();
                    int second = DateTime.Now.Second;
                    if (!secondCurrent.HasValue)
                    {
                        secondCurrent = second;
                        secondCount = 0;
                    }else{
                        if (secondCurrent.Value != second)
                        {
                            this.LogWrite(secondCount.ToString());
                            secondCurrent = second;
                            secondCount = 0;
                        }else{
                            if (package != null)
                                secondCount++;
                        }
                    }
                }
            }
        #endregion
        #region Stop
            public void Stop()
            {
                this.Network.Disconnect();
                this._running = false;
            }
        #endregion

        #region Log
            private void LogWrite(Package package) 
            {
                LogWrite("*****");
                LogWrite(package.Type.ToString());
                foreach (PackageItem item in package.Items) 
                {
                    LogWrite(item.Type.ToString());
                    foreach (string data in item.Data)
                        LogWrite(data);
                }
                LogWrite("*****");
            }
            
            private void LogWrite(string message)
            {
                if (CicaClient.LogEvent != null)
                    CicaClient.LogEvent(message);
            }
        #endregion
    }
}
