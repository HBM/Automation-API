// <copyright file="ReadTestsModbus.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.WTX;
    using Hbm.Automation.Api.Weighing.WTX.Modbus;
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Threading.Tasks;

    [TestFixture]
    public class ReadTestsModbus
    {
        private TestModbusTCPConnection testConnection;
        private WTXModbus _wtxDevice;
        private string ipaddress = "172.19.103.8";

        /*
        private bool connectCallbackCalled;
        private bool connectCompleted;

        private bool disconnectCallbackCalled;
        private bool disconnectCompleted;
        */

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable ReadTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.ReadFail).Returns(0);
                yield return new TestCaseData(Behavior.ReadSuccess).Returns(16448);
            }
        }

        // Test case source for checking the transition of the handshake bit. 
        public static IEnumerable HandshakeTestCases
        {
            get
            {
               // yield return new TestCaseData(Behavior.HandshakeFail).Returns(1);
                yield return new TestCaseData(Behavior.HandshakeSuccess).Returns(false);
            }
        }


        public static IEnumerable MeasureZeroTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.MeasureZeroFail).Returns(false);
                yield return new TestCaseData(Behavior.MeasureZeroSuccess).Returns(true);
            }
        }

        // Test case source for checking the values of the application mode: 

        public static IEnumerable ApplicationModeTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.InStandardMode).Returns(0);
                yield return new TestCaseData(Behavior.InFillerMode).Returns(1);
            }
        }

        // Test case source for checking the values of the application mode: 

        public static IEnumerable LogEventTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.LogEvent_Fail).Returns(false);
                yield return new TestCaseData(Behavior.LogEvent_Success).Returns(true);
            }
        }

        private ushort[] _data;
        

        [SetUp]
        public void Setup()
        {
            //this.connectCallbackCalled = true;
            //this.connectCompleted = true;
        }

        ushort _testValue = 0;
 
        // Test for reading: 
        [Test, TestCaseSource(typeof(ReadTestsModbus), "ReadTestCases")]
        public async Task<ushort> ReadTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxDevice = new WTXModbus(testConnection);

            _wtxDevice.Connect(this.OnConnect, 100);
          
            await Task.Run(async () =>
            {
                ushort[] _data = await testConnection.SyncData();
            });

            _testValue = (ushort)_wtxDevice.ProcessData.Weight.Net;
            return _testValue;
        }

        private void UpdateReadTest(object sender, ProcessDataReceivedEventArgs e)
        {
            _testValue = (ushort) e.ProcessData.Weight.Net;
        }

        /*
        // Test for checking the handshake bit 
        [Test, TestCaseSource(typeof(ReadTestsModbus), "HandshakeTestCases")]
        public bool testHandshake(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, "172.19.103.8");
            _wtxDevice = new WTXModbus(testConnection, 200,UpdateTestHandshake);

            _wtxDevice.Connect(this.OnConnect, 100);

            _wtxDevice.WriteSync(0, 0x1);

            return _wtxDevice.ProcessData.Handshake;
        }
        */

        private void UpdateTestHandshake(object sender, ProcessDataReceivedEventArgs e)
        {
        }

        [Test, TestCaseSource(typeof(ReadTestsModbus), "MeasureZeroTestCases")]
        public bool MeasureZeroTest(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxDevice = new WTXModbus(testConnection, 200,UpdateMeasureZeroTest);

            _wtxDevice.Connect(this.OnConnect, 100);

            _wtxDevice.AdjustZeroSignal();

            //check if : write reg 48, 0x7FFFFFFF and if Net and gross value are zero. 

            if ((testConnection.getArrElement1 == (0x7FFFFFFF & 0xffff0000) >> 16) &&
                (testConnection.getArrElement2 == (0x7FFFFFFF & 0x0000ffff)) &&
                _wtxDevice.ProcessData.Weight.Net == 0 && _wtxDevice.ProcessData.Weight.Gross == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateMeasureZeroTest(object sender, ProcessDataReceivedEventArgs e)
        {
        }
 
        [Test, TestCaseSource(typeof(ReadTestsModbus), "ApplicationModeTestCases")]
        public int ApplicationModeTest(Behavior behavior)
        {
            TestModbusTCPConnection testConnection = new TestModbusTCPConnection(behavior, ipaddress);

            WTXModbus _wtxDevice = new WTXModbus(testConnection, 200,UpdateApplicationModeTest);

            _wtxDevice.Connect(this.OnConnect, 100);

            testConnection.WriteInteger(ModbusCommands.ControlWordActivateData, 0);

            testConnection.Read(0);

            return testConnection.getData[5] & 0x3 >> 1;
        }

        private void UpdateApplicationModeTest(object sender, ProcessDataReceivedEventArgs e)
        {
        }
        
        [Test, TestCaseSource(typeof(ReadTestsModbus), "LogEventTestCases")]
        public async Task<bool> LogEventGetTest(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxDevice = new WTXModbus(testConnection, 200,UpdateLogEventGetTest);

            _wtxDevice.Connect(this.OnConnect, 100);
            testConnection.IsConnected = true;

            _data = await testConnection.SyncData();
            
            if (testConnection._logObj.Args.Equals("Read successful: Registers have been read"))
                return true;

            else
                if (testConnection._logObj.Args.Equals("Read failed : Registers have not been read"))
                return false;

            else
                return false; 
            //return _wtxDevice.ApplicationMode;
        }
        
        private void UpdateLogEventGetTest(object sender, ProcessDataReceivedEventArgs e)
        {
        }
        
        [Test, TestCaseSource(typeof(ReadTestsModbus), "LogEventTestCases")]
        public async Task<bool> LogEventSetTest(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxDevice = new WTXModbus(testConnection, 200, UpdateLogEventSetTest);

            _wtxDevice.Connect(this.OnConnect, 100);
            testConnection.IsConnected = true;

            _data = await testConnection.SyncData();

            if (testConnection._logObj.Args.Equals("Read successful: Registers have been read"))
                return true;

            else
                if (testConnection._logObj.Args.Equals("Read failed : Registers have not been read"))
                return false;

            else
                return false;
            //return _wtxDevice.ApplicationMode;
        }
        
        private void UpdateLogEventSetTest(object sender, ProcessDataReceivedEventArgs e)
        {
        }


        private void OnConnect(bool obj)
        {
            throw new NotImplementedException();
        }

    }
}
