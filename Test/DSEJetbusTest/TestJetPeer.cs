// <copyright file="TestJetPeer.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Test.DSEJetTest
{
    using Hbm.Devices.Jet;
    using Newtonsoft.Json.Linq;
    using System;

    /// <summary>
    /// Class to simulate the peer. F.e. to simulate when a Fetch-Event is raced by a Peer. 
    /// In method 'Fetch(..)' the data is simulated to be read from the wtx device and this methods commits the data to 'TestJetBusConnection'
    /// for further operations. 
    /// </summary>
    public class TestJetPeer
    {
        private TestJetbusConnection _connection;
        private Behavior behavior;

        private string path;
        private string Event;
        private int data;

        public TestJetPeer(Behavior _behaviorParameter, TestJetbusConnection _connectionParameter)
        {
            _connection = _connectionParameter;
            this.behavior = _behaviorParameter;

            path = "";
            Event= "";
            data = 0;
        }

        // Method to simulate the fetching of data from the wtx device : By adding and changing paths to the data buffer(='_databuffer') and by calling an event in TestJetbusConnection with invoke.  
        public JObject Fetch(out FetchId id, Matcher matcher, Action<JToken> fetchCallback, Action<bool, JToken> responseCallback, double responseTimeoutMs)
        {
            path  = "";
            Event = "";
            data  = 0;

            if (!_connection._dataBuffer.ContainsKey("6144/00")) // Only if the dictionary does not contain a path(for example "6014/01") the dictionary will be filled: 
            { 
                _connection._dataBuffer.Add("6144/00", simulateJTokenInstance("6144/00", "add", 0)["value"]);   // Read 'gross value'
                _connection._dataBuffer.Add("601A/01", simulateJTokenInstance("601A/01", "add", 0)["value"]);   // Read 'net value'
                _connection._dataBuffer.Add("6153/00", simulateJTokenInstance("6153/00", "add", 1)["value"]);   // Read 'weight moving detection'        
                _connection._dataBuffer.Add("6012/01", simulateJTokenInstance("6012/01", "add", 1)["value"]);   // Read 'Weighing device 1 (scale) weight status'

                _connection._dataBuffer.Add("SDO", simulateJTokenInstance("SDO", "add", 1)["value"]);
                _connection._dataBuffer.Add("FRS1", simulateJTokenInstance("FRS1", "add", 1)["value"]);
                _connection._dataBuffer.Add("NDS", simulateJTokenInstance("NDS", "add", 1)["value"]);

                _connection._dataBuffer.Add("6013/01", simulateJTokenInstance("6013/01", "add", 4)["value"]);   // Read 'Weight decimal point', f.e. = 4.
                _connection._dataBuffer.Add("IM1", simulateJTokenInstance("IM1", "add", 1)["value"]);
                _connection._dataBuffer.Add("IM2", simulateJTokenInstance("IM2", "add", 1)["value"]);
                _connection._dataBuffer.Add("IM3", simulateJTokenInstance("IM3", "add", 1)["value"]);
                _connection._dataBuffer.Add("IM4", simulateJTokenInstance("IM4", "add", 1)["value"]);

                _connection._dataBuffer.Add("OM1", simulateJTokenInstance("OM1", "add", 1)["value"]);
                _connection._dataBuffer.Add("OM2", simulateJTokenInstance("OM2", "add", 1)["value"]);
                _connection._dataBuffer.Add("OM3", simulateJTokenInstance("OM3", "add", 1)["value"]);
                _connection._dataBuffer.Add("OM4", simulateJTokenInstance("OM4", "add", 1)["value"]);

                _connection._dataBuffer.Add("OS1", simulateJTokenInstance("OS1", "add", 1)["value"]);
                _connection._dataBuffer.Add("OS2", simulateJTokenInstance("OS2", "add", 1)["value"]);
                _connection._dataBuffer.Add("OS3", simulateJTokenInstance("OS3", "add", 1)["value"]);
                _connection._dataBuffer.Add("OS4", simulateJTokenInstance("OS4", "add", 1)["value"]);

                _connection._dataBuffer.Add("CFT", simulateJTokenInstance("CFT", "add", 1)["value"]);
                _connection._dataBuffer.Add("FFT", simulateJTokenInstance("FFT", "add", 1)["value"]);
                _connection._dataBuffer.Add("TMD", simulateJTokenInstance("TMD", "add", 1)["value"]);
                _connection._dataBuffer.Add("UTL", simulateJTokenInstance("UTL", "add", 1)["value"]);
                _connection._dataBuffer.Add("LTL", simulateJTokenInstance("LTL", "add", 1)["value"]);
                _connection._dataBuffer.Add("MSW", simulateJTokenInstance("MSW", "add", 1)["value"]);
                _connection._dataBuffer.Add("EWT", simulateJTokenInstance("EWT", "add", 1)["value"]);
                _connection._dataBuffer.Add("TAD", simulateJTokenInstance("TAD", "add", 1)["value"]);
                _connection._dataBuffer.Add("CBT", simulateJTokenInstance("CBT", "add", 1)["value"]);
                _connection._dataBuffer.Add("CBK", simulateJTokenInstance("CBK", "add", 1)["value"]);
                _connection._dataBuffer.Add("FBK", simulateJTokenInstance("FBK", "add", 1)["value"]);
                _connection._dataBuffer.Add("FBT", simulateJTokenInstance("FBT", "add", 1)["value"]);
                _connection._dataBuffer.Add("SYD", simulateJTokenInstance("SYD", "add", 1)["value"]);
                _connection._dataBuffer.Add("VCT", simulateJTokenInstance("VCT", "add", 1)["value"]);
                _connection._dataBuffer.Add("EMD", simulateJTokenInstance("EMD", "add", 1)["value"]);
                _connection._dataBuffer.Add("CFD", simulateJTokenInstance("CFD", "add", 1)["value"]);
                _connection._dataBuffer.Add("FFD", simulateJTokenInstance("FFD", "add", 1)["value"]);
                _connection._dataBuffer.Add("SDM", simulateJTokenInstance("SDM", "add", 1)["value"]);
                _connection._dataBuffer.Add("SDS", simulateJTokenInstance("SDS", "add", 1)["value"]);
                _connection._dataBuffer.Add("RFT", simulateJTokenInstance("RFT", "add", 1)["value"]);

                _connection._dataBuffer.Add("MDT", simulateJTokenInstance("MDT", "add", 1)["value"]);
                _connection._dataBuffer.Add("FFM", simulateJTokenInstance("FFM", "add", 1)["value"]);
                _connection._dataBuffer.Add("OSN", simulateJTokenInstance("OSN", "add", 1)["value"]);
                _connection._dataBuffer.Add("FFL", simulateJTokenInstance("FFL", "add", 1)["value"]);
                _connection._dataBuffer.Add("DL1", simulateJTokenInstance("DL1", "add", 1)["value"]);
                _connection._dataBuffer.Add("6002/02", simulateJTokenInstance("6002/02", "add", 1801543519)["value"]); // = Status
                _connection._dataBuffer.Add("2020/25", simulateJTokenInstance("2020/25", "add", 0xA)["value"]);   // = Limit value status

                _connection._dataBuffer.Add("2010/07", simulateJTokenInstance("2010/07", "add", 1)["value"]);
                _connection._dataBuffer.Add("2110/06", simulateJTokenInstance("2110/06", "add", 1)["value"]);
                _connection._dataBuffer.Add("2110/07", simulateJTokenInstance("2110/07", "add", 1)["value"]);

                _connection._dataBuffer.Add("2020/18", simulateJTokenInstance("2020/18", "add", 0)["value"]);   // = Status digital input 1
                _connection._dataBuffer.Add("2020/19", simulateJTokenInstance("2020/19", "add", 0)["value"]);   // = Status digital input 2
                _connection._dataBuffer.Add("2020/1A", simulateJTokenInstance("2020/1A", "add", 0)["value"]);   // = Status digital input 3
                _connection._dataBuffer.Add("2020/1B", simulateJTokenInstance("2020/1B", "add", 0)["value"]);   // = Status digital input 4

                _connection._dataBuffer.Add("2020/1E", simulateJTokenInstance("2020/1E", "add", 0)["value"]);   // = Status digital output 1
                _connection._dataBuffer.Add("2020/1F", simulateJTokenInstance("2020/1F", "add", 0)["value"]);   // = Status digital output 2
                _connection._dataBuffer.Add("2020/20", simulateJTokenInstance("2020/20", "add", 0)["value"]);   // = Status digital output 3
                _connection._dataBuffer.Add("2020/21", simulateJTokenInstance("2020/21", "add", 0)["value"]);   // = Status digital output 4

                _connection._dataBuffer.Add("6014/01", simulateJTokenInstance("6014/01", "add", 0x4C0000)["value"]);    // Read Unit, prefix or fixed parameters - for t.

                _connection._dataBuffer.Add("6002/01", simulateJTokenInstance("6002/01", "add", 0)["value"]);
                _connection._dataBuffer.Add("6152/00", simulateJTokenInstance("6152/00", "add", 15000)["value"]);
                _connection._dataBuffer.Add("6015/01", simulateJTokenInstance("6015/01", "add", 0x004C0000)["value"]);
            }
            // For the different unit cases : lb, g, kg, t
            
            JToken JTokenobj = simulateJTokenInstance(path,Event,data);

            id = null;

            fetchCallback = (JToken x) => _connection.OnFetchData(JTokenobj);

            fetchCallback.Invoke(JTokenobj);

            return JTokenobj.ToObject<JObject>();
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
        
        public JObject Set(string path, JToken jsonValue, Action<JToken/*bool,*/> responseCallback, double responseTimeoutMs)
        {
            if (this.behavior == Behavior.lb_UnitValue_Fail || this.behavior == Behavior.g_UnitValue_Fail || this.behavior == Behavior.kg_UnitValue_Fail || this.behavior == Behavior.t_UnitValue_Fail)
            {
                path = "6015/01";
                Event = "change";
                data = 0x00000000;
            }

            switch (this.behavior)
            {

                case Behavior.setTestsSuccess:
                    path  = "6002/01";
                    Event = "change";
                    data  = 100010;
                    break;

                case Behavior.setTestsFail:
                    path  = "6002/02";
                    Event = "change";
                    data  = 1230;
                    break;

                case Behavior.WriteTareSuccess:
                    path  = "6002/01";
                    Event = "change";          
                    data  = 1701994868;            
                    break;

                case Behavior.WriteZeroSuccess:
                    path  = "6002/01";
                    Event = "change";
                    data  = 1869768058;
                    break;

                case Behavior.WriteGrossSuccess:
                    path  = "6002/01";
                    Event = "change";
                    data  = 1936683623;
                    break;

                case Behavior.t_UnitValue_Success:
                    path  = "6015/01";
                    Event = "change";
                    data  = 0x004C0000;
                    break;
                case Behavior.kg_UnitValue_Success:
                    path  = "6015/01";
                    Event = "change";
                    data  = 0x00020000;
                    break;
                case Behavior.g_UnitValue_Success:
                    path = "6015/01";
                    Event = "change";
                    data = 0x004B0000;
                    break;
                case Behavior.lb_UnitValue_Success:
                    path = "6015/01";
                    Event = "change";
                    data = 0x00A60000;
                    break;

                case Behavior.CalibrationSuccess:

                    if (path.Equals("6002/01"))
                    {
                        path = "6002/01";
                        Event = "change";
                        data = 1852596579;
                    }
                    
                    if (path.Equals("6152/00"))
                    {
                        path = "6152/00";
                        Event = "change";
                        data = 15000;
                    }
                    
                    break;
                
                case Behavior.CalibrationFail:

                    if (_connection.AllData["6002/02"] == "1634168417")
                    {
                        path = "6002/02";
                        Event = "change";
                        data = 1801543519;
                    }
                    else
                    if (_connection.AllData["6002/02"] == "1801543519")
                    {
                        path = "6002/02";
                        Event = "change";
                        data = 1634168417;
                    }
                    
                    if (path.Equals("6002/01"))
                    {
                       path  = "6002/01";
                       Event = "change";
                       data  = 0;
                    }


                   if (path.Equals("6152/00"))
                   {
                       path = "6152/00";
                       Event = "change";
                       data = 15000;
                   }

                   break;
               
                case Behavior.MeasureZeroSuccess:

                    if (path.Equals("6002/01"))
                    {
                        path  = "6002/01";
                        Event = "change";
                        data  = 2053923171;
                    }
                    
                    break;

                case Behavior.MeasureZeroFail:

                    if (path.Equals("6002/01"))
                    {
                        path  = "6002/01";
                        Event = "change";
                        data  = 0;
                    }

                    break;

                case Behavior.CalibratePreloadCapacitySuccess:

                    double preload = 1;
                    double capacity = 2;
                    double multiplierMv2D = 500000;
                    
                    int testIntPreload = Convert.ToInt32(preload * multiplierMv2D);
                    int testIntNominalLoad = Convert.ToInt32(testIntPreload + (capacity * multiplierMv2D));


                    if (path.Equals("2110/06"))
                    {
                        path  = "2110/06";
                        Event = "change";
                        data  = testIntPreload;
                    }

                    if (path.Equals("2110/07"))
                    {
                        path  = "2110/07";
                        Event = "change";
                        data  = testIntNominalLoad;
                    }

                    break;

            }

            JToken JTokenobj = simulateJTokenInstance(path, Event, data);

            responseCallback = (JToken x) => _connection.OnFetchData(JTokenobj);    

            responseCallback.Invoke(JTokenobj);

            return JTokenobj.ToObject<JObject>();
        }


        public class FetchData
        {
            public string path { get; set; }
            public string Event { get; set; }
            public int value { get; set; }
        }

        /*
        public JObject Authenticate(string user, string password, Action<bool, JToken> responseCallback, double responseTimeoutMs)
        {
        }
        
        public void Connect(Action<bool> completed, double timeoutMs)
        {
        }

        public void Disconnect()
        {           
        }
        */


    }
}
