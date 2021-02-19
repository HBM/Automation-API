// <copyright file="ConnectTestsModbus.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Test.WTXModbusTest
{
    using System.Collections;
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.WTX;
    using NUnit.Framework;
 
    [TestFixture]
    public class ConnectTestsModbus 
    {
        private string ipaddress = "172.19.103.8";
        //private bool connectCallbackCalled;
        //private bool connectCompleted;

        //private bool disconnectCallbackCalled;
        private bool disconnectCompleted;

        private static ushort[] _dataReadSuccess;
        private static ushort[] _dataReadFail;
        
        private TestModbusTCPConnection testConnection;
        private WTXModbus WTXModbusObj;

        // Test case source for the connection establishment. 
        public static IEnumerable ConnectTestCases 
        { 
        get 
        { 
            yield return new TestCaseData(Behavior.ConnectionSuccess).Returns(true);                
            yield return new TestCaseData(Behavior.ConnectionFail).Returns(false); 
        } 
        }

        // Test case source for the connection establishment. 
        public static IEnumerable DisconnectTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.DisconnectionSuccess).Returns(false);
                yield return new TestCaseData(Behavior.DisconnectionFail).Returns(true);
            }
        }

        [SetUp]
        public void Setup()
        {
            //this.connectCallbackCalled = true;
            //this.connectCompleted = true;

            //Array size for standard mode of the WTX120 device: 
            _dataReadFail     = new ushort[59];
            _dataReadSuccess  = new ushort[59];

            for (int i = 0; i < _dataReadSuccess.Length; i++)
            {
                _dataReadSuccess[i] = 0;
                _dataReadFail[i] = 0;
            }

            _dataReadSuccess[0] = 16448;       // Net value
            _dataReadSuccess[1] = 16448;       // Gross value
            _dataReadSuccess[2] = 0;           // General scale error
            _dataReadSuccess[3] = 0;           // Scale alarm triggered
            _dataReadSuccess[4] = 0;           // Limit status
            _dataReadSuccess[5] = 0;           // Weight moving
            _dataReadSuccess[6] = 0;//1;       // Scale seal is open
            _dataReadSuccess[7] = 0;           // Manual tare
            _dataReadSuccess[8] = 0;           // Weight type
            _dataReadSuccess[9] = 0;           // Scale range
            _dataReadSuccess[10] = 0;          // Zero required/True zero
            _dataReadSuccess[11] = 0;          // Weight within center of zero 
            _dataReadSuccess[12] = 0;          // weight in zero range
            _dataReadSuccess[13] = 0;          // Application mode = 0
            _dataReadSuccess[14] = 0; //4;     // Decimal Places
            _dataReadSuccess[15] = 0; //2;     // Unit
            _dataReadSuccess[16] = 0;          // Handshake
            _dataReadSuccess[17] = 0;          // Status

        }

        [Test, TestCaseSource(typeof(ConnectTestsModbus), "ConnectTestCases")]
        public bool ConnectTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            WTXModbusObj = new WTXModbus(testConnection, 200, update);

            //this.connectCallbackCalled = false;

            WTXModbusObj.Connect(this.OnConnect, 100);
            
            return WTXModbusObj.IsConnected;
        }

        private void OnConnect(bool connectCompleted)
        {
            //this.connectCallbackCalled = true;
            //this.connectCompleted = connectCompleted;
        }
       
        [Test, TestCaseSource(typeof(ConnectTestsModbus), "DisconnectTestCases")]
        public bool DisconnectTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            WTXModbusObj = new WTXModbus(testConnection, 200, update);

            WTXModbusObj.Connect(this.OnConnect, 100);
            
            WTXModbusObj.Disconnect(this.OnDisconnect);

            return WTXModbusObj.IsConnected;       
        }

        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnDisconnect(bool disonnectCompleted)
        {
            //this.disconnectCallbackCalled = true;
            this.disconnectCompleted = disonnectCompleted;
        }
    }
}
