// <copyright file="WriteTests.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.DSE;
    using NUnit.Framework;
    using System;
    using System.Collections;

    // Class for testing write functions of JetBusConnection, f.e. 'Write(path,data)' and 
    // 'WriteInt(object index)' and so on.
    [TestFixture]
    public class WriteTests
    {
        //private TestJetbusConnection _jetTestConnection;
        //private WTXJet _wtxObj;
        //private int testGrossValue;
        private string ipaddress = "wss://172.20.41.120:443/jet/canopen";


        // Test case source for writing values to the WTX120 device: Taring 
        public static IEnumerable WriteTareTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteTareFail).Returns(false);
                yield return new TestCaseData(Behavior.WriteTareSuccess).Returns(true);
            }
        }

        // Test case source for writing values to the WTX120 device: Set to gross
        public static IEnumerable WriteGrossTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteGrossFail).Returns(false);
                yield return new TestCaseData(Behavior.WriteGrossSuccess).Returns(true);
            }
        }

        // Test case source for writing values to the WTX120 device : Zeroing
        public static IEnumerable WriteZeroTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteZeroFail).Returns(false);
                yield return new TestCaseData(Behavior.WriteZeroSuccess).Returns(true);
            }
        }

        public static IEnumerable WriteUnitTest
        {
            get
            {
                yield return new TestCaseData(Behavior.writeUnitTest);
            }
        }

        [SetUp]
        public void Setup()
        {
            //testGrossValue = 0;
        }

        private TestJetbusConnection _jetTestConnection;
        private DSEJet _dseJet;

        [Test, TestCaseSource(typeof(WriteTests), "WriteTareTestCases")]
        public bool WriteTareTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseJet = new DSEJet(_jetTestConnection, 200, update);

            _dseJet.Connect(this.OnConnect, 100);
          
            _dseJet.Tare();     // Write in index(address) "6002/01" value 1701994868

            if (_jetTestConnection.getDataBuffer.ContainsKey("6002/01") && _jetTestConnection.getDataBuffer.ContainsValue(1701994868))
                return true;

            else
                return false;

        }

        [Test, TestCaseSource(typeof(WriteTests), "WriteUnitTest")]
        public void writeUnitTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseJet = new DSEJet(_jetTestConnection, 200, update);

            _dseJet.Connect(this.OnConnect, 100);

            string[] units = { "kg", "g", "lb", "t", "N"};

            foreach(string unit in units)
            {
                _dseJet.Unit = unit;
                _dseJet.ProcessData.UpdateData(this, new EventArgs());
                Assert.AreEqual(unit, _dseJet.Unit);
            }

        }

        [Test, TestCaseSource(typeof(WriteTests), "WriteGrossTestCases")]
        public bool WriteGrossTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseJet = new DSEJet(_jetTestConnection, 200, update);

            _dseJet.Connect(this.OnConnect, 100);

            _dseJet.SetGross();     // Write in index(address) "6002/01" value 1936683623

            if (_jetTestConnection.getDataBuffer.ContainsKey("6002/01") && _jetTestConnection.getDataBuffer.ContainsValue(1936683623))
                return true;

            else
                return false;

        }

        [Test, TestCaseSource(typeof(WriteTests), "WriteZeroTestCases")]
        public bool WriteZeroTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseJet = new DSEJet(_jetTestConnection, 200, update);

            _dseJet.Connect(this.OnConnect, 100);

            _dseJet.Zero();     // Write in index(address) "6002/01" value 1869768058);

            if (_jetTestConnection.getDataBuffer.ContainsKey("6002/01") && _jetTestConnection.getDataBuffer.ContainsValue(1869768058)) 
                return true;

            else
                return false;          

        }

        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
        }

        private void OnConnect(bool obj)
        {
        }

    }
}

