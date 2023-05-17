// <copyright file="JetBusConnection.cs" company="Hottinger Baldwin Messtechnik GmbH">
//
// Hbm.Automation.Api, a library to communicate with HBM weighing technology devices  
//
// The MIT License (MIT)
//
// Copyright (C) Hottinger Baldwin Messtechnik GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// </copyright>

namespace Hbm.Automation.Api.Weighing.DSE.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Hbm.Automation.Api.SharpJet;
    using Hbm.Automation.Api.Weighing.WTX.Jet;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Use this class to handle a connection via Ethernet.
    /// This class establishes the communication to your WTX device, starts/ends the connection,
    /// receives and stores new data or writes new data.
    /// </summary>
    public class DSEJetConnection : INetConnection, IDisposable
    {
        #region ==================== constants & fields ====================
        private JetPeer _peer;
        private AutoResetEvent _successEvent = new AutoResetEvent(false);
        private Exception _localException = null;
        private int _timeoutMs;
        private bool _disposedValue = false;
        #endregion

        #region ==================== events & delegates ====================
        public event EventHandler<LogEventArgs> CommunicationLog;

        public event EventHandler<EventArgs> UpdateData;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Initializes a new instance of the <see cref="JetBusConnection" /> class.
        /// Use this constructor for individual authentication
        /// </summary>
        /// <param name="ipAddress">IP address of the target device</param>
        public DSEJetConnection(string ipAddress)
        {
            string _uri;
            IJetConnection jetConnection;

           _uri = "ws://" + ipAddress + ":80/jet/canopen";
           jetConnection = new WebSocketJetConnection(_uri);
            
            _peer = new JetPeer(jetConnection);
            
            this.IpAddress = ipAddress;
        }
        #endregion

        #region ======================== properties ========================
        ///<inheritdoc/>
        public ConnectionType ConnectionType => ConnectionType.DSEJet;

        ///<inheritdoc/>
        public bool IsConnected { get; private set; }
        
        ///<inheritdoc/>
        public string IpAddress { get; set; }

        ///<inheritdoc/>
        public Dictionary<string, string> AllData { get; } = new Dictionary<string, string>();
        #endregion

        #region ================ public & internal methods =================
        ///<inheritdoc/>
        public void Connect(int timeoutMs = 20000)
        {
            IsConnected = false;
            _timeoutMs = timeoutMs;
            ConnectPeer(timeoutMs); 
            WaitOne(3);
        }

        ///<inheritdoc/>
        public void Disconnect()
        {
            if (this.IsConnected)
            {
                _peer.Disconnect();
                this.IsConnected = false;
            }
        }

        ///<inheritdoc/>
        public string Read(object command)
        {
            return ReadFromBuffer(command);
        }

        ///<inheritdoc/>
        public Task<string> ReadAsync(object command)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public string ReadFromBuffer(object command)
        {
            string result = "0";
            JetBusCommand jetcommand = (JetBusCommand)command;
            if (AllData.ContainsKey(jetcommand.Path))
                result = jetcommand.ToString(AllData[jetcommand.Path]);
            return result;
        }

        ///<inheritdoc/>
        public string ReadFromDevice(object command)
        {
            string result = "0";
            JetBusCommand jetcommand = (JetBusCommand)command;
            if(!DSEFetchTargets.Contains(jetcommand.Path))
            {
                result = jetcommand.ToString(OneTimeFetch(jetcommand));
            }
            else if (AllData.ContainsKey(jetcommand.Path))
            {
                result = jetcommand.ToString(AllData[jetcommand.Path]);
            }
            return result;
        }

        ///<inheritdoc/>
        private string OneTimeFetch(JetBusCommand jetcommand)
        {
            string result = "-1";
            FetchId id;
            Matcher matcher = new Matcher();
            matcher.EqualsTo = jetcommand.Path;
            _peer.Fetch(out id, matcher, OnFetchData, OnFetch, this._timeoutMs);
            while (!AllData.ContainsKey(jetcommand.Path)) { }
            result = jetcommand.ToString(AllData[jetcommand.Path]);
            _peer.Unfetch(id, OnFetch, this._timeoutMs);
            AllData.Remove(jetcommand.Path);
            return result;
        }

        /// <inheritdoc />
        public int ReadIntegerFromBuffer(object command)
        {
            JetBusCommand jetcommand = (JetBusCommand)command;

            if(!DSEFetchTargets.Contains(jetcommand.Path))
            {
                return jetcommand.ToSValue(OneTimeFetch(jetcommand));
            }
            else
            {
                return jetcommand.ToSValue(AllData[jetcommand.Path]);
            }
        }

        ///<inheritdoc/>
        public bool WriteInteger(object command, int value)
        {
            bool result = false;
            JValue jasonValue = new JValue(value);
            JetBusCommand _command = (JetBusCommand)command;
            SetData(_command.Path, jasonValue);
            WaitOne(1);
            result = true;
            return result;
        }

        ///<inheritdoc/>
        public bool Write(object command, string value)
        {
            bool result = false;
            JValue jasonValue = new JValue(value);
            JetBusCommand _command = (JetBusCommand)command;
            SetData(_command.Path, jasonValue);
            WaitOne(1);
            result = true;
            return result;
        }

        ///<inheritdoc/>
        public Task<int> WriteAsync(object command, int commandParam)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region =============== protected & private methods ================
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _successEvent.Close();
                    _successEvent.Dispose();
                }

                _disposedValue = true;
            }
        }

        private void ConnectPeer(int timeoutMs)
        {
            _peer.Connect(OnConnected, timeoutMs);
        }

        private void OnConnected(bool connected)
        {
            if (connected)
            {
                FetchSelective();
                CommunicationLog?.Invoke(this, new LogEventArgs("Connection successful"));
            }
            else
            {
                _localException = new Exception("Connection failed");
                _successEvent.Set();
            }
        }

        private void FetchAll()
        {
            Matcher matcher = new Matcher();
            FetchId id;
            _peer.Fetch(out id, matcher, OnFetchData, OnFetch, this._timeoutMs);
        }


        string[] DSEFetchTargets = new string[]
        {

            "6002/02",   //Scale Status: OK: 6b6f5f5f | ONGO: 6f676e6f
            "6012/01",   //6012.1 = Weighing device 1 (scale) weight status
            "6013/01",   //6013.1 = Scale1 decimal point
            "6015/01",   //6015.1 = Weighing Device 1 (scale) unit and prefix output weight
            "6016/01",   //6016.1 = weight step. 1=1
            "601A/01",   //601a.1 = output weight    
            "6113/01",   //scale maximum capacity
            "611C/01",   //611c.1 = control
            "611C/02",   //611c.2 = limit1
            "611C/03",   //611c.3 = limit2
            "6141/02",   //6142.0 = Zero value
            "6143/00",   //6143.0 = Tare value
            "6144/00",   //6144.0 = Gross value
            //"6152/00",   //Scale calibration weight
            "6153/00",   //6153.0 = Weight movement detection
            "6002/00",     //Scale command status
        };

        private void FetchSelective()
        {
            Matcher matcher = new Matcher();
            FetchId id;
            foreach (string fetchtarget in DSEFetchTargets)
            {
                matcher.EqualsTo = fetchtarget;
                _peer.Fetch(out id, matcher, OnFetchData, OnFetch, this._timeoutMs);
            }
        }

        private void OnFetch(bool success, JToken token)
        {
            if (success)
            {
                IsConnected = true;
            }
            else
            {
                JetBusException exception = new JetBusException(token);
                _localException = new Exception(exception.ErrorCode.ToString());
            }

            _successEvent.Set();

            this.UpdateData?.Invoke(this, new EventArgs());

            CommunicationLog?.Invoke(this, new LogEventArgs("Fetch-Selective success: " + success + " - Buffer size is " + AllData.Count));
        }            

        private void WaitOne(int timeoutMultiplier = 1)
        {
            if (!_successEvent.WaitOne(_timeoutMs * timeoutMultiplier))
            {
                throw new Exception("Jet connection timeout");
            }
                     
            if (_localException != null)
            {
                CommunicationLog?.Invoke(this, new LogEventArgs(_localException.Message));
                Exception exception = _localException;
                _localException = null;
                throw exception;
            }                        
        }
        
        /// <summary>
        /// Event will be called when device sends new fetch events
        /// </summary>
        /// <param name="data">New device data from jet peer</param>
        private void OnFetchData(JToken data)
        {
            string path = data["path"].ToString();

            lock (AllData)
            {
                switch (data["event"].ToString())
                {
                    case "add":
                        AllData.Add(path, data["value"].ToString());
                        break;

                    case "fetch":
                        AllData[path] = data["value"].ToString();
                        break;

                    case "change":
                        AllData[path] = data["value"].ToString();
                        break;
                }
      
                if (IsConnected)
                {
                    this.UpdateData?.Invoke(this, new EventArgs());
                }
         
                CommunicationLog?.Invoke(this, new LogEventArgs(data.ToString()));
            }
        }
               
        /// <summary>
        /// Sets data for a single jet path
        /// </summary>
        /// <param name="path">Jet path for the data (e.g. "6002/01)"</param>
        /// <param name="value">The new value for the path</param>
        private void SetData(string path, JValue value)
        {
            _localException = null; 
            try
            {
                JObject request = _peer.Set(path.ToString(), value, OnSet, this._timeoutMs);
            }
            catch (Exception e)
            {
                _successEvent.Set();
                _localException = e;
            }
        }

        private void OnSet(bool success, JToken token)
        {
           if (!success)
           {
                _localException = new JetBusException(token);
            }
            
           _successEvent.Set();
            
           CommunicationLog?.Invoke(this, new LogEventArgs("Set data" + success));
        }      
 
        #endregion
    }
}
