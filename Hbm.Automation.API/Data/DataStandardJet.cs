// <copyright file="DataStandard.cs" company="Hottinger Baldwin Messtechnik GmbH">
//
// Hbm.Weighing.Api, a library to communicate with HBM weighing technology devices  
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

namespace Hbm.Weighing.Api.Data
{
    using Hbm.Weighing.Api.WTX.Jet;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Jetbus implementation of the interface IDataStandard for the standard mode.
    /// The class DataStandardJet contains the data input word and data output words for the standard mode
    /// of WTX device 120 and 110 via Jetbus.
    /// </summary>
    public class DataStandardJet : IDataIO, IDataLimitSwitch
    {

        #region ==================== constants & fields ====================
        private bool _output1;
        private bool _output2;
        private bool _output3;
        private bool _output4;
        private LimitSwitchSource _limitSwitch1Source;
        private LimitSwitchMode _limitSwitch1Mode;
        private int _limitSwitch1LevelAndLowerBandValue;
        private int _limitSwitch1HysteresisAndBandHeight;
        private LimitSwitchSource _limitSwitch2Source;
        private LimitSwitchMode _limitSwitch2Mode;
        private int _limitSwitch2LevelAndLowerBandValue;
        private int _limitSwitch2HysteresisAndBandHeight;
        private LimitSwitchSource _limitSwitch3Source;
        private LimitSwitchMode _limitSwitch3Mode;
        private int _limitSwitch3LevelAndLowerBandValue;
        private int _limitSwitch3HysteresisAndBandHeight;
        private LimitSwitchSource _limitSwitch4Source;
        private LimitSwitchMode _limitSwitch4Mode;
        private int _limitSwitch4LevelAndLowerBandValue;
        private int _limitSwitch4HysteresisAndBandHeight;
        private INetConnection _connection;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Constructor of class DataStandardJet : Initalizes values and connects 
        /// the eventhandler from Connection to the interal update method
        /// </summary>
        public DataStandardJet(INetConnection Connection)
        {
            _connection = Connection;
            _connection.UpdateData += UpdateData;
            _connection.UpdateData += UpdateDataIO;
            _connection.UpdateData += UpdateDataLimitSwitch;
            WeightMemory = new WeightMemory();
            Input1 = false;
            Input2 = false;
            Input3 = false;
            Input4 = false;
            _output1 = false;
            _output2 = false;
            _output3 = false;
            _output4 = false;
            LimitStatus1 = false;
            LimitStatus2 = false;
            LimitStatus3 = false;
            LimitStatus4 = false;
            _limitSwitch1Source=0;
            _limitSwitch1Mode=0;
            _limitSwitch1LevelAndLowerBandValue = 0;
            _limitSwitch1HysteresisAndBandHeight=0;
            _limitSwitch2Source=0;
            _limitSwitch2Mode=0;
            _limitSwitch2LevelAndLowerBandValue = 0;
            _limitSwitch2HysteresisAndBandHeight = 0;
            _limitSwitch3Source =0;
            _limitSwitch3Mode=0;
            _limitSwitch3LevelAndLowerBandValue = 0;
            _limitSwitch3HysteresisAndBandHeight = 0;
            _limitSwitch4Source =0;
            _limitSwitch4Mode=0;
            _limitSwitch4LevelAndLowerBandValue = 0;
            _limitSwitch4HysteresisAndBandHeight = 0;
        }
        #endregion

        #region ==================== events & delegates ====================

        /// <summary>
        /// Updates and converts the values from buffer
        /// </summary>
        /// <param name="sender">Connection class</param>
        /// <param name="e">EventArgs, Event argument</param>
        public void UpdateData(object sender, EventArgs e)
        {
            try
            {
                WeightMemory = ExtractWeightMemory();
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("KeyNotFoundException in class DataStandardJet, update method");
            }
        }

        /// <summary>
        /// Updates and converts the values from buffer
        /// </summary>
        /// <param name="sender">Connection class</param>
        /// <param name="e">EventArgs, Event argument</param>
        public void UpdateDataLimitSwitch(object sender, EventArgs e)
        {
            try
            {
                LimitStatus1 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.LVSLimitValue1Status));
                LimitStatus2 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.LVSLimitValue2Status));
                LimitStatus3 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.LVSLimitValue3Status));
                LimitStatus4 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.LVSLimitValue4Status));

                ApplicationMode _applicationMode = (ApplicationMode)Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.IMDApplicationMode));
                if (_applicationMode == ApplicationMode.Standard)
                {
                    _limitSwitch1Mode = StringToLimitSwitchMode(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchMode));
                    _limitSwitch1Source = StringToLimitSwitchSource(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchSource));
                    _limitSwitch1LevelAndLowerBandValue = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchLevel));
                    _limitSwitch1HysteresisAndBandHeight = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchHysteresis));
                    _limitSwitch2Mode = StringToLimitSwitchMode(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchMode));
                    _limitSwitch2Source = StringToLimitSwitchSource(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchSource));
                    _limitSwitch2LevelAndLowerBandValue = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchLevel));
                    _limitSwitch2HysteresisAndBandHeight = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchHysteresis));
                    _limitSwitch3Mode = StringToLimitSwitchMode(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchMode));
                    _limitSwitch3Source = StringToLimitSwitchSource(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchSource));
                    _limitSwitch3LevelAndLowerBandValue = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchLevel));
                    _limitSwitch3HysteresisAndBandHeight = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchHysteresis));
                    _limitSwitch4Mode = StringToLimitSwitchMode(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchMode));
                    _limitSwitch4Source = StringToLimitSwitchSource(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchSource));
                    _limitSwitch4LevelAndLowerBandValue = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchLevel));
                    _limitSwitch4HysteresisAndBandHeight = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.LIV1LimitSwitchHysteresis));
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("KeyNotFoundException in class DataStandardJet, update method");
            }
        }
        /// <summary>
        /// Updates and converts the values from buffer
        /// </summary>
        /// <param name="sender">Connection class</param>
        /// <param name="e">EventArgs, Event argument</param>
        public void UpdateDataIO(object sender, EventArgs e)
        {
            try
            {
                Input1 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.IS1DigitalInput1));
                Input2 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.IS2DigitalInput2));
                Input3 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.IS3DigitalInput3));
                Input4 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.IS4DigitalInput4));
                _output1 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.OS1DigitalOutput1));
                _output2 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.OS2DigitalOutput2));
                _output3 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.OS3DigitalOutput3));
                _output4 = StringToBool(_connection.ReadFromBuffer(JetBusCommands.OS4DigitalOutput4));
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("KeyNotFoundException in class DataStandardJet, update method");
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
            get{ return _output1; }
            set
            {
                _connection.WriteInteger(JetBusCommands.OS1DigitalOutput1, Convert.ToInt32(value));
                _output1 = value;
            }
        }

        ///<inheritdoc/>
        public bool Output2
        {
            get{ return _output2; }
            set
            {
                _connection.WriteInteger(JetBusCommands.OS2DigitalOutput2, Convert.ToInt32(value));
                _output2 = value;
            }
        }

        ///<inheritdoc/>
        public bool Output3
        {
            get{ return _output3; }
            set
            {
                _connection.WriteInteger(JetBusCommands.OS3DigitalOutput3, Convert.ToInt32(value));
                _output3 = value;
            }
        }

        ///<inheritdoc/>
        public bool Output4
        {
            get{ return _output4; }
            set
            {
                _connection.WriteInteger(JetBusCommands.OS4DigitalOutput4, Convert.ToInt32(value));
                _output4 = value;
            }
        }

        ///<inheritdoc/>
        public bool LimitStatus1 { get; private set; }

        ///<inheritdoc/>
        public bool LimitStatus2 { get; private set; }

        ///<inheritdoc/>
        public bool LimitStatus3 { get; private set; }

        ///<inheritdoc/>
        public bool LimitStatus4 { get; private set; }

        ///<inheritdoc/>
        public WeightMemory WeightMemory { get; private set; }
        
        ///<inheritdoc/>
        public LimitSwitchSource LimitSwitch1Source
        {
            get
            {
                return _limitSwitch1Source;
            }
            set
            {
                  _connection.WriteInteger(JetBusCommands.LIV1LimitSwitchSource, LimitSwitchSouceToInt(value));
                _limitSwitch1Source = value;
            }
        }

        ///<inheritdoc/>
        public LimitSwitchMode LimitSwitch1Mode
        {
            get
            {
                return _limitSwitch1Mode;
            }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV1LimitSwitchMode, LimitSwitchModeToInt(value));
                _limitSwitch1Mode = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch1Level
        {
            get { return _limitSwitch1LevelAndLowerBandValue; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV1LimitSwitchLevel, value);
                _limitSwitch1LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch1Hysteresis
        {
            get { return _limitSwitch1HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV1LimitSwitchHysteresis, value);
                _limitSwitch1HysteresisAndBandHeight = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch1LowerBandValue
        {
            get { return _limitSwitch1LevelAndLowerBandValue; }
            set
            {
                this._connection.WriteInteger(JetBusCommands.LIV1LimitSwitchLevel, value);
                _limitSwitch1LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch1BandHeight
        {
            get { return _limitSwitch1HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV1LimitSwitchHysteresis, value);
                _limitSwitch1HysteresisAndBandHeight = value;
            }
        }

        ///<inheritdoc/>
        public LimitSwitchSource LimitSwitch2Source 
        {
            get { return _limitSwitch2Source; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV2LimitSwitchSource, LimitSwitchSouceToInt(value));
                _limitSwitch2Source = value;
            }
        }
        
        ///<inheritdoc/>
        public LimitSwitchMode LimitSwitch2Mode
        {
            get
            {
                return _limitSwitch2Mode;
            }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV2LimitSwitchSource, LimitSwitchModeToInt(value));
                _limitSwitch2Mode = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch2Level 
        {
            get { return _limitSwitch2LevelAndLowerBandValue; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV2LimitSwitchLevel, value);
                _limitSwitch2LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch2Hysteresis
        {
            get { return _limitSwitch2HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV2LimitSwitchHysteresis, value);
                _limitSwitch2HysteresisAndBandHeight = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch2LowerBandValue
        {
            get { return _limitSwitch2LevelAndLowerBandValue; }
            set
            {
                this._connection.WriteInteger(JetBusCommands.LIV2LimitSwitchLevel, value);
                _limitSwitch2LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch2BandHeight
        {
            get { return _limitSwitch2HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV2LimitSwitchHysteresis, value);
                _limitSwitch2HysteresisAndBandHeight = value;
            }
        }

        ///<inheritdoc/>
        public LimitSwitchSource LimitSwitch3Source
        {
            get { return _limitSwitch3Source; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV3LimitSwitchSource, LimitSwitchSouceToInt(value));
                _limitSwitch3Source = value;
            }
        }
        
        ///<inheritdoc/>
        public LimitSwitchMode LimitSwitch3Mode
        {
            get
            {
                return _limitSwitch3Mode;
            }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV3LimitSwitchSource, LimitSwitchModeToInt(value));
                _limitSwitch3Mode = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch3Level
        {
            get { return _limitSwitch3LevelAndLowerBandValue; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV3LimitSwitchLevel, value);
                _limitSwitch3LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch3Hysteresis
        {
            get { return _limitSwitch3HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV3LimitSwitchHysteresis, value);
                _limitSwitch3HysteresisAndBandHeight = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch3LowerBandValue
        {
            get { return _limitSwitch3LevelAndLowerBandValue; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV3LimitSwitchLevel, value);
                _limitSwitch3LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch3BandHeight
        {
            get { return _limitSwitch3HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV3LimitSwitchHysteresis, value);
                _limitSwitch3HysteresisAndBandHeight = value;
            }
        }

        ///<inheritdoc/>
        public LimitSwitchSource LimitSwitch4Source
        {
            get { return _limitSwitch4Source; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV4LimitSwitchSource, LimitSwitchSouceToInt(value));
                _limitSwitch4Source = value;
            }
        }
        
        ///<inheritdoc/>
        public LimitSwitchMode LimitSwitch4Mode
        {
            get
            {
                return _limitSwitch4Mode;
            }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV4LimitSwitchMode, LimitSwitchModeToInt(value));
                _limitSwitch4Mode = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch4Level
        {
            get { return _limitSwitch4LevelAndLowerBandValue; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV4LimitSwitchLevel, value);
                _limitSwitch4LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch4Hysteresis 
        {
            get { return _limitSwitch4HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV4LimitSwitchSource, value);
                _limitSwitch4HysteresisAndBandHeight = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch4LowerBandValue
        {
            get { return _limitSwitch1LevelAndLowerBandValue; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV4LimitSwitchLevel, value);
                _limitSwitch1LevelAndLowerBandValue = value;
            }
        }

        ///<inheritdoc/>
        public int LimitSwitch4BandHeight
        {
            get { return _limitSwitch4HysteresisAndBandHeight; }
            set
            {
                _connection.WriteInteger(JetBusCommands.LIV4LimitSwitchHysteresis, value);
                _limitSwitch4HysteresisAndBandHeight = value;
            }
        }
        #endregion

        #region =============== protected & private methods ================
        /// <summary>
        /// Convert limt switch mode from enum to int
        /// </summary>
        /// <param name="mode">Limit switch mode</param>
        /// <returns></returns>
        private int LimitSwitchModeToInt(LimitSwitchMode mode)
        {
            int result = 0;
            switch (mode)
            {
                case LimitSwitchMode.AboveLevel: result = 0; break;
                case LimitSwitchMode.BelowLevel: result = 1; break;
                case LimitSwitchMode.InsideBand: result = 2; break;
                case LimitSwitchMode.OutsideBand: result = 3; break;
            }
            return result;
        }

        /// <summary>
        /// Convert limt switch mode from int to enum
        /// </summary>
        /// <param name="mode">Limit switch mode from wtx device</param>
        /// <returns></returns>
        private LimitSwitchMode StringToLimitSwitchMode(string mode)
        {
            LimitSwitchMode result = LimitSwitchMode.AboveLevel;
            switch (mode)
            {
                case "0": result = LimitSwitchMode.AboveLevel; break;
                case "2": result = LimitSwitchMode.BelowLevel; break;
                case "3": result = LimitSwitchMode.InsideBand; break;
                case "4": result = LimitSwitchMode.OutsideBand; break;
            }
            return result;
        }

        /// <summary>
        /// Convert limt switch source from enum to int
        /// </summary>
        /// <param name="source">Limit switch source</param>
        /// <returns></returns>
        private int LimitSwitchSouceToInt(LimitSwitchSource source)
        {
            int result = 0;
            switch (source)
            {
                case LimitSwitchSource.Gross: result = 0; break;
                case LimitSwitchSource.Net: result = 1; break;
            }
            return result;
        }

        /// <summary>
        /// Convert limt switch source from string to enum
        /// </summary>
        /// <param name="mode">Limit switch source from wtx device</param>
        /// <returns></returns>
        private LimitSwitchSource StringToLimitSwitchSource(string mode)
        {
            LimitSwitchSource result = LimitSwitchSource.Gross;
            switch (mode)
            {
                case "0": result = LimitSwitchSource.Gross; break;
                case "1": result = LimitSwitchSource.Net; break;
            }
            return result;
        }

        /// <summary>
        /// Convert string to bool ("0"=False, "1"=True)
        /// </summary>
        /// <param name="boolAsString">Sring representing a boolean</param>
        /// <returns></returns>
        private bool StringToBool(string boolAsString)
        {
            return boolAsString != "0";
        }

        private WeightMemory ExtractWeightMemory()
        {
             WeightMemory _wm = new WeightMemory();
            string _currentweight = _connection.ReadFromBuffer(JetBusCommands.WRSWeightMemoryEntry);
            if (_currentweight.Length>61)
            {
                try
                {
                    int _year = Convert.ToInt32(_currentweight.Substring(10, 2)) + 2000;
                    int _month = Convert.ToInt32(_currentweight.Substring(7, 2));
                    int _day = Convert.ToInt32(_currentweight.Substring(4, 2));
                    int _id = Convert.ToInt32(_currentweight.Substring(17, 4).Trim());
                    int _gross = Convert.ToInt32(_currentweight.Substring(22, 8).Trim());
                    int _net = Convert.ToInt32(_currentweight.Substring(38, 8).Trim());
                    _wm.Update(_year, _month, _day, _net, _gross, _id);
                }
                catch
                {
                    _wm.Update(0, 0, 0, 0, 0, -1);
                }
            }
            return _wm;
        }
        #endregion
    }
}