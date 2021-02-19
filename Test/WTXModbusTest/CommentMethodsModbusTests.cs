// <copyright file="CommentMethodsModbusTests.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using System.Collections;
    using System.Threading.Tasks;

    [TestFixture]
    public class CommentMethodsModbusTests
    {
        private string ipaddress = "172.19.103.8";
        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable T_UnitValueTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.t_UnitValue_Success).Returns("t");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable KG_UnitValueTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.kg_UnitValue_Success).Returns("kg");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable G_UnitValueTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.g_UnitValue_Success).Returns("g");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable LB_UnitValueTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.lb_UnitValue_Success).Returns("lb");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossValueStringComment_0D_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_0D_Fail).Returns("0.010000");
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_0D_Success).Returns("10000");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossValueStringComment_1D_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_1D_Fail).Returns("10000");
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_1D_Success).Returns("1000.0");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossValueStringComment_2D_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_2D_Fail).Returns("10000");
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_2D_Success).Returns("100.00");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossValueStringComment_3D_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_3D_Fail).Returns("10000");
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_3D_Success).Returns("10.000");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossValueStringComment_4D_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_4D_Fail).Returns("10000");
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_4D_Success).Returns("1.0000");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossValueStringComment_5D_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_5D_Fail).Returns("10000");
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_5D_Success).Returns("0.10000");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable NetGrossValueStringComment_6D_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_6D_Fail).Returns("10000");
                yield return new TestCaseData(Behavior.NetGrossValueStringComment_6D_Success).Returns("0.010000");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable LimitStatusStringComment_Case0_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case0_Fail).Returns("Higher than safe load limit");
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case0_Success).Returns("Weight within limits");
            }
        }
        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable LimitStatusStringComment_Case1_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case1_Fail).Returns("Higher than maximum capacity");
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case1_Success).Returns("Lower than minimum");
            }
        }
        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable LimitStatusStringComment_Case2_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case2_Fail).Returns("Weight within limits");
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case2_Success).Returns("Higher than maximum capacity");
            }
        }
        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable LimitStatusStringComment_Case3_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case3_Fail).Returns("Lower than minimum");
                yield return new TestCaseData(Behavior.LimitStatusStringComment_Case3_Success).Returns("Higher than safe load limit");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable WeightMovingStringComment_Case0_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.WeightMovingStringComment_Case0_Fail).Returns("1=Weight is moving");
                yield return new TestCaseData(Behavior.WeightMovingStringComment_Case0_Success).Returns("0=Weight is not moving.");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable WeightMovingStringComment_Case1_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.WeightMovingStringComment_Case1_Fail).Returns("0=Weight is not moving.");
                yield return new TestCaseData(Behavior.WeightMovingStringComment_Case1_Success).Returns("1=Weight is moving");
            }
        }

        // Test case source for reading values from the WTX120 device. 
        public static IEnumerable WeightTypeStringComment_Case1_TestCase_Modbus
        {
            get
            {
                yield return new TestCaseData(Behavior.WeightTypeStringComment_Case1_Fail).Returns("gross");
                yield return new TestCaseData(Behavior.WeightTypeStringComment_Case1_Success).Returns("net");
            }
        }

        [Test, TestCaseSource(typeof(CommentMethodsModbusTests), "NetGrossValueStringComment_0D_TestCase_Modbus")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "NetGrossValueStringComment_1D_TestCase_Modbus")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "NetGrossValueStringComment_2D_TestCase_Modbus")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "NetGrossValueStringComment_3D_TestCase_Modbus")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "NetGrossValueStringComment_4D_TestCase_Modbus")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "NetGrossValueStringComment_5D_TestCase_Modbus")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "NetGrossValueStringComment_6D_TestCase_Modbus")]
        public async Task<string> testModbus_NetGrossValueStringComment(Behavior behavior)
        {
            TestModbusTCPConnection testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            WTXModbus _wtxObj = new WTXModbus(testConnection, 200, update);
            _wtxObj.Connect(this.OnConnect, 100);

            await Task.Run(async () =>
            {
                ushort[] _data = await testConnection.SyncData();
            });

            string strValue = _wtxObj.PrintableWeight.Net;

            return strValue;
        }

        [Test, TestCaseSource(typeof(CommentMethodsModbusTests), "T_UnitValueTestCases")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "KG_UnitValueTestCases")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "G_UnitValueTestCases")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "LB_UnitValueTestCases")]
        public async Task<string> testModbus_Unit(Behavior behavior)
        {
            TestModbusTCPConnection testConnection = new TestModbusTCPConnection(behavior, ipaddress);

            WTXModbus _wtxObj = new WTXModbus(testConnection, 200, update);

            _wtxObj.Connect(this.OnConnect, 100);

            await Task.Run(async () =>
            {
                ushort[] _data = await testConnection.SyncData();
            });


            return _wtxObj.ProcessData.Unit;
        }
        
        [Test, TestCaseSource(typeof(CommentMethodsModbusTests), "WeightMovingStringComment_Case0_TestCase_Modbus")]
        [TestCaseSource(typeof(CommentMethodsModbusTests), "WeightMovingStringComment_Case1_TestCase_Modbus")]
        public async Task<string> testModbus_WeightMovingStringComment(Behavior behavior)
        {
            TestModbusTCPConnection testConnection = new TestModbusTCPConnection(behavior, ipaddress);
            WTXModbus _wtxObj = new WTXModbus(testConnection, 200,update);
            _wtxObj.Connect(this.OnConnect, 100);

            await Task.Run(async () =>
            {
                ushort[] _data = await testConnection.SyncData();
            });
            
            if (_wtxObj.WeightStable)
                return "0=Weight is not moving.";
            else
                return "1=Weight is moving";            
        }
           
        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
        }
        private void OnConnect(bool obj)
        {
        }
        
    }
}
