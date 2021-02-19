
using Hbm.Weighing.Api;
using Hbm.Weighing.Api.Data;
using Hbm.Weighing.Api.WTX;
using Hbm.Weighing.Api.WTX.Jet;

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetbusTest
{
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