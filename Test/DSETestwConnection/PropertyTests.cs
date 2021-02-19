// <copyright file="PropertyTests.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Test.DSETestwConnection
{
    using Hbm.Automation.Api.Weighing.DSE;
    using Hbm.Automation.Api.Weighing.DSE.Jet;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PropertyTests
    {
        private DSEJet _dse;
        private DSEJetConnection _connection;
        private string ipaddress = "192.168.178.46";

        [TestMethod]
        public void TestSerial()
        {
            _connection = new DSEJetConnection(ipaddress);
            _dse = new DSEJet(_connection, 500, null);
            _dse.Connect(200);
            Assert.AreEqual(true, _dse.SerialNumber.Length > 0);
            _dse.Disconnect();
        }

        [TestMethod]
        public void TestPropertySwitch()
        {
            _connection = new DSEJetConnection(ipaddress);
            _dse = new DSEJet(_connection, 500, null);
            _dse.Connect(200);
            System.Threading.Thread.Sleep(500);
            double tare = _dse.ManualTareValue;
            _dse.ManualTareValue = 0.040;
            System.Threading.Thread.Sleep(500);
            Assert.AreNotEqual(tare, _dse.ManualTareValue);
            _dse.ManualTareValue = tare;
            _dse.Disconnect();
        }

        [TestMethod]
        public void readProperties()
        {
            _connection = new DSEJetConnection(ipaddress);
            _dse = new DSEJet(_connection, 500, null);
            _dse.Connect(200);
            System.Threading.Thread.Sleep(500);
            foreach(var prop in _dse.GetType().GetProperties())
            {
                if (prop.ToString().Equals("Int32 LowPasCutOffFrequency") || prop.ToString().Equals("Int32 LowPassFilterOrder")) continue;
                Assert.AreNotEqual(null, prop.GetValue(_dse));
            }
            _dse.Disconnect();
        }
    }
}
