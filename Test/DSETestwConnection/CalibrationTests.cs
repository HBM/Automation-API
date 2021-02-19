using Hbm.Weighing.Api;
using Hbm.Weighing.Api.Data;
using Hbm.Weighing.Api.WTX;
using Hbm.Weighing.Api.WTX.Jet;

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetbusTest
{
    [TestFixture]
    public class CalibrationTests
    {
        private INetConnection _jetTestConnection;
        private WTXJet _wtxObj;
        private string ipaddress = "wss://172.20.41.120:443/jet/canopen";

        // Test case source for writing values to the WTX120 device: Taring 
        public static IEnumerable CalibrationTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.CalibrationFail).Returns(false);
                yield return new TestCaseData(Behavior.CalibrationSuccess).Returns(true);
            }
        }

        
        // Test case source for writing values to the WTX120 device: Taring 
        public static IEnumerable CalibrationPreloadCapacityTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.CalibratePreloadCapacityFail).Returns(false);
                yield return new TestCaseData(Behavior.CalibratePreloadCapacitySuccess).Returns(true);
            }
        }

        // Test case source for writing values to the WTX120 device: Taring 
        public static IEnumerable MeasureZeroTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.MeasureZeroFail).Returns(false);
                yield return new TestCaseData(Behavior.MeasureZeroSuccess).Returns(true);
            }
        }

        [SetUp]
        public void Setup()
        {
            //testGrossValue = 0;
        }
   
        [Test, TestCaseSource(typeof(CalibrationTests), "CalibrationTestCases")]
        public bool CalibrationTest(Behavior behavior)
        {

            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);
           
            _wtxObj.AdjustNominalSignalWithCalibrationWeight(1.5);
            
            if (
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.CIA461CalibrationWeight) == 15000 &&       // LFT_SCALE_CALIBRATION_WEIGHT = "6152/00" 
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.CIA461ScaleCommand) == 1852596579          // CALIBRATE_NOMINAL_WEIGHT = 1852596579 // SCALE_COMMAND = "6002/01"
                )

                return true;

            else
                return false;

        }

        private void Update(object sender, ProcessDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        
        [Test, TestCaseSource(typeof(CalibrationTests), "MeasureZeroTestCases")]
        public bool MeasureZeroTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 200,Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.AdjustZeroSignal();
            
            if (
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.CIA461ScaleCommand) == 2053923171  &&
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.CIA461GrossValue) == 0 &&
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.CIA461NetValue) == 0
                )
                return true;

            else
                return false;
        }
        
        [Test, TestCaseSource(typeof(CalibrationTests), "CalibrationPreloadCapacityTestCases")]
        public bool CalibrationPreloadCapacityTest(Behavior behavior)
        {
            double preload = 1;
            double capacity = 2;

            double testdPreload = 0;
            double testdNominalLoad = 0;
            int testIntPreload = 0;
            int testIntNominalLoad = 0;

            double multiplierMv2D = 500000; //   2 / 1000000; // 2mV/V correspond 1 million digits (d)

            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection,200,Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.CalculateAdjustment(preload, capacity);

            testdPreload = preload * multiplierMv2D;
            testdNominalLoad = testdPreload + (capacity * multiplierMv2D);

            testIntPreload = Convert.ToInt32(testdPreload);
            testIntNominalLoad = Convert.ToInt32(testdNominalLoad);

            if (
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.LDWZeroValue) == testIntPreload &&
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.LWTNominalValue) == testIntNominalLoad
                )

                return true;

            else
                return false;
        }
        
        private void OnConnect(bool obj)
        {
            //Callback, do something ... 
        }
        
    }
}
