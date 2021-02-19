// <copyright file="SetDataTests.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.WTX;
    using NUnit.Framework;
    using System;
    using System.Collections;

    // Class for testing the set properties the class WTXJet:
    [TestFixture]
    public class SetDataTests
    {

        // Test case source for writing values to the WTX120 device : Zeroing
        public static IEnumerable setTests
        {
            get
            {
                yield return new TestCaseData(Behavior.setTestsFail).Returns(false);
                yield return new TestCaseData(Behavior.setTestsSuccess).Returns(true);
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        
        [Test, TestCaseSource(typeof(SetDataTests), "setTests")]
        public bool SetActivationTimeTest(Behavior behavior)
        {
            TestJetbusConnection _jetTestConnection = new TestJetbusConnection(behavior, "wss://172.19.103.8:443/jet/canopen", "Administrator", "wtx", delegate { return true; });

            WTXJet _wtxObj = new WTXJet(_jetTestConnection, 200, update);

            _wtxObj.Connect(this.OnConnect, 100);

            
            _wtxObj.LimitSwitch.LimitSwitch1Source = LimitSwitchSource.Gross;
            _wtxObj.LimitSwitch.LimitSwitch2Source = LimitSwitchSource.Net;
            _wtxObj.LimitSwitch.LimitSwitch3Source = LimitSwitchSource.Gross;
            _wtxObj.LimitSwitch.LimitSwitch4Source = LimitSwitchSource.Net;

            ((IDataFillerExtended)_wtxObj.Filler).CoarseFlowMonitoring = 10;
            ((IDataFillerExtended)_wtxObj.Filler).CoarseFlowMonitoringTime = 101;
            ((IDataFillerExtended)_wtxObj.Filler).CoarseLockoutTime = 110;
            ((IDataFillerExtended)_wtxObj.Filler).EmptyingMode = 1000;
            ((IDataFillerExtended)_wtxObj.Filler).EmptyWeight = 1001;
            ((IDataFillerExtended)_wtxObj.Filler).FineFlowMonitoring = 1011;
            ((IDataFillerExtended)_wtxObj.Filler).FineFlowMonitoringTime = 1100;
            ((IDataFillerExtended)_wtxObj.Filler).FineLockoutTime = 1111;

            ((IDataFillerExtended)_wtxObj.Filler).LowerToleranceLimit = 11000;
            ((IDataFillerExtended)_wtxObj.Filler).MinimumFineFlow = 11001;
            ((IDataFillerExtended)_wtxObj.Filler).MinimumStartWeight = 11010;
            ((IDataFillerExtended)_wtxObj.Filler).ResidualFlowTime = 11101;
            ((IDataFillerExtended)_wtxObj.Filler).SystematicDifference = 11110;
            ((IDataFillerExtended)_wtxObj.Filler).TareDelay = 11111;
            ((IDataFillerExtended)_wtxObj.Filler).TareMode = 100000;
            ((IDataFillerExtended)_wtxObj.Filler).UpperToleranceLimit = 100001;
            ((IDataFillerExtended)_wtxObj.Filler).ValveControl = 100010;           

            if (_jetTestConnection.getDataBuffer.ContainsKey("VCT") && _jetTestConnection.getDataBuffer.ContainsValue(100010))           
                return true;
            else
                if (_jetTestConnection.getDataBuffer.ContainsKey("6002/02") && _jetTestConnection.getDataBuffer.ContainsValue(1230))
                    return false;

            return false;
        }

        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnConnect(bool obj)
        {
            throw new NotImplementedException();
        }
        
    }
}