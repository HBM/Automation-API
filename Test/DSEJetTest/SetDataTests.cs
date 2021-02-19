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

namespace Hbm.Automation.Api.Test.DSEJetTest
{
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.DSE;
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

        
        /**[Test, TestCaseSource(typeof(SetDataTests), "setTests")]
        public bool SetActivationTimeTest(Behavior behavior)
        {
            TestJetbusConnection _jetTestConnection = new TestJetbusConnection(behavior, "wss://172.19.103.8:443/jet/canopen", "Administrator", "wtx", delegate { return true; });

            DSEJet _dseObj = new DSEJet(_jetTestConnection, 200, update);

            _dseObj.Connect(this.OnConnect, 100);

            
            _dseObj.LimitSwitch.LimitSwitch1Source = LimitSwitchSource.Gross;
            _dseObj.LimitSwitch.LimitSwitch2Source = LimitSwitchSource.Net;
            _dseObj.LimitSwitch.LimitSwitch3Source = LimitSwitchSource.Gross;
            _dseObj.LimitSwitch.LimitSwitch4Source = LimitSwitchSource.Net;

            ((IDataFillerExtended)_dseObj.Filler).CoarseFlowMonitoring = 10;
            ((IDataFillerExtended)_dseObj.Filler).CoarseFlowMonitoringTime = 101;
            ((IDataFillerExtended)_dseObj.Filler).CoarseLockoutTime = 110;
            ((IDataFillerExtended)_dseObj.Filler).EmptyingMode = 1000;
            ((IDataFillerExtended)_dseObj.Filler).EmptyWeight = 1001;
            ((IDataFillerExtended)_dseObj.Filler).FineFlowMonitoring = 1011;
            ((IDataFillerExtended)_dseObj.Filler).FineFlowMonitoringTime = 1100;
            ((IDataFillerExtended)_dseObj.Filler).FineLockoutTime = 1111;

            ((IDataFillerExtended)_dseObj.Filler).LowerToleranceLimit = 11000;
            ((IDataFillerExtended)_dseObj.Filler).MinimumFineFlow = 11001;
            ((IDataFillerExtended)_dseObj.Filler).MinimumStartWeight = 11010;
            ((IDataFillerExtended)_dseObj.Filler).ResidualFlowTime = 11101;
            ((IDataFillerExtended)_dseObj.Filler).SystematicDifference = 11110;
            ((IDataFillerExtended)_dseObj.Filler).TareDelay = 11111;
            ((IDataFillerExtended)_dseObj.Filler).TareMode = 100000;
            ((IDataFillerExtended)_dseObj.Filler).UpperToleranceLimit = 100001;
            ((IDataFillerExtended)_dseObj.Filler).ValveControl = 100010;           

            if (_jetTestConnection.getDataBuffer.ContainsKey("VCT") && _jetTestConnection.getDataBuffer.ContainsValue(100010))           
                return true;
            else
                if (_jetTestConnection.getDataBuffer.ContainsKey("6002/02") && _jetTestConnection.getDataBuffer.ContainsValue(1230))
                    return false;

            return false;
        }**/

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