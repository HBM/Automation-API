// <copyright file="CalibrationTestsModbus.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using NUnit.Framework;
    using System;
    using System.Collections;
    [TestFixture]
    public class CalibrationTestsModbus
    {

        // Test case source for writing values to the WTX120 device : Calibrating
        private string ipaddress = "172.19.103.8";
        public static IEnumerable CalculateCalibrationTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.CalibrationFail).Returns(false);
                yield return new TestCaseData(Behavior.CalibrationSuccess).Returns(true);
            }
        }

        // The following 2 tests as a first draw : Implementation for the following 2 tests follows in the week from 27.08-31.08
        
        [Test, TestCaseSource(typeof(CalibrationTestsModbus), "CalculateCalibrationTestCases")]
        public bool CalculateCalibrationTest(Behavior behavior)
        {
            TestModbusTCPConnection testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            WTXModbus WTXModbusObj = new WTXModbus(testConnection, 200,update);

            WTXModbusObj.Connect(this.OnConnect, 100);

            double preload = 1;
            double capacity = 2;

            double multiplierMv2D = 500000; //   2 / 1000000; // 2mV/V correspond 1 million digits (d)

            double dPreload = preload * multiplierMv2D;
            double dNominalLoad = dPreload + (capacity * multiplierMv2D);

            WTXModbusObj.CalculateAdjustment(preload, capacity);

            if (
               (testConnection.getArrElement1 == (Convert.ToInt32(dPreload) & 0xffff0000) >> 16) &&
               (testConnection.getArrElement2 == (Convert.ToInt32(dPreload) & 0x0000ffff))  
               )
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        [Test, TestCaseSource(typeof(CalibrationTestsModbus), "CalculateCalibrationTestCases")]
        public bool CalibrationTest(Behavior behavior)
        {
            TestModbusTCPConnection testConnection = new TestModbusTCPConnection(behavior, ipaddress);

            WTXModbus WTXModbusObj = new WTXModbus(testConnection, 200,update);

            WTXModbusObj.Connect(this.OnConnect, 100);

            int testCalibrationValue = 111;
            
            WTXModbusObj.AdjustNominalSignalWithCalibrationWeight((double)testCalibrationValue);

            // Check if: write reg 46, CalibrationWeight and write reg 50, 0x7FFFFFFF

            if (
                (testConnection.getArrElement1 == (testCalibrationValue & 0xffff0000) >> 16) &&
                (testConnection.getArrElement2 == (testCalibrationValue & 0x0000ffff)) &&

                (testConnection.getArrElement3 == (0x7FFFFFFF & 0xffff0000) >> 16) &&
                (testConnection.getArrElement4 == (0x7FFFFFFF & 0x0000ffff))
            )
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        
        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnConnect(bool obj)
        {
            throw new NotImplementedException();
        }
    }
}
