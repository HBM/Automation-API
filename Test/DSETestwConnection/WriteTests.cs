
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
    // Class for testing write functions of JetBusConnection, f.e. 'Write(path,data)' and 
    // 'WriteInt(object index)' and so on.
    [TestFixture]
    public class WriteTests
    {
        //private TestJetbusConnection _jetTestConnection;
        //private WTXJet _wtxObj;
        //private int testGrossValue;
        private string ipaddress = "wss://172.20.41.120:443/jet/canopen";


        // Test case source for writing values to the WTX120 device: Taring 
        public static IEnumerable WriteTareTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteTareFail).Returns(false);
                yield return new TestCaseData(Behavior.WriteTareSuccess).Returns(true);
            }
        }

        // Test case source for writing values to the WTX120 device: Set to gross
        public static IEnumerable WriteGrossTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteGrossFail).Returns(false);
                yield return new TestCaseData(Behavior.WriteGrossSuccess).Returns(true);
            }
        }

        // Test case source for writing values to the WTX120 device : Zeroing
        public static IEnumerable WriteZeroTestCases
        {
            get
            {
                yield return new TestCaseData(Behavior.WriteZeroFail).Returns(false);
                yield return new TestCaseData(Behavior.WriteZeroSuccess).Returns(true);
            }
        }

        [SetUp]
        public void Setup()
        {
            //testGrossValue = 0;
        }

        private TestJetbusConnection _jetTestConnection;
        private WTXJet _wtxObj;

        [Test, TestCaseSource(typeof(WriteTests), "WriteTareTestCases")]
        public bool WriteTareTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 200, update);

            _wtxObj.Connect(this.OnConnect, 100);
          
            _wtxObj.Tare();     // Write in index(address) "6002/01" value 1701994868

            if (_jetTestConnection.getDataBuffer.ContainsKey("6002/01") && _jetTestConnection.getDataBuffer.ContainsValue(1701994868))
                return true;

            else
                return false;

        }

        [Test, TestCaseSource(typeof(WriteTests), "WriteGrossTestCases")]
        public bool WriteGrossTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 200, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.SetGross();     // Write in index(address) "6002/01" value 1936683623

            if (_jetTestConnection.getDataBuffer.ContainsKey("6002/01") && _jetTestConnection.getDataBuffer.ContainsValue(1936683623))
                return true;

            else
                return false;

        }

        [Test, TestCaseSource(typeof(WriteTests), "WriteZeroTestCases")]
        public bool WriteZeroTest(Behavior behavior)
        {
            _jetTestConnection = new TestJetbusConnection(behavior, ipaddress, "Administrator", "wtx", delegate { return true; });

            _wtxObj = new WTXJet(_jetTestConnection, 200, update);

            _wtxObj.Connect(this.OnConnect, 100);

            _wtxObj.Zero();     // Write in index(address) "6002/01" value 1869768058);

            if (_jetTestConnection.getDataBuffer.ContainsKey("6002/01") && _jetTestConnection.getDataBuffer.ContainsValue(1869768058)) 
                return true;

            else
                return false;          

        }

        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnConnect(bool obj)
        {
        }

    }
}

