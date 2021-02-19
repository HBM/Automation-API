// <copyright file="TestJetbusConnection.cs" company="Hottinger Baldwin Messtechnik GmbH">
//
// WTXGUIsimple, a demo application for HBM Weighing-API  
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

namespace Hbm.Automation.Api.Test.WTXJetBusTest
{
    using Hbm.Devices.Jet;
    using Hbm.Automation.Api;
    using Hbm.Automation.Api.Weighing.WTX.Jet;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Security;
    using System.Threading;
    using System.Threading.Tasks;

    public enum Behavior
    {
        ConnectionFail,
        ConnectionSuccess,

        DisconnectionFail,
        DisconnectionSuccess,

        NetGrossTareValues_Fail,
        NetGrossTareValues_Success,

        ReadFail_WEIGHING_DEVICE_1_WEIGHT_STATUS,
        ReadSuccess_WEIGHING_DEVICE_1_WEIGHT_STATUS,

        WriteTareFail,
        WriteTareSuccess,

        WriteGrossFail,
        WriteGrossSuccess,

        WriteZeroFail,
        WriteZeroSuccess,

        CalibrationFail,
        CalibrationSuccess,

        CalibratePreloadCapacityFail,
        CalibratePreloadCapacitySuccess,

        MeasureZeroFail,
        MeasureZeroSuccess,

        ReadFail_Decimals,
        ReadSuccess_Decimals,

        ReadFail_FillingProcessSatus,
        ReadSuccess_FillingProcessSatus,

        ReadFail_DosingResult,
        ReadSuccess_DosingResult,

        ReadFail_NumberDosingResults,
        ReadSuccess_NumberDosingResults,

        ReadFail_Unit,
        ReadSuccess_Unit,

        t_UnitValue_Fail,
        t_UnitValue_Success,

        kg_UnitValue_Fail,
        kg_UnitValue_Success,

        g_UnitValue_Fail,
        g_UnitValue_Success,

        lb_UnitValue_Fail,
        lb_UnitValue_Success,

        N_UnitValue_Fail,
        N_UnitValue_Success,

        NetGrossValueStringComment_4D_Fail,
        NetGrossValueStringComment_4D_Success,

        NetGrossValueStringComment_3D_Fail,
        NetGrossValueStringComment_3D_Success,

        NetGrossValueStringComment_2D_Fail,
        NetGrossValueStringComment_2D_Success,

        NetGrossValueStringComment_1D_Fail,
        NetGrossValueStringComment_1D_Success,

        ReadFail_Attributes,
        ReadSuccess_Attributes,

        LimitValues_WeightWithinLimits,
        LimitValues_Underload,
        LimitValues_Overload,
        LimitValues_HigherSafeLoadLimit,

        ReadFail_DataReceived,
        ReadSuccess_DataReceived,

        setTestsFail,
        setTestsSuccess,

    }

    public class TestJetbusConnection : INetConnection, IDisposable
    {
        private Behavior behavior;
        private List<string> messages;
        private bool connected;

        public event EventHandler<LogEventArgs> CommunicationLog;
        public event EventHandler<EventArgs> UpdateData;

        private int _mTimeoutMs;

        public Dictionary<string, JToken> _dataBuffer;

        private AutoResetEvent _mSuccessEvent = new AutoResetEvent(false);
                
        private Exception _mException = null;

        private string IP;
        private int interval;

        private JToken[] JTokenArray;
        private ushort[] DataUshortArray;
        private string[] DataStrArray;

        private TestJetPeer _peer;
        
        // Constructor with all parameters possible from class 'JetbusConnection' - Without ssh certification.
        //public TestJetbusConnection(Behavior behavior, string ipAddr, string user, string passwd, RemoteCertificateValidationCallback certificationCallback, int timeoutMs = 5000) : base(ipAddr, user, passwd, certificationCallback, timeoutMs = 5000)

        public TestJetbusConnection(Behavior behavior, string ipAddr, string user, string passwd, RemoteCertificateValidationCallback certificationCallback, int timeoutMs = 5000)
        {
            //IJetConnection jetConnection = new WebSocketJetConnection(_uri, RemoteCertificationCheck);

            _peer = new TestJetPeer(behavior, this);
            
            this.connected = false;
            this.behavior = behavior;
            this.messages = new List<string>();

            _dataBuffer = new Dictionary<string, JToken>();

            this._mTimeoutMs = 5000; // values of 5000 according to the initialization in class JetBusConnection. 

            //ConnectOnPeer(user, passwd, timeoutMs);
            FetchAll();
        }

        public Behavior GetBehavior
        {
            get
            {
                return this.behavior;
            }
        }

        public int SendingInterval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }

        public string Read(object index)
        {
            try
            {
                return ReadObj(index).ToString();
            }
            catch (FormatException)
            {
                throw new Exception("Invalid data format");
            }
        }

        protected JToken ReadObj(object index)
        {
            
            switch (this.behavior)
            {             
                case Behavior.CalibrationSuccess:

                    if (_dataBuffer.ContainsValue(1801543519))
                        _dataBuffer["6002/02"] = 1634168417;  // = command 'on go', in exection.
                    else
                       if (_dataBuffer.ContainsValue(1634168417))
                        _dataBuffer["6002/02"] = 1801543519;  // = command ok, done. 

                    return _dataBuffer["6002/02"];

                case Behavior.CalibrationFail:

                    if (_dataBuffer.ContainsValue(1801543519))
                        _dataBuffer["6002/02"] = 1634168417;  // = command 'on go', in exection.
                    else
                       if (_dataBuffer.ContainsValue(1634168417))
                        _dataBuffer["6002/02"] = 1801543519;  // = command ok, done. 

                    return _dataBuffer["6002/02"];

                case Behavior.MeasureZeroSuccess:

                    if (_dataBuffer.ContainsValue(1801543519))
                        _dataBuffer["6002/02"] = 1634168417;  // = command 'on go', in exection.
                    else
                        if (_dataBuffer.ContainsValue(1634168417))
                        _dataBuffer["6002/02"] = 1801543519;  // = command ok, done. 

                    return _dataBuffer["6002/02"];
                    
                case Behavior.MeasureZeroFail:

                    if (_dataBuffer.ContainsValue(1801543519))
                        _dataBuffer["6002/02"] = 1634168417;  // = command 'on go', in exection.
                    else
                        if (_dataBuffer.ContainsValue(1634168417))
                        _dataBuffer["6002/02"] = 1801543519;  // = command ok, done. 

                    return _dataBuffer["6002/02"];
                    
                default:
                    break;               
            }

            if (IsConnected)
            {
                this.UpdateData?.Invoke(this, new EventArgs());
            }

            CommunicationLog?.Invoke(this, new LogEventArgs(index.ToString()));

            return _dataBuffer[index.ToString()];
            
        }

        private void ConvertJTokenToStringArray()
        {
            JTokenArray = _dataBuffer.Values.ToArray();
            DataUshortArray = new ushort[JTokenArray.Length];
            DataStrArray = new string[JTokenArray.Length];

            for (int i = 0; i < JTokenArray.Length; i++)
            {
                JToken JTokenElement = JTokenArray[i];
                DataStrArray[i] = JTokenElement.ToString();
            }

        }

        public Dictionary<string, string> AllData
        {
            get
            {
                Dictionary<string, string> newDict = new Dictionary<string, string>();

                foreach (var element in _dataBuffer)
                {
                    int i = 0;

                    if (int.TryParse(element.Value.ToString(), out i))
                        newDict.Add(element.Key,element.Value.ToString());
                }

                return newDict;
            }
        }

        public Dictionary<string, JToken> getDataBuffer
        {
            get
            {
                return this._dataBuffer;
            }
        }

        public int NumofPoints
        {
            get
            {
                return 38;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.connected;
            }

            set
            {
                this.connected = value;
            }
        }

        public string IpAddress
        {
            get
            {
                return this.IP;
            }

            set
            {
                this.IP = value;
            }
        }
        public ConnectionType ConnectionType
        {
            get { return ConnectionType.Jetbus; }
        }

        public void FetchAll()
        {
            Matcher matcher = new Matcher();
            FetchId id;
            _peer.Fetch(out id, matcher, OnFetchData, null, 500); // Onfetch = null (given by 'JetBusConnection'), timeoutms=500;         
        }

        protected virtual void WaitOne(int timeoutMultiplier = 1)
        {
            if (!_mSuccessEvent.WaitOne(_mTimeoutMs * timeoutMultiplier))
            {

                this.connected = false;
                //
                // Timeout-Exception
                //

                throw new TimeoutException("Interface Timeout - signal-handler will never reset");
            }
            if (_mException != null)
            {
                Exception exception = _mException;
                _mException = null;
                throw exception;
            }
        }

        /// <summary>
        /// Event with callend when raced a Fetch-Event by a other Peer.
        /// For testing it must be filled with pseudo data be tested in the UNIT tests. 
        /// </summary>
        /// <param name="data"></param>
        public void OnFetchData(JToken data)
        {
            string path = data["path"].ToString();
            string Event = data["Event"].ToString();
            lock (_dataBuffer)
            {
                switch (Event)
                {
                    case "add":
                        _dataBuffer.Add(path, data["value"]);

                        break;

                    case "fetch":
                        _dataBuffer[path] = data["value"];

                        break;

                    case "change":
                        _dataBuffer[path] = data["value"];

                        break;
                }

                if (IsConnected)
                {
                    this.UpdateData?.Invoke(this, new EventArgs());
                }

                CommunicationLog?.Invoke(this, new LogEventArgs(data.ToString()));
            }
        }

        public void Connect(int timeoutMs = 20000)
        {
            switch (this.behavior)
            {
                case Behavior.ConnectionFail:
                    connected = false;
                    break;

                case Behavior.ConnectionSuccess:
                    connected = true;
                    break;

                default:
                    connected = true;
                    break;
            }
        }

        public void Disconnect()
        {
            switch (this.behavior)
            {
                case Behavior.DisconnectionFail:
                    connected = true;
                    break;

                case Behavior.DisconnectionSuccess:
                    connected = false;
                    break;

                default:
                    connected = true;
                    break;
            }
        }

        private void OnSet(/*bool success, */JToken token)
        {
            /*
            if (!success)
            {
                JetBusException exception = new JetBusException(token);
                _mException = new Exception(exception.Error.ToString());
            }
            */
            _mSuccessEvent.Set();

            CommunicationLog?.Invoke(this, new LogEventArgs("Set data" + true));
        }

        private void SetData(object path, JValue value)
        {
            try
            {
                JObject request = _peer.Set(path.ToString(), value, OnSet,5000);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public bool WriteInteger(object command, int data)
        {

            JetBusCommand _command = (JetBusCommand)command;
            
            if(this.behavior == Behavior.WriteZeroSuccess   || this.behavior == Behavior.WriteGrossSuccess || this.behavior == Behavior.WriteTareSuccess   ||
               this.behavior == Behavior.CalibrationSuccess || this.behavior == Behavior.CalibrationFail   || this.behavior == Behavior.MeasureZeroSuccess ||
               this.behavior == Behavior.MeasureZeroFail    || this.behavior == Behavior.CalibratePreloadCapacitySuccess)
               {
                JValue valueObj = new JValue(data);
                this.SetData(_command.Path, valueObj);
               }


            if(this.behavior == Behavior.setTestsSuccess || this.behavior == Behavior.setTestsFail)
            {
                JValue valueObj = new JValue(data);
                this.SetData(_command.Path, valueObj);
            }

            return true;
        }

        public bool Write(object command, string data)
        {
            throw new NotImplementedException();
        }
        
        public JToken simulateJTokenInstance(string pathParam, string eventParam, int data)
        {

            FetchData fetchInstance = new FetchData
            {
                path = pathParam,    // For path  = "6014/01" (f.e.)
                Event = eventParam,  // For event = "add" || "change" || "fetch" 
                value = data,        
            };

            return JToken.FromObject(fetchInstance);
        }


        public void WriteArray(string index, int data)
        {
            //throw new NotImplementedException();
        }

        public void SendMessage(string json)
        {
            messages.Add(json);
        }

        public class FetchData
        {
            public string path { get; set; }
            public string Event { get; set; }
            public int value { get; set; }
        }

        public Task<string> ReadAsync(object commmand)
        {
            throw new NotImplementedException();
        }

        public Task<int> WriteAsync(object commmand, int value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Write(string register, DataType dataType, int value)
        {
            throw new NotImplementedException();
        }

        public string ReadFromBuffer(object command)
        {
            this.DoHandshake();

            switch (this.behavior)
            {
                case Behavior.t_UnitValue_Success:
                    _dataBuffer["6015/01"] = Convert.ToString(0x004C0000);  // Unit, prefix or fixed parameters - for unit 't'.
                    break;
                case Behavior.kg_UnitValue_Success:
                    _dataBuffer["6015/01"] = Convert.ToString(0x00020000);  // Unit, prefix or fixed parameters - for unit 'kg'.
                    break;
                case Behavior.g_UnitValue_Success:
                    _dataBuffer["6015/01"] = Convert.ToString(0x004B0000);  // Unit, prefix or fixed parameters - for unit 'g'.
                    break;
                case Behavior.lb_UnitValue_Success:
                    _dataBuffer["6015/01"] = Convert.ToString(0X00A60000);  // Unit, prefix or fixed parameters - for unit 'lb'.
                    break;
                case Behavior.N_UnitValue_Success:
                    _dataBuffer["6015/01"] = Convert.ToString(0x00210000);  // Read Unit, prefix or fixed parameters - for unit 'N'.
                    break;

                case Behavior.LimitValues_WeightWithinLimits:
                    _dataBuffer["6012/01"] = Convert.ToString(0x00);
                    break;
                case Behavior.LimitValues_Underload:
                    _dataBuffer["6012/01"] = Convert.ToString(0x4);  // For limit values
                    break;
                case Behavior.LimitValues_Overload:
                    _dataBuffer["6012/01"] = Convert.ToString(0x8);
                    break;
                case Behavior.LimitValues_HigherSafeLoadLimit:
                    _dataBuffer["6012/01"] = Convert.ToString(0xC); 
                    break;
                case Behavior.NetGrossTareValues_Success:
                    _dataBuffer["601A/01"] = Convert.ToString(11);
                    _dataBuffer["6143/00"] = Convert.ToString(11);
                    break;

                default:
                    _dataBuffer["6015/01"] = Convert.ToString(0x00);  // Unit, prefix or fixed parameters - for default value 
                    _dataBuffer["601A/01"] = Convert.ToString(0);
                    break;
            }

            JetBusCommand jetcommand = (JetBusCommand)command;
                 
            return jetcommand.ToString(AllData[jetcommand.Path]);
        }

        public string ReadFromDevice(object command)
        {
            return null; //tbd
        }

        ///<inheritdoc/>
        public int ReadIntegerFromBuffer(object command)
        {       
            JetBusCommand jetcommand = (JetBusCommand)command;

            return jetcommand.ToSValue(AllData[jetcommand.Path]);
        }

        private int state = 0;

        private void DoHandshake()
        {
            if (this.behavior == Behavior.CalibrationSuccess || this.behavior == Behavior.CalibrationFail || this.behavior == Behavior.MeasureZeroSuccess || this.behavior == Behavior.MeasureZeroFail)
            {
                switch(state)
                {
                    case 0:
                        if (_dataBuffer.ContainsValue(1801543519))
                            _dataBuffer["6002/02"] = 1634168417;  // = command 'on go', in exection.
                        state = 1;
                        break;

                    case 1:
                        if (_dataBuffer.ContainsValue(1634168417))
                            _dataBuffer["6002/02"] = 1801543519;  // = command ok, done. 
                        state = 0;
                        break;
                }
            }
        }

        public void WriteSync(ushort wordNumber, ushort commandParam)
        {
            throw new NotImplementedException();
        }
    }
}
 