// <copyright file="JetDataFillerExtended.cs" company="Hottinger Baldwin Messtechnik GmbH">
//
// Hbm.Automation.Api, a library to communicate with HBM weighing technology devices  
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

using Hbm.Automation.Api.Weighing.WTX.Jet;
using Hbm.Automation.Api.Utils;

namespace Hbm.Automation.Api.Data
{
    /// <summary>
    /// Implementation of the interface IDataFillerExtended for the filler extended mode.
    /// The class DataFillerExtended contains the data input word and data output words for the filler extended mode
    /// of WTX device 120 and 110 via Jetbus. 
    /// 
    /// This is only available via a JetBus Ethernet connection as an extension to DataFillerJet, not via Modbus. 
    /// </summary>
    public class JetDataFillerExtended : JetDataFiller, IDataFillerExtended
    {
        #region ==================== constants & fields ====================        
        private INetConnection _connection;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Constructor of class DataFillerExtendedJet : Initalizes values and connects 
        /// the eventhandler from Connection to the interal update method
        /// </summary>
        /// <param name="Connection">Target connection</param>
        public JetDataFillerExtended(INetConnection Connection):base(Connection)          
        {
            _connection = Connection;        
        }
        #endregion

        #region ==================== events & delegates ====================
        #endregion

        #region ======================== properties ========================
       
        ///<inhertifdoc/>
        public double MaterialStreamLastFilling
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.MFOMaterialFlow), decimals); }
        }

        ///<inhertifdoc/>
        public int SpecialFillingFunctions
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.SDFSpecialDosingFunctions); }
            set { _connection.WriteInteger(JetBusCommands.SDFSpecialDosingFunctions , value);}
        }

        ///<inhertifdoc/>
        public int DischargeTime
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.EPTDischargeTime); }
            set { _connection.WriteInteger(JetBusCommands.EPTDischargeTime , value); }
        }

        ///<inhertifdoc/>
        public bool EmptyWeightBreak
        {
            get { return (_connection.ReadIntegerFromBuffer(JetBusCommands.EWBEmptyWeightBreak)==1); }
            set 
            { 
                if (value) 
                    _connection.WriteInteger(JetBusCommands.EWBEmptyWeightBreak, 1);
                else
                    _connection.WriteInteger(JetBusCommands.EWBEmptyWeightBreak, 0);
            }
        }

        ///<inhertifdoc/>
        public int Delay1Dosing
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.DL1DosingDelay1); }
            set { _connection.WriteInteger(JetBusCommands.DL1DosingDelay1 , value); }
        }

        ///<inhertifdoc/>
        public int Delay2Dosing
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.DL2DosingDelay2); }
            set { _connection.WriteInteger(JetBusCommands.DL2DosingDelay2 , value); }
        }

        ///<inhertifdoc/>
        public double EmptyWeightTolerance
        {
            get { 
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.EWTEmptyWeight), decimals); }
            set {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                _connection.WriteInteger(JetBusCommands.EWTEmptyWeight , MeasurementUtils.DoubleToDigit(value, decimals)); }
        }

        ///<inhertifdoc/>
        public double ResidualFlowDosingCycle
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.RFOResidualFlow), decimals); }
            set {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                _connection.WriteInteger(JetBusCommands.RFOResidualFlow , MeasurementUtils.DoubleToDigit(value, decimals)); }
        }

        ///<inhertifdoc/>
        public new int ParameterSetProduct
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.RDPActivateParameterSet); }
            set { _connection.WriteInteger(JetBusCommands.RDPActivateParameterSet, value); }
        }

        ///<inhertifdoc/>
        public int WeightStorageMode
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.SMDRecordWeightMode); }
            set { _connection.WriteInteger(JetBusCommands.SMDRecordWeightMode, value); }
        }
        #endregion
    }
}