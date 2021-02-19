// <copyright file="ReadTests.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

    // Class for testing read functions of JetBusConnection, like 'OnFetchData(JToken data)' and 
    // 'JToken ReadObj(object index)'.
    // In class JetBusConnection at #region read-functions:
    [TestFixture]
    public class ReadTests
    {
        private TestJetbusConnection _jetTestConnection;
        private WTXJet _wtxObj;
        private int _testInteger;
        private double _testDouble;
        private bool _testBoolean;
        private string ipaddress = "wss://172.20.41.120:443/jet/canopen";


        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossTareValuesTest
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossTareValues_Fail).Returns(false);
                yield return new TestCaseData(Behavior.NetGrossTareValues_Success).Returns(true);
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_WEIGHING_DEVICE_1_WEIGHT_STATUS).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.ReadSuccess_WEIGHING_DEVICE_1_WEIGHT_STATUS).ExpectedResult = 1;
            }
        }

        // Test case source for reading values from the WTX120 device : Decimals 
        public static IEnumerable ReadTestCases_Decimals
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_Decimals).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.ReadSuccess_Decimals).ExpectedResult = 1;
            }
        }


        // Test case source for reading values from the WTX120 device : Filling process status 
        public static IEnumerable ReadTestCases_FillingProcessSatus
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_FillingProcessSatus).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.ReadSuccess_FillingProcessSatus).ExpectedResult = 1;
            }
        }

        // Test case source for reading values from the WTX120 device : Dosing result 
        public static IEnumerable ReadTestCases_DosingResult
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_DosingResult).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.ReadSuccess_DosingResult).ExpectedResult = 1;
            }
        }

        // Test case source for reading values from the WTX120 device : Number of dosing results 
        public static IEnumerable ReadTestCases_NumberDosingResults
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_NumberDosingResults).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.ReadSuccess_NumberDosingResults).ExpectedResult = 1;
            }
        }


        // Test case source for reading values from the WTX120 device : Unit or prefix or fixed parameters 
        public static IEnumerable ReadTestCases_Unit
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_Unit).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.ReadSuccess_Unit).ExpectedResult = 1;
            }
        }

        // Test case source for reading values from the WTX120 device : Unit or prefix or fixed parameters 
        public static IEnumerable ReadTestCases_Attributes
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_Attributes).ExpectedResult = 0;
                yield return new TestCaseData(Behavior.ReadSuccess_Attributes).ExpectedResult = 1;
            }
        }

        // Test case source for reading values from the WTX120 device : Have data been reveiceid? (Property 'isDataReceived') 
        public static IEnumerable ReadTestCases_DataReceived
        {
            get
            {

                yield return new TestCaseData(Behavior.ReadFail_DataReceived).Returns(false);
                yield return new TestCaseData(Behavior.ReadSuccess_DataReceived).Returns(true);
            }
        }

        [SetUp]
        public void Setup()
        {
            _testInteger = 0;
            _testBoolean = false;
        }
      
        
        [Test, TestCaseSource(typeof(ReadTests), "NetGrossTareValuesTest")]
        public bool testNetGrossTareValues(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);
            
            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.ProcessData.UpdateData(this, new EventArgs());

            if (_wtxObj.ProcessData.Weight.Net == 0.0011 && _wtxObj.ProcessData.Weight.Tare == 0.0011 && _wtxObj.ProcessData.Weight.Gross == 0)
                return true;
            else
                return false;
        }
              
        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightMovingValue(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.WeightStable;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }
     
        private void OnConnect(bool obj)
        {
            throw new NotImplementedException();
        }
        
        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testGeneralWeightError(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.GeneralScaleError;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testScaleAlarmTriggered(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.ScaleAlarm;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testOverload(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.Overload;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testScaleSealIsOpen(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.LegalForTrade;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightType(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.TareMode!=TareMode.None;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testScaleRange(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100,  update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = _wtxObj.ProcessData.ScaleRange;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testZeroRequired(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.ZeroRequired;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightWithinTheCenterOfZero(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.CenterOfZero;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightInZeroRange(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.ProcessData.InsideZero;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }



        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Decimals")]
        public void testDecimals(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = _wtxObj.ProcessData.Decimals;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6013/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_FillingProcessSatus")]
        public void testFillingProcessStatus(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).FillingProcessStatus;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("SDO"));
        }
        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_DosingResult")]
        public void testDosingResult(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).FillingResult;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("FRS1"));
        }


        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_NumberDosingResults")]
        public void testNumberDosingResults(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).FillingResultCount;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("NDS"));
        }

        /*
        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Unit")]
        public void testUnit(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, "wss://172.19.103.8:443/jet/canopen", "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testString = _wtxObj.ProcessData.Unit;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6014/01"));
        }
        */

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testInput1(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Input1;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("IM1"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testInput2(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Input2;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("IM2"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testInput3(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Input3;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("IM3"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testInput4(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Input4;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("IM4"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testOutput1(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Output1;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OM1"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testOutput2(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Output2;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OM2"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testOutput3(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Output3;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OM3"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testOutput4(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.DigitalIO.Output4;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OM4"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitStatus1(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.LimitSwitch.LimitStatus1;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OS1"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitStatus2(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.LimitSwitch.LimitStatus2;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OS2"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitStatus3(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.LimitSwitch.LimitStatus3;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OS3"));
        }


        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitStatus4(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testBoolean = _wtxObj.LimitSwitch.LimitStatus4;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OS4"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testMaxDosingTime(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).MaxFillingTime;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("MDT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testMeanValueDosingResults(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).FillingResultMeanValue;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("SDM"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testStandardDeviation(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).FillingResultStandardDeviation;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("SDS"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testFineFlowCutOffPoint(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).FineFlowCutOffLevel;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("FFD"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testCoarseFlowCutOffPoint(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).CoarseFlowCutOffLevel;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("CFD"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testResidualFlowTime(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).ResidualFlowTime;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("RFT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testMinimumFineFlow(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).MinimumFineFlow;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("FFM"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testOptimizationOfCutOffPoints(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).OptimizationMode;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("OSN"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testMaximumDosingTime(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).MaxFillingTime;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("MDT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testCoarseLockoutTime(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).CoarseLockoutTime;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("CFT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testFineLockoutTime(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).FineLockoutTime;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("FFT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testTareMode(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).TareMode;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("TMD"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testUpperToleranceLimit(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).UpperToleranceLimit;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("UTL"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLowerToleranceLimit(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).LowerToleranceLimit;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("LTL"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testMinimumStartWeight(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).MinimumStartWeight;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("MSW"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testEmptyWeight(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).EmptyWeight;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("EWT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testTareDelay(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).TareDelay;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("TAD"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testCoarseFlowMonitoringTime(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).CoarseFlowMonitoringTime;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("CBT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testCoarseFlowMonitoring(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).CoarseFlowMonitoring;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("CBK"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testFineFlowMonitoring(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).FineFlowMonitoring;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("FBK"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testFineFlowMonitoringTime(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).FineFlowMonitoringTime;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("FBT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testSystematicDifference(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testDouble = ((IDataFillerExtended)_wtxObj.Filler).SystematicDifference;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("SYD"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testValveControl(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).ValveControl;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("VCT"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testEmptyingMode(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).EmptyingMode;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("EMD"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testDelayTimeAfterFineFlow(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).DelayTimeAfterFilling;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("DL1"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testActivationTimeAfterFineFlow(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _testInteger = ((IDataFillerExtended)_wtxObj.Filler).ActivationTimeAfterFilling;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("FFL"));
        }
     
        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitSwitchStatusLVS1(Behavior behavior)
        {
            bool testVar = false;

            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            bool LimitSwitch1 = _wtxObj.LimitSwitch.LimitStatus1;

            if (_jetTestConnection.getDataBuffer.ContainsKey("2020/25") == true && !LimitSwitch1)
                testVar = true;
            else
                testVar = false;

            Assert.IsTrue(testVar);
        }

        /*
        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitSwitchStatusLVS2(Behavior behavior)
        {
            bool testVar = false;

            _jetTestConnection = new TestJetbusConnection(behavior, "wss://172.19.103.8:443/jet/canopen", "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);
        
            int LimitSwitch2 = _wtxObj.LimitSwitch.LimitSwitch2Source;

            if (_jetTestConnection.getDataBuffer.ContainsKey("2020/25") == true && LimitSwitch2 == 1)
                testVar = true;
            else
                testVar = false;

            Assert.IsTrue(testVar);
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitSwitchStatusLVS3(Behavior behavior)
        {
            bool testVar = false;

            _jetTestConnection = new TestJetbusConnection(behavior, "wss://172.19.103.8:443/jet/canopen", "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            int LimitSwitch3 = _wtxObj.LimitSwitch.LimitSwitch3Source;

            if (_jetTestConnection.getDataBuffer.ContainsKey("2020/25") == true && LimitSwitch3 == 0)
                testVar = true;
            else
                testVar = false;

            Assert.IsTrue(testVar);
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Attributes")]
        public void testLimitSwitchStatusLVS4(Behavior behavior)
        {
            bool testVar = false;

            _jetTestConnection = new TestJetbusConnection(behavior, "wss://172.19.103.8:443/jet/canopen", "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 100, update);

            _wtxObj.Connect(this.OnConnect, 100);

            int LimitSwitch4 = _wtxObj.LimitSwitch.LimitSwitch4Source;

            if (_jetTestConnection.getDataBuffer.ContainsKey("2020/25") == true && LimitSwitch4 == 1)
                testVar = true;
            else
                testVar = false;

            Assert.IsTrue(testVar);
        }
        */
        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
        }
    }
}
