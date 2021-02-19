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

namespace Hbm.Automation.Api.Test.DSEJetTest
{
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.DSE;
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
        private DSEJet _dseObj;
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

        public static IEnumerable DataRate
        {
            get
            {
                //yield return new TestCaseData(Behavior.NetGrossTareValues_Fail).Returns(false);
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

        // Test case source for lowPassFilter mode
        public static IEnumerable ReadTestCases_LowPassFilter
        {
            get
            {

                yield return new TestCaseData(Behavior.lowPassFilterNoFilter).Returns(true);
                yield return new TestCaseData(Behavior.lowPassFilterIIRFilter).Returns(true);
                yield return new TestCaseData(Behavior.lowPassFilterFIRFilter).Returns(true);
            }
        }

        // Test case source for lowPassFilterFrequency
        public static IEnumerable ReadTestCases_LowPassFilterFreq
        {
            get
            {

                yield return new TestCaseData(Behavior.lowPassFilterNoFFreq).Returns(0);
                yield return new TestCaseData(Behavior.lowPassFilterIIRFreq).Returns(400);
                yield return new TestCaseData(Behavior.lowPassFilterFIRFreq).Returns(200);
            }
        }

        public static IEnumerable ReadTestCases_WeightStep
        {
            get
            {
                yield return new TestCaseData(Behavior.weightStepCase).Returns(0.01);
            }
        }

        public static IEnumerable ReadTestCases_FWVersion
        {
            get
            {
                yield return new TestCaseData(Behavior.fwVersion);
            }
        }

        public static IEnumerable ReadTestCases_Ident
        {
            get
            {
                yield return new TestCaseData(Behavior.ident);
            }
        }

        public static IEnumerable ReadTestCases_SerialNumber
        {
            get
            {
                yield return new TestCaseData(Behavior.serialNumber);
            }
        }

        public static IEnumerable ReadTestCases_BlankTestCase
        {
            get
            {
                yield return new TestCaseData(Behavior.blankTestCase);
            }
        }

        public static IEnumerable ReadTestCases_AdditionalFilterTest
        {
            get
            {
                yield return new TestCaseData(Behavior.additionalFilterTest);
            }
        }

        public static IEnumerable ReadTestCases_AdditionalFilterFrequencyTest
        {
            get
            {
                yield return new TestCaseData(Behavior.additionalFilterFrequencyTestSuccess);
                yield return new TestCaseData(Behavior.additionalFilterFrequencyTestFail);

            }
        }

        public static IEnumerable ReadTestCases_ZeroValueTest
        {
            get
            {
                yield return new TestCaseData(Behavior.zeroValueTest);
            }
        }

        public static IEnumerable ReadTestCases_TareTest
        {
            get
            {
                yield return new TestCaseData(Behavior.tareModeNoneTest).Returns(TareMode.None);
                yield return new TestCaseData(Behavior.tareModePreTest).Returns(TareMode.PresetTare);
                yield return new TestCaseData(Behavior.tareModeTareTest).Returns(TareMode.Tare);
            }
        }

        public static IEnumerable ReadTestCases_WeightStableTest
        {
            get
            {
                yield return new TestCaseData(Behavior.weightStableFalseTest).Returns(false);
                yield return new TestCaseData(Behavior.weightStableTrueTest).Returns(true);
            }
        }

        public static IEnumerable ReadTestCases_ScaleRangeTest
        {
            get
            {
                yield return new TestCaseData(Behavior.scaleRangeTest).Returns(5);
            }
        }

        public static IEnumerable ReadTestCases_ReadWeightTest
        {
            get
            {
                yield return new TestCaseData(Behavior.readWeightTest).Returns(true);
            }
        }

        public static IEnumerable ReadTestCases_GeneralScaleErrorTest
        {
            get
            {
                yield return new TestCaseData(Behavior.generalScaleErrorFalseTest).Returns(false);
                yield return new TestCaseData(Behavior.generalScaleErrorTrueTest).Returns(true);
            }
        }

        public static IEnumerable ReadTestCases_ReadWriteManualTareTest
        {
            get
            {
                yield return new TestCaseData(Behavior.readWriteManualTareTestFail).Returns(false);
                yield return new TestCaseData(Behavior.readWriteManualTareTestSuccess).Returns(true);
            }
        }

        public static IEnumerable ReadTestCases_ReadWriteMaxCapTest
        {
            get
            {
                yield return new TestCaseData(Behavior.readWriteMaxCapTestFail).Returns(false);
                yield return new TestCaseData(Behavior.readWriteMaxCapTestSuccess).Returns(true);
            }
        }

        public static IEnumerable ReadTestCases_ReadWriteCalWeightTest
        {
            get
            {
                yield return new TestCaseData(Behavior.readWriteCalWeightFail).Returns(false);
                yield return new TestCaseData(Behavior.readWriteCalWeightSuccess).Returns(true);
            }
        }

        public static IEnumerable ReadTestCases_ReadWriteWeightMoveTest
        {
            get
            {
                yield return new TestCaseData(Behavior.readWriteWeightMoveDetecFail).Returns(false);
                yield return new TestCaseData(Behavior.readWriteWeightMoveDetecSuccess).Returns(true);
            }
        }

        public static IEnumerable ReadTestCases_ReadWriteScaleRangeTest
        {
            get
            {
                yield return new TestCaseData(Behavior.readWriteScaleRangeModeFail).Returns(false);
                yield return new TestCaseData(Behavior.readWriteScaleRangeModeSuccess).Returns(true);
            }
        }

        [SetUp]
        public void Setup()
        {
            _testInteger = 0;
            _testBoolean = false;
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_BlankTestCase")]
        public void filterTypeMethod(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            Assert.AreEqual((FilterTypes)13105, _dseObj.FilterType(13109));
            Assert.AreEqual((FilterTypes)13089, _dseObj.FilterType(13093));
            Assert.AreEqual((FilterTypes)0, _dseObj.FilterType(0));

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_BlankTestCase")]
        public void dateTimeTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect();

            _dseObj.DateTime = 18012021;

            Assert.AreEqual(18012021, _dseObj.DateTime);

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_BlankTestCase")]
        public void maxZeroingTimeTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect();

            _dseObj.MaximumZeroingTime = 5;

            Assert.AreEqual(5, _dseObj.MaximumZeroingTime);

        }


        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ReadWriteMaxCapTest")]
        public bool readWriteCalWeight(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            _dseObj.CalibrationWeight = 4000;

            if (_dseObj.CalibrationWeight == 4000) return true;
            else return false;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ReadWriteScaleRangeTest")]
        public bool readWriteScaleRangeModeTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            bool cond = false;

            _dseObj.ScaleRangeMode = ScaleRangeMode.None;

            if (_dseObj.ScaleRangeMode == ScaleRangeMode.None) cond = true;

            _dseObj.ScaleRangeMode = ScaleRangeMode.MultiRange;

            if (_dseObj.ScaleRangeMode == ScaleRangeMode.MultiRange && cond == true) cond = true;

            _dseObj.ScaleRangeMode = ScaleRangeMode.MultiInterval;

            if (_dseObj.ScaleRangeMode == ScaleRangeMode.MultiInterval && cond == true) cond = true;

            return cond;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ReadWriteWeightMoveTest")]
        public bool readWriteWeightMoveTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.WeightMovementDetection = 1;

            if (_dseObj.WeightMovementDetection == 1) return true;
            else return false;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ReadWriteCalWeightTest")]
        public bool readWriteMaxCapTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.MaximumCapacity = 4000;

            if (_dseObj.MaximumCapacity == 4000) return true;
            else return false;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ReadWriteManualTareTest")]
        public bool readWriteManualTareTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            _dseObj.ManualTareValue = 50.0;

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            if (_dseObj.ManualTareValue == 50) return true;
            else return false;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_TareTest")]
        public TareMode tareModeTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            return _dseObj.TareMode;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ReadWeightTest")]
        public bool readWeightTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            double expGross = 413.96;
            double expNet = 463.96;
            double expTare = 50;

            WeightType readWeight = _dseObj.Weight;

            if (readWeight.Gross == expGross && readWeight.Net == expNet && readWeight.Tare == expTare) return true;
            else return false;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_GeneralScaleErrorTest")]
        public bool generalScaleErrorTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            return _dseObj.GeneralScaleError;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ScaleRangeTest")]
        public int scaleRangeTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            return _dseObj.ScaleRange;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WeightStableTest")]
        public bool weightStableTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            return _dseObj.WeightStable;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_BlankTestCase")]
        public void nominalSignalTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            int setNom = 600;

            _dseObj.NominalSignal = setNom;

            Assert.AreEqual(setNom, _dseObj.NominalSignal);

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_BlankTestCase")]
        public void zeroSignalTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            int setZero = 600;

            _dseObj.ZeroSignal = setZero;

            Assert.AreEqual(setZero, _dseObj.ZeroSignal);

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_ZeroValueTest")]
        public void zeroValueTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            Assert.AreEqual(3, _dseObj.ZeroValue);

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_AdditionalFilterFrequencyTest")]
        public void additionalFilterFrequencyTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.FilterStage2Mode = FilterTypes.No_Filter;
            _dseObj.FilterStage3Mode = FilterTypes.No_Filter;
            _dseObj.FilterStage4Mode = FilterTypes.No_Filter;
            _dseObj.FilterStage5Mode = FilterTypes.No_Filter;



            if (behavior == Behavior.additionalFilterFrequencyTestSuccess)
            {
                Assert.AreEqual(0, _dseObj.FilterCutOffFrequencyStage2);
                Assert.AreEqual(0, _dseObj.FilterCutOffFrequencyStage3);
                Assert.AreEqual(0, _dseObj.FilterCutOffFrequencyStage4);
                Assert.AreEqual(0, _dseObj.FilterCutOffFrequencyStage5);

                _dseObj.FilterStage2Mode = FilterTypes.FIR_Comb_Filter;
                _dseObj.FilterCutOffFrequencyStage2 = 200;
                _dseObj.FilterStage3Mode = FilterTypes.FIR_Comb_Filter;
                _dseObj.FilterCutOffFrequencyStage3 = 300;
                _dseObj.FilterStage4Mode = FilterTypes.FIR_Comb_Filter;
                _dseObj.FilterCutOffFrequencyStage4 = 400;
                _dseObj.FilterStage5Mode = FilterTypes.FIR_Comb_Filter;
                _dseObj.FilterCutOffFrequencyStage5 = 500;
                Assert.AreEqual(200, _dseObj.FilterCutOffFrequencyStage2);
                Assert.AreEqual(300, _dseObj.FilterCutOffFrequencyStage3);
                Assert.AreEqual(400, _dseObj.FilterCutOffFrequencyStage4);
                Assert.AreEqual(500, _dseObj.FilterCutOffFrequencyStage5);

                _dseObj.FilterStage2Mode = FilterTypes.FIR_Moving_Average;
                _dseObj.FilterCutOffFrequencyStage2 = 300;
                _dseObj.FilterStage3Mode = FilterTypes.FIR_Moving_Average;
                _dseObj.FilterCutOffFrequencyStage3 = 400;
                _dseObj.FilterStage4Mode = FilterTypes.FIR_Moving_Average;
                _dseObj.FilterCutOffFrequencyStage4 = 500;
                _dseObj.FilterStage5Mode = FilterTypes.FIR_Moving_Average;
                _dseObj.FilterCutOffFrequencyStage5 = 600;
                Assert.AreEqual(300, _dseObj.FilterCutOffFrequencyStage2);
                Assert.AreEqual(400, _dseObj.FilterCutOffFrequencyStage3);
                Assert.AreEqual(500, _dseObj.FilterCutOffFrequencyStage4);
                Assert.AreEqual(600, _dseObj.FilterCutOffFrequencyStage5);

            } else if(behavior == Behavior.additionalFilterFrequencyTestFail)
            {
                Assert.AreNotEqual(100, _dseObj.FilterCutOffFrequencyStage2);
                Assert.AreNotEqual(100, _dseObj.FilterCutOffFrequencyStage3);
                Assert.AreNotEqual(100, _dseObj.FilterCutOffFrequencyStage4);
                Assert.AreNotEqual(100, _dseObj.FilterCutOffFrequencyStage5);
            }
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_AdditionalFilterTest")]
        public void additionalFilterTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.FilterStage2Mode = FilterTypes.FIR_Comb_Filter;

            _dseObj.FilterStage2Mode = FilterTypes.No_Filter;

            Assert.AreEqual(FilterTypes.No_Filter, _dseObj.FilterStage2Mode);

            _dseObj.FilterStage3Mode = FilterTypes.No_Filter;

            _dseObj.FilterStage3Mode = FilterTypes.FIR_Comb_Filter;

            Assert.AreEqual(FilterTypes.FIR_Comb_Filter, _dseObj.FilterStage3Mode);

            _dseObj.FilterStage4Mode = FilterTypes.No_Filter;

            _dseObj.FilterStage4Mode = FilterTypes.FIR_Moving_Average;

            Assert.AreEqual(FilterTypes.FIR_Moving_Average, _dseObj.FilterStage4Mode);

            _dseObj.FilterStage5Mode = FilterTypes.FIR_Moving_Average;

            _dseObj.FilterStage5Mode = FilterTypes.No_Filter;

            Assert.AreEqual(FilterTypes.No_Filter, _dseObj.FilterStage5Mode);
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_FWVersion")]
        public void fwVersion(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            Assert.IsTrue(_dseObj.FirmwareVersion.Equals("testFW"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Ident")]
        public void ident(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            Assert.IsTrue(_dseObj.Identification.Equals("testIdent"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_SerialNumber")]
        public void serialNumber(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            Assert.IsTrue(_dseObj.SerialNumber.Equals("DSEtest123"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_LowPassFilter")]
        public bool lowPassFilter(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            LowPassFilter filter = _dseObj.LowPassFilterMode;

            switch (behavior)
            {
                case Behavior.lowPassFilterNoFilter:
                    if (filter == LowPassFilter.No_Filter) return true;
                    else return false;
                case Behavior.lowPassFilterIIRFilter:
                    if (filter == LowPassFilter.IIR_Filter) return true;
                    else return false;
                case Behavior.lowPassFilterFIRFilter:
                    if (filter == LowPassFilter.FIR_Filter) return true;
                    else return false;
                default:
                    return false;
            }
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_LowPassFilterFreq")]
        public int lowPassFilterFreq(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            switch (behavior)
            {
                case Behavior.lowPassFilterNoFFreq:
                    _dseObj.LowPassFilterMode = LowPassFilter.No_Filter;
                    _dseObj.LowPasCutOffFrequency = 0;
                    break;
                case Behavior.lowPassFilterIIRFreq:
                    _dseObj.LowPassFilterMode = LowPassFilter.IIR_Filter;
                    _dseObj.LowPasCutOffFrequency = 400;
                    break;
                case Behavior.lowPassFilterFIRFreq:
                    _dseObj.LowPassFilterMode = LowPassFilter.FIR_Filter;
                    _dseObj.LowPasCutOffFrequency = 200;
                    break;
                default:
                    break;
            }
            return _dseObj.LowPasCutOffFrequency;

        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WeightStep")]
        public double weightStep(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            double ws = 10;

            _dseObj.WeightStep = ws;

            return _dseObj.WeightStep;

        }

        [Test, TestCaseSource(typeof(ReadTests), "NetGrossTareValuesTest")]
        public bool testNetGrossTareValues(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);
            
            _dseObj.Connect(this.OnConnect, 100);

            _dseObj.ProcessData.UpdateData(this, new EventArgs());

            if (_dseObj.ProcessData.Weight.Net == 0.0011 && _dseObj.ProcessData.Weight.Tare == 0.0011 && _dseObj.ProcessData.Weight.Gross == 0)
                return true;
            else
                return false;
        }

        [Test, TestCaseSource(typeof(ReadTests), "DataRate")]
        public bool testDataRate(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            if (_dseObj.DataRate == 2000)
                return true;
            else
                return false;
        }
        
        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_BlankTestCase")]
        public void testConnectionType(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            Assert.AreEqual("Jetbus", _dseObj.ConnectionType);
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightMovingValue(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.WeightStable;

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

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.GeneralScaleError;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testScaleAlarmTriggered(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.ScaleAlarm;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testOverload(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.Overload;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testScaleSealIsOpen(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.LegalForTrade;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightType(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.TareMode!=TareMode.None;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testScaleRange(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100,  update);

            _dseObj.Connect(this.OnConnect, 100);

            _testInteger = _dseObj.ProcessData.ScaleRange;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testZeroRequired(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.ZeroRequired;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightWithinTheCenterOfZero(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.CenterOfZero;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }

        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_WEIGHING_DEVICE_1_WEIGHT_STATUS")]
        public void testWeightInZeroRange(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testBoolean = _dseObj.ProcessData.InsideZero;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6012/01"));
        }



        [Test, TestCaseSource(typeof(ReadTests), "ReadTestCases_Decimals")]
        public void testDecimals(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _dseObj = new DSEJet(_jetTestConnection, 100, update);

            _dseObj.Connect(this.OnConnect, 100);

            _testInteger = _dseObj.ProcessData.Decimals;

            Assert.IsTrue(_jetTestConnection.getDataBuffer.ContainsKey("6013/01"));
        }

 
        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
        }
    }
}
