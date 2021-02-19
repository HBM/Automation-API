// <copyright file="WriteTestsModbus.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

    [TestFixture]
    public class WriteTestsModbus
    {

        private TestModbusTCPConnection testConnection;
        private WTXModbus _wtxObj;
        private string ipaddress = "172.19.103.8";

        //private bool connectCallbackCalled;
        //private bool connectCompleted;

        //private bool disconnectCallbackCalled;
        //private bool disconnectCompleted;

        // Test case source for writing values to the WTX120 device. 
        public static IEnumerable WriteTestCases
        {
            get
            {
                //yield return new TestCaseData(Behavior.WriteFail).Returns(0x2);
                yield return new TestCaseData(Behavior.WriteSuccess).Returns(0);
            }
        }

        public static IEnumerable WriteArrayTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteArrayFail).Returns(false);
                yield return new TestCaseData(Behavior.WriteArraySuccess).Returns(true);
            }
        }

        // Test case source for writing values to the WTX120 device. 
        public static IEnumerable WriteSyncTestModbus
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteSyncSuccess).Returns(0x100);
                yield return new TestCaseData(Behavior.WriteSyncFail).Returns(0);
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

        public static IEnumerable AsyncWriteBackgroundworkerCase
        {
            get
            {
                yield return new TestCaseData(Behavior.AsyncWriteBackgroundworkerSuccess).Returns(true);
                yield return new TestCaseData(Behavior.AsyncWriteBackgroundworkerFail).Returns(true);
            }
        }

        // Test case source for writing values to the WTX120 device. 
        public static IEnumerable TareTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.TareFail).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.TareSuccess).ExpectedResult = 0x1;
            }
        }

        public static IEnumerable WriteHandshakeTestModbus
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteHandshakeTestSuccess).Returns(0x1);
                yield return new TestCaseData(Behavior.WriteHandshakeTestFail).Returns(0x0);
            }
        }

        public static IEnumerable GrosMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.GrosMethodTestSuccess).ExpectedResult = (0x2);
                yield return new TestCaseData(Behavior.GrosMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable TareMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.TareMethodTestSuccess).ExpectedResult = (0x1);
                yield return new TestCaseData(Behavior.TareMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable ZeroMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.ZeroMethodTestSuccess).ExpectedResult=(0x40);
                yield return new TestCaseData(Behavior.ZeroMethodTestFail).ExpectedResult=(0x0);
            }
        }

        public static IEnumerable AdjustingZeroMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.AdjustingZeroMethodSuccess).ExpectedResult = (0x80);
                yield return new TestCaseData(Behavior.AdjustingZeroMethodFail).ExpectedResult = (0x0);
            }
        }

        public static IEnumerable AdjustNominalMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.AdjustNominalMethodTestSuccess).ExpectedResult = (0x100);
                yield return new TestCaseData(Behavior.AdjustNominalMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable ActivateDataMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.ActivateDataMethodTestSuccess).ExpectedResult = (0x800);
                yield return new TestCaseData(Behavior.ActivateDataMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable ManualTaringMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.ManualTaringMethodTestSuccess).ExpectedResult = (0x1000);
                yield return new TestCaseData(Behavior.ManualTaringMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable ClearDosingResultsMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.ClearDosingResultsMethodTestSuccess).ExpectedResult = (0x4);
                yield return new TestCaseData(Behavior.ClearDosingResultsMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable AbortDosingMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.AbortDosingMethodTestSuccess).ExpectedResult = (0x8);
                yield return new TestCaseData(Behavior.AbortDosingMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable StartDosingMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.StartDosingMethodTestSuccess).ExpectedResult = (0x10);
                yield return new TestCaseData(Behavior.StartDosingMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable RecordWeightMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.RecordWeightMethodTestSuccess).ExpectedResult = (0x4000);
                yield return new TestCaseData(Behavior.RecordWeightMethodTestFail).ExpectedResult = (0x0);
            }
        }
        public static IEnumerable ManualRedosingMethodTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.ManualRedosingMethodTestSuccess).ExpectedResult = (0x8000);
                yield return new TestCaseData(Behavior.ManualRedosingMethodTestFail).ExpectedResult = (0x0);
            }
        }

        public static IEnumerable WriteS32ArrayTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteS32ArrayTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.WriteS32ArrayTestFail).Returns(false);
            }
        }


        public static IEnumerable WriteU16ArrayTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteU16ArrayTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.WriteU16ArrayTestFail).Returns(false);
            }
        }

        public static IEnumerable WriteU08ArrayTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteU08ArrayTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.WriteU08ArrayTestFail).Returns(false);
            }
        }

        public static IEnumerable ResetTimerTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.ResetTimerTestSuccess).Returns(500);
                //yield return new TestCaseData(Behavior.ResetTimerTestFail).Returns(200);
            }
        }

        public static IEnumerable UpdateOutputTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.UpdateOutputTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.UpdateOutputTestFail).Returns(false);
            }
        }

        public static IEnumerable WriteLimitSwitch1ModeTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteLimitSwitch1ModeTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.WriteLimitSwitch1ModeTestFail).Returns(false);
            }
        }

        public static IEnumerable WriteLimitSwitch2ModeTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteLimitSwitch2ModeTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.WriteLimitSwitch2ModeTestFail).Returns(false);
            }
        }
        public static IEnumerable WriteLimitSwitch3ModeTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteLimitSwitch3ModeTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.WriteLimitSwitch3ModeTestFail).Returns(false);
            }
        }
        public static IEnumerable WriteLimitSwitch4ModeTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteLimitSwitch4ModeTestSuccess).Returns(true);
                yield return new TestCaseData(Behavior.WriteLimitSwitch4ModeTestFail).Returns(false);
            }
        }


        [SetUp]
        public void Setup()
        {
            //this.connectCallbackCalled = true;
            //this.connectCompleted = true;
        }

        // Tests for writing limit values : 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteLimitSwitch1ModeTestCases")]
        public bool LimitSwitch1ModeWriteTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.LimitSwitch.LimitSwitch1Mode = LimitSwitchMode.AboveLevel;

            if (testConnection.GetCommand == 4 && _wtxObj.LimitSwitch.LimitSwitch1Mode == LimitSwitchMode.AboveLevel)
                return true;

            else
                return false;
        }

        // Tests for writing limit values : 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteLimitSwitch2ModeTestCases")]
        public bool LimitSwitch2ModeWriteTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);
            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.LimitSwitch.LimitSwitch2Mode = LimitSwitchMode.BelowLevel;

            if (testConnection.GetCommand == 11 && _wtxObj.LimitSwitch.LimitSwitch2Mode == LimitSwitchMode.BelowLevel)
                return true;

            else
                return false;
        }

        // Tests for writing limit values : 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteLimitSwitch3ModeTestCases")]
        public bool LimitSwitch3ModeWriteTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);
            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.LimitSwitch.LimitSwitch3Mode = LimitSwitchMode.InsideBand;

            if (testConnection.GetCommand == 17 && _wtxObj.LimitSwitch.LimitSwitch3Mode == LimitSwitchMode.InsideBand)
                return true;

            else
                return false;
        }

        // Tests for writing limit values : 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteLimitSwitch4ModeTestCases")]
        public bool LimitSwitch4ModeWriteTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);
            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.LimitSwitch.LimitSwitch4Mode = LimitSwitchMode.OutsideBand;

            if (testConnection.GetCommand == 23 && _wtxObj.LimitSwitch.LimitSwitch4Mode == LimitSwitchMode.OutsideBand)
                return true;
            else
                return false;
        }

        // Test for method : Zeroing
        [Test, TestCaseSource(typeof(WriteTestsModbus), "ZeroMethodTestCases")]
        public void ZeroMethodTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.Zero();

            if (behavior == Behavior.ZeroMethodTestSuccess)
                Assert.AreEqual(0x40, testConnection.GetCommand);
            else
                if (behavior == Behavior.ZeroMethodTestFail)
                Assert.AreEqual(0x00, testConnection.GetCommand);
          
        }
        /*
        // Test for synchronous writing : Tare 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteSyncTestModbus")]
        public int WriteSyncTest(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, "172.19.103.8");

            _wtxObj = new WtxModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.WriteSync(0, 0x100);

            return testConnection.GetCommand;
        }
        */
        /*
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteArrayTestCases")]
        public bool WriteArrayTestCasesModbus(Behavior behavior)
        {
            bool parameterEqualArrayWritten = false;

            TestModbusTCPConnection testConnection = new TestModbusTCPConnection(behavior, "172.19.103.8");
            WtxModbus _wtxObj = new WtxModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.WriteOutputWordS32(0x7FFFFFFF, 50);

            if ((testConnection.getArrElement1 == (0x7FFFFFFF & 0xffff0000) >> 16) &&
                (testConnection.getArrElement2 == (0x7FFFFFFF & 0x0000ffff)))
            {
                parameterEqualArrayWritten = true;
            }
            else
            {
                parameterEqualArrayWritten = false;
            }

            //Assert.AreEqual(true ,parameterEqualArrayWritten);

            return parameterEqualArrayWritten;
        }
        */


        // Test for writing : Tare 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "TareTestCases")]
        public void TareAsyncTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.Tare();

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x1, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);

        }

        private void Update(object sender, ProcessDataReceivedEventArgs e)
        {

        }

        private void OnConnect(bool obj)
        {
            throw new NotImplementedException();
        }

        // Test for method : Switch to gross value or net value
        [Test, TestCaseSource(typeof(WriteTestsModbus), "GrosMethodTestCases")]
        public void GrosMethodTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.SetGross();

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x2, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);
        }

        // Test for method : Taring
        [Test, TestCaseSource(typeof(WriteTestsModbus), "TareMethodTestCases")]
        public void TareMethodTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.Tare();

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x1, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);

        }
        
        // Test for method : Adjusting zero
        [Test, TestCaseSource(typeof(WriteTestsModbus), "AdjustingZeroMethodTestCases")]
        public void AdjustingZeroMethodTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.AdjustZeroSignal();

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x80, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);
        }
        
        // Test for method : Adjusting nominal
        [Test, TestCaseSource(typeof(WriteTestsModbus), "AdjustNominalMethodTestCases")]
        public void AdjustingNominalMethodTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.AdjustNominalSignal();

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x100, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);
        }

        // Test for method : Adjusting nominal
        [Test, TestCaseSource(typeof(WriteTestsModbus), "ActivateDataMethodTestCases")]
        public void /*int*/ ActivateDataMethodTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.ActivateData();

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x800, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);
        }

        // Test for method : Adjusting nominal
        [Test, TestCaseSource(typeof(WriteTestsModbus), "ManualTaringMethodTestCases")]
        public void ManualTaringTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.TareManually(200.0); 

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x1000, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);
        }

        // Test for method : Record weight
        [Test, TestCaseSource(typeof(WriteTestsModbus), "RecordWeightMethodTestCases")]
        public void RecordweightMethodTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.RecordWeight();

            if (behavior == Behavior.TareMethodTestSuccess)
                Assert.AreEqual(0x4000, testConnection.GetCommand);
            else
                if (behavior == Behavior.TareMethodTestFail)
                Assert.AreEqual(0x0, testConnection.GetCommand);
        }
        
        // Test for method : Write an Array of type signed integer 32 bit. 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteS32ArrayTestCases")]
        public bool WriteS32ArrayTestModbus(Behavior behavior)
        {
            ushort[] _data = new ushort[2];

            _data[0] = (ushort)((0x7FFFFFFF & 0xFFFF0000) >> 16);
            _data[1] = (ushort)(0x7FFFFFFF & 0x0000FFFF);

            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);
           
            _wtxObj.Connection.WriteInteger(ModbusCommands.LDWZeroSignal, 0x7FFFFFFF);

            if (testConnection.getArrElement1 == _data[0] && testConnection.getArrElement2 == _data[1] && testConnection.getWordNumber == 48)
                return true;
            else
                return false;
        }
              
        // Test for method : Write an Array of type unsigned integer 16 bit. 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteU16ArrayTestCases")]
        public bool WriteU16ArrayTestModbus(Behavior behavior)
        {
            ushort[] _data = new ushort[2];

            _data[0] = (ushort)((0x7fffffff & 0xffff0000) >> 16);
            _data[1] = (ushort)(0x7fffffff & 0x0000ffff);

            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.Connection.WriteInteger(ModbusCommands.LWTNominalSignal, 0x7FFFFFFF);
            
            if (testConnection.getArrElement1 == _data[0] && testConnection.getArrElement2 == _data[1] && testConnection.getWordNumber == 50)
                return true;
            else
                return false;
        }
        
        
        // Test for method : Write an Array of type unsigned integer 16 bit. 
        [Test, TestCaseSource(typeof(WriteTestsModbus), "WriteU08ArrayTestCases")]
        public bool WriteU08ArrayTestModbus(Behavior behavior)
        {
            ushort[] _data = new ushort[1];

            _data[0] = (ushort)(0xA1 & 0xFF);

            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.Connection.WriteInteger(ModbusCommands.LIV1LimitSwitchSource, 0xA1);
            
            if (testConnection.getArrElement1 == _data[0] && testConnection.getWordNumber == 4)
                return true;
            else
                return false;

        }

        [Test, TestCaseSource(typeof(WriteTestsModbus), "ResetTimerTestCases")]
        public int ResetTimerTestModbus(Behavior behavior)
        {
            testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            _wtxObj = new WTXModbus(testConnection, 200, Update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.ProcessDataInterval = 500;

            return (int)_wtxObj.ProcessDataInterval;
            //Assert.AreEqual(_wtxObj._aTimer.Interval, 500);
        }

        private bool testGetOutputwords()
        {
            if
             (
            _wtxObj.ManualTareValue == 1 && 
            _wtxObj.LimitSwitch.LimitSwitch1Source == LimitSwitchSource.Gross && 
            _wtxObj.LimitSwitch.LimitSwitch1Mode == LimitSwitchMode.AboveLevel && 
            _wtxObj.LimitSwitch.LimitSwitch1Level == 1 && 
            _wtxObj.LimitSwitch.LimitSwitch1Hysteresis == 1 &&
            _wtxObj.LimitSwitch.LimitSwitch2Source == LimitSwitchSource.Gross &&
            _wtxObj.LimitSwitch.LimitSwitch2Mode == LimitSwitchMode.AboveLevel &&
            _wtxObj.LimitSwitch.LimitSwitch2Level == 1 && 
            _wtxObj.LimitSwitch.LimitSwitch2Hysteresis == 1 && 
            _wtxObj.LimitSwitch.LimitSwitch3Source == LimitSwitchSource.Gross &&
            _wtxObj.LimitSwitch.LimitSwitch3Mode == LimitSwitchMode.AboveLevel &&
            _wtxObj.LimitSwitch.LimitSwitch3Level == 1 && 
            _wtxObj.LimitSwitch.LimitSwitch3Hysteresis == 1 && 
            _wtxObj.LimitSwitch.LimitSwitch4Source == LimitSwitchSource.Gross &&
            _wtxObj.LimitSwitch.LimitSwitch4Mode == LimitSwitchMode.AboveLevel &&
            _wtxObj.LimitSwitch.LimitSwitch4Level == 1 && 
            _wtxObj.LimitSwitch.LimitSwitch4Hysteresis == 1 && 
            _wtxObj.Filler.ResidualFlowTime == 1 && 
            _wtxObj.Filler.TargetFillingWeight == 1 && 
            _wtxObj.Filler.EmptyingMode == 1 &&
            _wtxObj.Filler.CoarseFlowCutOffLevel == 1 && 
            _wtxObj.Filler.FineFlowCutOffLevel == 1 && 
            _wtxObj.Filler.MinimumFineFlow == 1 && 
            _wtxObj.Filler.OptimizationMode == 1 && 
            _wtxObj.Filler.MaxFillingTime == 1 && 
            _wtxObj.Filler.ValveControl == 1 &&
            _wtxObj.Filler.StartWithFineFlow == 1 && 
            _wtxObj.Filler.CoarseLockoutTime == 1 && 
            _wtxObj.Filler.FineLockoutTime == 1 && 
            _wtxObj.Filler.TareMode == 1 && 
            _wtxObj.Filler.UpperToleranceLimit == 1 && 
            _wtxObj.Filler.LowerToleranceLimit == 1 &&
            _wtxObj.Filler.MinimumStartWeight == 1 && 
            _wtxObj.Filler.TareDelay == 1 && 
            _wtxObj.Filler.CoarseFlowMonitoringTime == 1 && 
            _wtxObj.Filler.CoarseFlowMonitoring == 1 && 
            _wtxObj.Filler.FineFlowMonitoring == 1 && 
            _wtxObj.Filler.EmptyWeight == 1 &&
            _wtxObj.Filler.FineFlowMonitoringTime == 1 && 
            _wtxObj.Filler.DelayTimeAfterFilling == 1 && 
            _wtxObj.Filler.ActivationTimeAfterFilling == 1 && 
            _wtxObj.Filler.SystematicDifference == 1 && 
            _wtxObj.Filler.FillingMode == 1
            )
                return true;

            else
                return false;
        }

    }
}