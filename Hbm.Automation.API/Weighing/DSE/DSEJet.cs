// <copyright file="DSEJet.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.DSE
{
    using System;
    using System.Threading;
    using System.Linq;
    using Hbm.Automation.Api;
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Utils;
    using Hbm.Automation.Api.Weighing.WTX.Jet;

    /// <summary>
    /// This class represents a DSE device with the Jet ethernet interface.
    /// Subscribe to ProcessDataReceived over the constructor to automatically get weight values.
    /// </summary>
    public class DSEJet : BaseWTDevice, IDataScale, IDataDigitalFilter
    {
        #region ==================== constants & fields ====================
        private const int CONVERISION_FACTOR_MVV_TO_D = 1000000;  
        private const int SCALE_COMMAND_CALIBRATE_ZERO = 2053923171;
        private const int SCALE_COMMAND_CALIBRATE_NOMINAL = 1852596579;
        private const int SCALE_COMMAND_EXIT_CALIBRATE = 1953069157;
        private const int SCALE_COMMAND_TARE = 1701994868;
        private const int SCALE_COMMAND_CLEAR_PEAK_VALUES = 1801545072;
        private const int SCALE_COMMAND_ZERO = 1869768058;
        private const int SCALE_COMMAND_SET_GROSS = 1936683623;
        private const int SCALE_COMMAND_STATUS_ONGOING = 1869049455;
        private const int SCALE_COMMAND_STATUS_OK = 1802461023;
        private const int SCALE_COMMAND_STATUS_ERROR_E1 = 826629983;
        private const int SCALE_COMMAND_STATUS_ERROR_E2 = 843407199;
        private const int SCALE_COMMAND_STATUS_ERROR_E3 = 860184415;
        private const int COMMAND_FACTORY_DEFAULTS = 1684107116;
        private INetConnection _connection;
        #endregion
        
        #region ==================== events & delegates ====================
        /// <summary>
        /// Event handler to raise whenever new process data from the device is available
        /// </summary>
        public override event EventHandler<ProcessDataReceivedEventArgs> ProcessDataReceived; 
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Initializes a new instance of the <see cref="DSEJet" /> class.
        /// </summary>
        /// <param name="connection">Inject connection (e.g. JetBusConnection)</param>
        /// <param name="timerIntervalms">Interval for updating ProcessData</param>
        /// <param name="onProcessData">This event is automatically called when new ProcessData is available</param>
        public DSEJet(INetConnection connection, int timerIntervalms, EventHandler<ProcessDataReceivedEventArgs> onProcessData) 
            : base(connection, timerIntervalms)
        {
            _connection = connection;
            ProcessData = new JetProcessData(Connection);
            ProcessDataReceived += onProcessData;
        }
        #endregion

        #region ======================== Properties ========================

        ///<inheritdoc/>
        public override string ConnectionType
        {
            get
            {
                return "Jetbus";
            }
        }

        ///<inheritdoc/>
        public override bool IsConnected
        {
            get
            {
                return Connection.IsConnected;
            }
        }

        ///<inheritdoc/>
        public override WeightType Weight
        {
            get
            {
                return ProcessData.Weight;
            }
        }

        ///<inheritdoc/>
        public override PrintableWeightType PrintableWeight
        {
            get
            {
                return ProcessData.PrintableWeight;
            }
        }

        ///<inheritdoc/>
        public override ApplicationMode ApplicationMode { get; set; }

        ///<inheritdoc/>
        public override string Unit
        {
            get
            {
                return ProcessData.Unit;
            }

            set
            {
                int _unit=0;
                switch (value)
                {
                    case "kg": _unit = 0x00020000; break;
                    case "g": _unit = 0x004B0000; break;
                    case "lb": _unit = 0x00A60000; break;
                    case "t": _unit = 0x004C0000; break;
                    case "N": _unit = 0x00210000; break;
                    default: throw new NotSupportedException();
                }
                Connection.WriteInteger(JetBusCommands.CIA461Unit, _unit);
            }
        }

        ///<inheritdoc/>
        public override TareMode TareMode
        {
            get
            {
                return ProcessData.TareMode;
            }
        }

        ///<inheritdoc/>
        public override bool GeneralScaleError
        {
            get
            {
                return Connection.ReadIntegerFromBuffer(JetBusCommands.CIA461WeightStatusGeneralWeightError) != 0;
            }
        }

        ///<inheritdoc/>
        public override bool WeightStable
        {
            get
            {
                return ProcessData.WeightStable;
            }
        }

        ///<inheritdoc/>
        public override int ScaleRange
        {
            get
            {
                return ProcessData.ScaleRange + 1;
            }
        }

        ///<inheritdoc/>
        public override double ManualTareValue
        {
            get
            {
                int readValue = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461TareValue);
                return MeasurementUtils.DigitToDouble(readValue, ProcessData.Decimals);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461TareValue, MeasurementUtils.DoubleToDigit(value, ProcessData.Decimals));
            }
        }

        ///<inheritdoc/>
        public override int MaximumCapacity
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461ScaleMaximumCapacity);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461ScaleMaximumCapacity, value);
            }
        }

        ///<inheritdoc/>
        public override double CalibrationWeight
        {
            get
            {
                int readValue = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461CalibrationWeight);
                return MeasurementUtils.DigitToDouble(readValue, ProcessData.Decimals);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461CalibrationWeight, MeasurementUtils.DoubleToDigit(value, ProcessData.Decimals));
            }
        }

        ///<inheritdoc/>
        public override double ZeroValue
        {
            get
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEZeroValue), decimals);
            }
        }

        ///<inheritdoc/>
        public override int ZeroSignal
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.SDLScaleZeroSignal);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.SDLScaleZeroSignal, value);
            }
        }

        ///<inheritdoc/>
        public override int NominalSignal
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.SNLScaleNominalSignal);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.SNLScaleNominalSignal, value);
            }
        }


        public override string SerialNumber 
        {
            get
            {
                return Connection.ReadFromDevice(JetBusCommands.DSESerialNumber);
            }
        }

        ///<inheritdoc/>
        public override string Identification
        { 
            get
            {
                return Connection.ReadFromDevice(JetBusCommands.DSEIDNDeviceIdentification);
            }

            set
            {
                Connection.Write(JetBusCommands.DSEIDNDeviceIdentification, value);
            }
        }


        ///<inheritdoc/>
        public override string FirmwareVersion
        {
            get
            {
                return Connection.ReadFromDevice(JetBusCommands.DSEFirmwareVersion);
            }
        }

       ///<inheritdoc/>
        public double WeightStep
        {
            get
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.CIA461WeightStep), decimals);
            }

            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                Connection.WriteInteger(JetBusCommands.CIA461WeightStep, MeasurementUtils.DoubleToDigit(value, decimals));
            }
        }

        ///<inheritdoc/>
        public int WeightMovementDetection
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461WeightMovingDetection);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461WeightMovingDetection, value);
            }
        }

        ///<inheritdoc/>
        public ScaleRangeMode ScaleRangeMode
        {
            get
            {
                ScaleRangeMode srm = ScaleRangeMode.None;
                switch (_connection.ReadIntegerFromBuffer(JetBusCommands.CIA461MultiIntervalRangeControl) )
                {
                    case 0: srm = ScaleRangeMode.None; break;
                    case 1: srm = ScaleRangeMode.MultiInterval; break;
                    case 2: srm = ScaleRangeMode.MultiRange; break;
                }
                return srm;
            }

            set
            {
                int setValue = 0;
                switch (value)
                {
                    case ScaleRangeMode.None: setValue = 0; break;
                    case ScaleRangeMode.MultiInterval: setValue = 1; break;
                    case ScaleRangeMode.MultiRange: setValue =  2; break;
                }
                Connection.WriteInteger(JetBusCommands.CIA461MultiIntervalRangeControl, setValue);
            }
        }

        ///<inheritdoc/>
        public double MultiScaleLimit1
        { 
            get
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.CIA461MultiLimit1), decimals);
            }

            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                Connection.WriteInteger(JetBusCommands.CIA461MultiLimit1, MeasurementUtils.DoubleToDigit(value, decimals));
            }
        }

        ///<inheritdoc/>
        public double MultiScaleLimit2
        {
            get
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.CIA461MultiLimit2), decimals);
            }

            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                Connection.WriteInteger(JetBusCommands.CIA461MultiLimit2, MeasurementUtils.DoubleToDigit(value, decimals));
            }
        }


        /// <summary>
        /// Gets the data rate in Hz (DSE is 2000Hz fix)
        /// </summary>
        public int DataRate
        {
            get
            {
                return 2000;
            }

            set
            {
                
            }
        }
  
        ///<inheritdoc/>
        public LowPassFilter LowPassFilterMode
        {
            get
            {
                int currentFilter = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461ScaleFilter);
                switch (currentFilter)
                {
                    case 24737:
                        return LowPassFilter.IIR_Filter;
                    case 13073:
                        return LowPassFilter.FIR_Filter;
                    default:
                        return LowPassFilter.No_Filter;
                } 
            }

            set
            {
                int toWrite = 0;
                switch (value)
                {
                    case LowPassFilter.No_Filter:
                        break;
                    case LowPassFilter.IIR_Filter:
                        toWrite = 24737;
                        break;
                    case LowPassFilter.FIR_Filter:
                        toWrite = 13073;
                        break;
                    default:
                        return;
                }
                Connection.WriteInteger(JetBusCommands.CIA461ScaleFilter, toWrite);
            }
        }

        ///<inheritdoc/>
        public int LowPasCutOffFrequency
        {
            get
            {
                int __value = 0;
                switch (LowPassFilterMode)
                {
                    case LowPassFilter.FIR_Filter: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.DSELowPassCutOffFrequencyFIR); break;
                    case LowPassFilter.IIR_Filter: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.DSELowPassCutOffFrequencyIIR); break;
                    default: break;
                }
                return __value;
            }
            set
            {
                switch (LowPassFilterMode)
                {
                    case LowPassFilter.FIR_Filter:
                        _connection.WriteInteger(JetBusCommands.DSELowPassCutOffFrequencyFIR, value);
                        break;
                    case LowPassFilter.IIR_Filter:
                        _connection.WriteInteger(JetBusCommands.DSELowPassCutOffFrequencyIIR, value);
                        break;
                    default:
                        break;
                }
            }
        }

        //Gets or sets the filter type of the filter in stage 2
        public FilterTypes FilterStage2Mode
        {
            get
            {
                return FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage2));
            }
            set
            {
                int currentFilter = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage2);
                if (FilterType(currentFilter) == value) return;
                else
                {
                    int filter = GetLowestFilterIndex(value, 2);
                    _connection.WriteInteger(JetBusCommands.DSEFilterModeStage2, filter);
                }
            }
        }

        //Gets or sets the filter type of the filter in stage 3
        public FilterTypes FilterStage3Mode
        {
            get
            {
                return FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage3));
            }
            set
            {
                int currentFilter = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage3);
                if (FilterType(currentFilter) == value) return;
                else
                {
                    int filter = GetLowestFilterIndex(value, 3);
                    _connection.WriteInteger(JetBusCommands.DSEFilterModeStage3, filter);
                }
            }
        }

        //Gets or sets the filter type of the filter in stage 4
        public FilterTypes FilterStage4Mode
        {
            get
            {
                return FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage4));
            }
            set
            {
                int currentFilter = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage4);
                if (FilterType(currentFilter) == value) return;
                else
                {
                    int filter = GetLowestFilterIndex(value, 4);
                    _connection.WriteInteger(JetBusCommands.DSEFilterModeStage4, filter);
                }
            }
        }

        //Gets or sets the filter type of the filter in stage 5
        public FilterTypes FilterStage5Mode
        {
            get
            {
                return FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage5));
            }
            set
            {
                int currentFilter = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage5);
                if (FilterType(currentFilter) == value) return;
                else
                {
                    int filter = GetLowestFilterIndex(value, 3);
                    _connection.WriteInteger(JetBusCommands.DSEFilterModeStage5, filter);
                }
            }
        }

        //Returns the filter to be set depending on the filters that are already set
        //From filter [2,5]
        public int GetLowestFilterIndex(FilterTypes desired, int filter)
        {
            string[] filterArr = new string[4];
            int[] filtIndex = { 0, 0, 0, 0 };
            if (desired == FilterTypes.FIR_Comb_Filter) filtIndex = new int[] { 13089, 13090, 13091, 13092 };
            else if (desired == FilterTypes.FIR_Moving_Average) filtIndex = new int[] { 13105, 13106, 13107, 13108 };
            else if (desired == FilterTypes.No_Filter) return 0;

            int returnIndex = 0;
            
            filterArr[0] = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage2).ToString();
            filterArr[1] = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage3).ToString();
            filterArr[2] = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage4).ToString();
            filterArr[3] = _connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage5).ToString();

            for(int i = 0; i < 4; i++)
            {
                if (!filterArr.Contains(filtIndex[i].ToString()))
                {
                    returnIndex = filtIndex[i];
                    break;
                }
            }     
            
            return returnIndex;
        }

        //Gets or sets the cut off frequency of the currently used filter type in stage 2 in mHz
        public int FilterCutOffFrequencyStage2
        {
            get
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage2));
                switch (currentFilter)
                {
                    case FilterTypes.No_Filter:
                        return 0;
                    case FilterTypes.FIR_Comb_Filter:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSECombFilterFrequencyStage2);
                    case FilterTypes.FIR_Moving_Average:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSEMovAvFilterFrequencyStage2);
                    default:
                        return 0;
                }
            }
            set
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage2));
                switch (currentFilter)
                {
                    case FilterTypes.FIR_Comb_Filter:
                        _connection.WriteInteger(JetBusCommands.DSECombFilterFrequencyStage2, value);
                        break;
                    case FilterTypes.FIR_Moving_Average:
                        _connection.WriteInteger(JetBusCommands.DSEMovAvFilterFrequencyStage2, value);
                        break;
                    default:
                        break;
                }
            }
        }

        //Gets or sets the cut off frequency of the currently used filter type in stage 3 in mHz
        public int FilterCutOffFrequencyStage3
        {
            get
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage3));
                switch (currentFilter)
                {
                    case FilterTypes.No_Filter:
                        return 0;
                    case FilterTypes.FIR_Comb_Filter:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSECombFilterFrequencyStage3);
                    case FilterTypes.FIR_Moving_Average:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSEMovAvFilterFrequencyStage3);
                    default:
                        return 0;
                }
            }
            set
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage3));
                switch (currentFilter)
                {
                    case FilterTypes.FIR_Comb_Filter:
                        _connection.WriteInteger(JetBusCommands.DSECombFilterFrequencyStage3, value);
                        break;
                    case FilterTypes.FIR_Moving_Average:
                        _connection.WriteInteger(JetBusCommands.DSEMovAvFilterFrequencyStage3, value);
                        break;
                    default:
                        break;
                }
            }
        }

        //Gets or sets the cut off frequency of the currently used filter type in stage 4 in mHz
        public int FilterCutOffFrequencyStage4
        {
            get
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage4));
                switch (currentFilter)
                {
                    case FilterTypes.No_Filter:
                        return 0;
                    case FilterTypes.FIR_Comb_Filter:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSECombFilterFrequencyStage4);
                    case FilterTypes.FIR_Moving_Average:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSEMovAvFilterFrequencyStage4);
                    default:
                        return 0;
                }
            }
            set
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage4));
                switch (currentFilter)
                {
                    case FilterTypes.FIR_Comb_Filter:
                        _connection.WriteInteger(JetBusCommands.DSECombFilterFrequencyStage4, value);
                        break;
                    case FilterTypes.FIR_Moving_Average:
                        _connection.WriteInteger(JetBusCommands.DSEMovAvFilterFrequencyStage4, value);
                        break;
                    default:
                        break;
                }
            }
        }


        //Gets or sets the cut off frequency of the currently used filter type in stage 5 in mHz
        public int FilterCutOffFrequencyStage5
        {
            get
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage5));
                switch (currentFilter)
                {
                    case FilterTypes.No_Filter:
                        return 0;
                    case FilterTypes.FIR_Comb_Filter:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSECombFilterFrequencyStage5);
                    case FilterTypes.FIR_Moving_Average:
                        return _connection.ReadIntegerFromBuffer(JetBusCommands.DSEMovAvFilterFrequencyStage5);
                    default:
                        return 0;
                }
            }
            set
            {
                FilterTypes currentFilter = FilterType(_connection.ReadIntegerFromBuffer(JetBusCommands.DSEFilterModeStage5));
                switch (currentFilter)
                {
                    case FilterTypes.FIR_Comb_Filter:
                        _connection.WriteInteger(JetBusCommands.DSECombFilterFrequencyStage5, value);
                        break;
                    case FilterTypes.FIR_Moving_Average:
                        _connection.WriteInteger(JetBusCommands.DSEMovAvFilterFrequencyStage5, value);
                        break;
                    default:
                        break;
                }
            }
        }

        ///<inheritdoc/>
        public int LowPassFilterOrder
        {
            get
            {
                /**int __value;
                switch (LowPassFilterMode)
                {
                    case 0x60A1: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterCriticallyDampedFilterOrder); break;
                    case 0x60A2: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterBesselFilterOrder); break;
                    default:
                    case 0x60B1: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterButterworthFilterOrder); break;
                }
                return __value;**/
                return 0;
            }

            set
            {
                /**
                Connection.WriteInteger(JetBusCommands.CIA461FilterCriticallyDampedFilterOrder, value);
                Connection.WriteInteger(JetBusCommands.CIA461FilterBesselFilterOrder, value);
                Connection.WriteInteger(JetBusCommands.CIA461FilterButterworthFilterOrder, value);**/
            }
        }

        /// <summary>
        /// Gets or sets the maximum zeroing time (DSE device)
        /// </summary>
        public int MaximumZeroingTime { get; set; }

        /// <summary>
        /// Gets or sets the current time (DSE device)
        /// </summary>
        public int DateTime { get; set; }

        #endregion

        #region ================ public & internal methods ================= 
        ///<inheritdoc/>
        public override void Connect(double timeoutMs = 20000)
        {
            Connection.Connect();
            ProcessDataTimer.Change(0, ProcessDataInterval);
        }
        
        ///<inheritdoc/>
        public override void Connect(Action<bool> completed, double timeoutMs)
        {
            Connection.Connect();
            ProcessDataTimer.Change(0, ProcessDataInterval);
        }

        ///<inheritdoc/>
        public override void Disconnect(Action<bool> disconnectCompleted)
        {
            Connection.Disconnect();
        }

        ///<inheritdoc/>
        public override void Disconnect()
        {
            Connection.Disconnect();
        }

        ///<inheritdoc/>
        public override void SaveAllParameters()
        {
            Connection.WriteInteger(JetBusCommands.CIA461SaveAllParameters, 0);
        }

        ///<inheritdoc/>
        public override void RestoreAllDefaultParameters()
        {
            Connection.WriteInteger(JetBusCommands.DSERestoreAllDefaultParameters, COMMAND_FACTORY_DEFAULTS);       //command missing int.Parse("6c6f6164", System.Globalization.NumberStyles.HexNumber)
        }

        ///<inheritdoc/>
        public override void Zero()
        {
            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_ZERO);
        }
        
        ///<inheritdoc/>
        public override void SetGross()
        {
            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_SET_GROSS);
        }
        
        ///<inheritdoc/>
        public override void Tare()
        {
            if (Connection.IsConnected)
                Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_TARE);
        }
        
        ///<inheritdoc/>
        public override void TareManually(double manualTareValue)
        {
            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_TARE);
        }
        
        ///<inheritdoc/>
        public override void RecordWeight()
        {
            Connection.WriteInteger(JetBusCommands.STORecordWeight, SCALE_COMMAND_TARE);
        }

        ///<inheritdoc/>
        public override void CalculateAdjustment(double scaleZeroLoad_mVV, double scaleCapacity_mVV)
        {
            int scalZeroLoad_d;
            int scaleCapacity_d;

            scalZeroLoad_d = (int)(scaleZeroLoad_mVV * CONVERISION_FACTOR_MVV_TO_D);
            scaleCapacity_d = (int)(scalZeroLoad_d + (scaleCapacity_mVV * CONVERISION_FACTOR_MVV_TO_D));

            this.ZeroSignal = Convert.ToInt32(scalZeroLoad_d);
            this.NominalSignal = Convert.ToInt32(scaleCapacity_d);

        }

        ///<inheritdoc/>
        public override bool AdjustZeroSignal()
        {
            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_CALIBRATE_ZERO); 

            while (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) != SCALE_COMMAND_STATUS_ONGOING)
            {
                Thread.Sleep(200);
            }     
            while (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) == SCALE_COMMAND_STATUS_ONGOING)
            {
                Thread.Sleep(200);
            }
            
            if (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) == SCALE_COMMAND_STATUS_OK)
            {
                return true;
            }
            
            return false;
        }
        
        ///<inheritdoc/>
        public override bool AdjustNominalSignal()
        {
            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_CALIBRATE_NOMINAL);

            while (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) != SCALE_COMMAND_STATUS_ONGOING)
            {
                Thread.Sleep(100);
            }

            while (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) == SCALE_COMMAND_STATUS_ONGOING)
            {
                Thread.Sleep(100);
            }

            if (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) == SCALE_COMMAND_STATUS_OK)
            {
                return true;
            }

            return false;
        }
        
        ///<inheritdoc/>
        public override bool AdjustNominalSignalWithCalibrationWeight(double calibrationWeight)
        {
            Connection.WriteInteger(JetBusCommands.CIA461CalibrationWeight, MeasurementUtils.DoubleToDigit(calibrationWeight, ProcessData.Decimals));

            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_CALIBRATE_NOMINAL);
           
            
            while (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) != SCALE_COMMAND_STATUS_ONGOING)
            {
                Thread.Sleep(100);
            }
            
            while (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) == SCALE_COMMAND_STATUS_ONGOING)
            {
                Thread.Sleep(100);
            }

            if (Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus)) == SCALE_COMMAND_STATUS_OK)
            { 
                return true;
            }

            return false;
        }

        ///<inheritdoc/>
        protected override void ProcessDataUpdateTick(object info)
        {
            if (IsConnected)
            {
                ProcessDataReceived?.Invoke(this, new ProcessDataReceivedEventArgs(ProcessData));
            }
        }

    

        public FilterTypes FilterType(int i)
        {
            //Convert ascending integer into filter type
            if ((i > 13104)) i = 13105;
            else if ((i > 13088) && (i < 13094)) i = 13089;
            return (FilterTypes)i;
        }

        #endregion
    }
}