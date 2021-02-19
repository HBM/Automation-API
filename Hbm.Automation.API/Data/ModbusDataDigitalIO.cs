// <copyright file="ModbusDataDigitalIO.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

using Hbm.Automation.Api.Utils;
using Hbm.Automation.Api.Weighing.WTX.Modbus;
using System;
using System.Collections.Generic;

namespace Hbm.Automation.Api.Data
{
    /// <summary>
    /// Implementation of the interface IDataStandard for the standard mode.
    /// The class DataStandardModbus contains the data input word and data output words for the standard mode
    /// of WTX device 120 and 110 via Modbus.
    /// </summary>
    public class ModbusDataDigitalIO : IDataDigitalIO
    {

        #region ==================== constants & fields ====================
        private bool _output1;
        private bool _output2;
        private bool _output3;
        private bool _output4;              
        private INetConnection _connection;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Constructor of class DataStandardModbus : Initalizes values and connects 
        /// the eventhandler from Connection to the interal update method
        /// </summary>
        /// <param name="Connection">Target connection</param>
        public ModbusDataDigitalIO(INetConnection Connection)
        {
            _connection = Connection;
            _connection.UpdateData += UpdateDataIO;
            Input1 = false;
            Input2 = false;
            Input3 = false;
            Input4 = false;
            _output1 = false;
            _output2 = false;
            _output3 = false;
            _output4 = false;           
        }
        #endregion

        #region ==================== events & delegates ====================

        /// <summary>
        /// Updates and converts the values from buffer
        /// </summary>
        /// <param name="sender">Connection class</param>
        /// <param name="e">EventArgs, Event argument</param>
        public void UpdateDataIO(object sender, EventArgs e)
        {
            try
            {
                Input1 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.IS1DigitalInput1));
                Input2 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.IS2DigitalInput2));
                Input3 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.IS3DigitalInput3));
                Input4 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.IS4DigitalInput4));
                _output1 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.OS1DigitalOutput1));
                _output2 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.OS2DigitalOutput2));
                _output3 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.OS3DigitalOutput3));
                _output4 = MeasurementUtils.StringToBool(_connection.ReadFromBuffer(ModbusCommands.OS4DigitalOutput4));
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("KeyNotFoundException in class DataStandardModbus, update method");
            }
        }
        #endregion

        #region ======================== properties ========================
        ///<inheritdoc/>
        public bool Input1 { get; private set; }

        ///<inheritdoc/>
        public bool Input2 { get; private set; }

        ///<inheritdoc/>
        public bool Input3 { get; private set; }

        ///<inheritdoc/>
        public bool Input4 { get; private set; }

        ///<inheritdoc/>
        public bool Output1
        {
            get { return _output1; }
            set
            {
                _connection.WriteInteger(ModbusCommands.OS1DigitalOutput1, Convert.ToInt32(value));
                _output1 = value;
            }
        }

        ///<inheritdoc/>
        public bool Output2
        {
            get { return _output2; }
            set
            {
                _connection.WriteInteger(ModbusCommands.OS2DigitalOutput2, Convert.ToInt32(value));
                _output2 = value;
            }
        }

        ///<inheritdoc/>
        public bool Output3
        {
            get { return _output3; }
            set
            {
                _connection.WriteInteger(ModbusCommands.OS3DigitalOutput3, Convert.ToInt32(value));
                _output3 = value;
            }
        }

        ///<inheritdoc/>
        public bool Output4
        {
            get { return _output4; }
            set
            {
                _connection.WriteInteger(ModbusCommands.OS4DigitalOutput4, Convert.ToInt32(value));
                _output4 = value;
            }
        }
                               
        #endregion

    }
}