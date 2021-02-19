// <copyright file="CalibrationTests.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using Hbm.Automation.Api;
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.DSE;
    using Hbm.Automation.Api.Weighing.WTX.Jet;
    using NUnit.Framework;
    using System;
    using System.Collections;

    [TestFixture]
    public class CalibrationTests
    {
        private INetConnection _jetTestConnection;
        private DSEJet _dseObj;
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

            _dseObj = new DSEJet(_jetTestConnection, 200, Update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.AdjustNominalSignalWithCalibrationWeight(1.5);

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

            _dseObj = new DSEJet(_jetTestConnection, 200, Update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.AdjustZeroSignal();

            if (
                _jetTestConnection.ReadIntegerFromBuffer(JetBusCommands.CIA461ScaleCommand) == 2053923171 &&
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

            _dseObj = new DSEJet(_jetTestConnection, 200, Update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.CalculateAdjustment(preload, capacity);

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
