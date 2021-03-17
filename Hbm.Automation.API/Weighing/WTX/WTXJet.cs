// <copyright file="WTXJet.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.WTX
{
    using System;
    using System.Threading;
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Utils;
    using Hbm.Automation.Api.Weighing.WTX.Jet;
    using Hbm.Automation.Api;

    /// <summary>
    /// This class represents a WTX device with the Jet ethernet interface.
    /// Subscribe to ProcessDataReceived over the constructor to automatically get weight values.
    /// </summary>
    public class WTXJet : BaseWTXDevice, IDataScale, IDataDigitalFilter
    {
        #region ==================== constants & fields ====================
        private const int CONVERISION_FACTOR_MVV_TO_D = 500000;  
        private const int SCALE_COMMAND_CALIBRATE_ZERO = 2053923171;
        private const int SCALE_COMMAND_CALIBRATE_NOMINAL = 1852596579;
        private const int SCALE_COMMAND_EXIT_CALIBRATE = 1953069157;
        private const int SCALE_COMMAND_TARE = 1701994868;
        private const int SCALE_COMMAND_CLEAR_PEAK_VALUES = 1801545072;
        private const int SCALE_COMMAND_ZERO = 1869768058;
        private const int SCALE_COMMAND_SET_GROSS = 1936683623;
        private const int SCALE_COMMAND_STATUS_ONGOING = 1634168417;
        private const int SCALE_COMMAND_STATUS_OK = 1801543519;
        private const int SCALE_COMMAND_STATUS_ERROR_E1 = 826629983;
        private const int SCALE_COMMAND_STATUS_ERROR_E2 = 843407199;
        private const int SCALE_COMMAND_STATUS_ERROR_E3 = 860184415;
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
        /// Initializes a new instance of the <see cref="WTXJet" /> class.
        /// </summary>
        /// <param name="connection">Inject connection (e.g. JetBusConnection)</param>
        /// <param name="timerIntervalms">Interval for updating ProcessData</param>
        /// <param name="onProcessData">This event is automatically called when new ProcessData is available</param>
        public WTXJet(INetConnection connection, int timerIntervalms, EventHandler<ProcessDataReceivedEventArgs> onProcessData) 
            : base(connection, timerIntervalms)
        {
            _connection = connection;
            ProcessData = new JetProcessData(Connection);
            DigitalIO = new JetDataDigitalIO(Connection);
            Filler = new JetDataFillerExtended(Connection);
            LimitSwitch = new JetDataLimitSwitch(Connection);
            ProcessDataReceived += onProcessData;
        }
        #endregion

        #region ======================== Properties ========================
        /// <summary>
        /// Gets or sets the extended filler data 
        /// </summary>
        public IDataFillerExtended Filler { get; set; }

        /// <summary>
        /// Gets or sets the digital IO data 
        /// </summary>
        public override IDataDigitalIO DigitalIO { get; set; }

        /// <summary>
        /// Gets or sets the extended limit switch data 
        /// </summary>
        public override IDataLimitSwitch LimitSwitch { get; set; }

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

        /// <summary>
        /// Gets last error code
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return Connection.ReadIntegerFromBuffer(JetBusCommands.ESRErrorRegister);
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
                int readValue = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461CalibrationWeight);
                return MeasurementUtils.DigitToDouble(readValue, ProcessData.Decimals);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461CalibrationWeight, MeasurementUtils.DoubleToDigit(value, ProcessData.Decimals));
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
                Connection.WriteInteger(JetBusCommands.CIA461CalibrationWeight, MeasurementUtils.DoubleToDigit(value,ProcessData.Decimals));
            }
        }

        ///<inheritdoc/>
        public override double ZeroValue
        {
            get
            {
                int decimals = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_connection.ReadIntegerFromBuffer(JetBusCommands.CIA461ZeroValue), decimals);
            }
        }

        ///<inheritdoc/>
        public override int ZeroSignal
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.LDWZeroValue)*2;
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.LDWZeroValue, value);
            }
        }

        ///<inheritdoc/>
        public override int NominalSignal
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.LWTNominalValue)*2;
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.LWTNominalValue, value);
            }
        }

        /// <summary>
        /// Gets the gross maximum peak value
        /// </summary>
        public int MaximumPeakValueGross
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461PeakValueGrossMax);
            }
        }

        /// <summary>
        /// Gets the the gross minimum peak value
        /// </summary>
        public int MinimumPeakValueGross
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461PeakValueGrossMin);
            }
        }

        /// <summary>
        /// Gets the net maximum peak value
        /// </summary>
        public int MaximumPeakValue
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461PeakValueMax); }
        }

        /// <summary>
        /// Gets the net minimum peak value
        /// </summary>
        public int MinimumPeakValue
        {
            get { return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461PeakValuMin); }
        }

        /// <summary>
        /// Gets the vendor id
        /// </summary>
        public int VendorID
        {
            get
            {
                return Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461VendorID));
            }
        }

        /// <summary>
        /// Gets the product code 
        /// </summary>
        public int ProductCode
        {
            get
            {
                return Connection.ReadIntegerFromBuffer(JetBusCommands.CIA461ProductCode);
            }
        }

        ///<inheritdoc/>
        public override string SerialNumber
        {
            get
            {
                return Connection.ReadFromBuffer(JetBusCommands.CIA461SerialNumber);
            }
        }

        ///<inheritdoc/>
        public override string Identification
        { 
            get
            {
                return Connection.ReadFromBuffer(JetBusCommands.HWVHardwareVersion);
            }

            set
            {
                Connection.Write(JetBusCommands.IDNDeviceIdentification, value);
            }
        }

        /// <summary>
        /// Gets the hardware version (e.g. WTX120)
        /// </summary>
        public string HardwareVersion
        {
            get
            {
                return Connection.ReadFromBuffer(JetBusCommands.HWVHardwareVersion);
            }
        }

        ///<inheritdoc/>
        public override string FirmwareVersion
        {
            get
            {
                return Connection.ReadFromBuffer(JetBusCommands.SWVSoftwareVersion);
            }
        }

        /// <summary>
        /// Gets the legal-for-trade software identification  
        /// </summary>
        public string SoftwareIdentification 
        {
            get
            {
                return Connection.ReadFromBuffer(JetBusCommands.SWISoftwareIdentification);
            }
        }

        /// <summary>
        /// Gets the firmware date
        /// </summary>
        public string FirmwareDate
        {
            get
            {
                return Connection.ReadFromBuffer(JetBusCommands.PDTFirmwareDate);
            }
        }

        /// <summary>
        /// Gets or sets the local gravity factor
        /// </summary>
        public int LocalGravityFactor 
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461LocalGravityFactor);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461LocalGravityFactor, value);
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


        ///<inheritdoc/>
        public int DataRate
        {
            get
            {
                return _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461SampleRate);
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461SampleRate, value);
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
                        return LowPassFilter.Critically_damped;
                    case 24738:
                        return LowPassFilter.Bessel;
                    case 60261:
                        return LowPassFilter.Butterworth;
                    default:
                        return LowPassFilter.Standard;
                }
            }

            set
            {
                int toWrite = 0;
                switch (value)
                {
                    case LowPassFilter.Standard:
                        break;
                    case LowPassFilter.Critically_damped:
                        toWrite = 24737;
                        break;
                    case LowPassFilter.Bessel:
                        toWrite = 24738;
                        break;
                    case LowPassFilter.Butterworth:
                        toWrite = 60261;
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
                int __value;
                switch (LowPassFilterMode)
                {
                    case LowPassFilter.Critically_damped: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterCriticallyDampedCutOffFrequency); break;
                    case LowPassFilter.Bessel: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterBesselCutOffFrequency); break;
                    default:
                    case LowPassFilter.Butterworth: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterButterworthCutOffFrequency); break;
                }
                return __value;
            }

            set
            {
                switch (LowPassFilterMode)
                {
                    case LowPassFilter.Critically_damped: Connection.WriteInteger(JetBusCommands.CIA461FilterCriticallyDampedCutOffFrequency, value); break;
                    case LowPassFilter.Bessel: Connection.WriteInteger(JetBusCommands.CIA461FilterBesselCutOffFrequency, value); break;
                    case LowPassFilter.Butterworth: Connection.WriteInteger(JetBusCommands.CIA461FilterButterworthCutOffFrequency, value); break;
                }
            }
        }

        ///<inheritdoc/>
        public int LowPassFilterOrder
        {
            get
            {
                int __value;
                switch (LowPassFilterMode)
                {
                    case LowPassFilter.Critically_damped: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterCriticallyDampedFilterOrder); break;
                    case LowPassFilter.Bessel: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterBesselFilterOrder); break;
                    default:
                    case LowPassFilter.Butterworth: __value = _connection.ReadIntegerFromBuffer(JetBusCommands.CIA461FilterButterworthFilterOrder); break;
                }
                return __value;
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.CIA461FilterCriticallyDampedFilterOrder, value);
                Connection.WriteInteger(JetBusCommands.CIA461FilterBesselFilterOrder, value);
                Connection.WriteInteger(JetBusCommands.CIA461FilterButterworthFilterOrder, value);
            }
        }

        /// <summary>
        /// Gets or sets the input function of digital input 1
        /// </summary>
        public InputFunction Input1Function
        {
            get
            {
                return IntToInputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.IM1DigitalInput1Mode)));
             }

            set
            {
                Connection.WriteInteger(JetBusCommands.IM1DigitalInput1Mode, InputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the input function of digital input 2
        /// </summary>
        public InputFunction Input2Function
        {
            get
            {
                return IntToInputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.IM2DigitalInput2Mode)));
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.IM2DigitalInput2Mode, InputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the input function of digital input 3
        /// </summary>
        public InputFunction Input3Function
        {
            get
            {
                return IntToInputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.IM3DigitalInput3Mode)));
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.IM2DigitalInput2Mode, InputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the input function of digital input 4
        /// </summary>
        public InputFunction Input4Function
        {
            get
            {
                return IntToInputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.IM4DigitalInput4Mode)));
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.IM4DigitalInput4Mode, InputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the output function of digital output 1
        /// </summary>
        public OutputFunction Output1Function
        { 
            get
            {
                return IntToOutputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.OM1DigitalOutput1Mode)));
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.OM1DigitalOutput1Mode, OutputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the output function of output 2
        /// </summary>
        public OutputFunction Output2Function
        {
            get
            {
                return IntToOutputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.OM2DigitalOutput2Mode)));
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.OM2DigitalOutput2Mode, OutputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the output function of digital output 3
        /// </summary>
        public OutputFunction Output3Function
        {
            get
            {
                return IntToOutputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.OM3DigitalOutput3Mode)));
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.OM3DigitalOutput3Mode, OutputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the output function of digital output 4
        /// </summary>
        public OutputFunction Output4Function
        {
            get
            {
                return IntToOutputFunction(Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.OM4DigitalOutput4Mode)));
            }

            set
            {
                Connection.WriteInteger(JetBusCommands.OM4DigitalOutput4Mode, OutputFunctionToInt(value));
            }
        }

        /// <summary>
        /// Gets or sets the maximum zeroing time (WTX device)
        /// </summary>
        public int MaximumZeroingTime { get; set; }

        /// <summary>
        /// Gets or sets the current time (WTX device)
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
            Connection.WriteInteger(JetBusCommands.CIA461RestoreAllDefaultParameters, 0);
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
            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_TARE);
        }
        
        ///<inheritdoc/>
        public override void TareManually(double manualTareValue)
        {
            Connection.WriteInteger(JetBusCommands.CIA461TareValue, MeasurementUtils.DoubleToDigit(manualTareValue, ProcessData.Decimals));
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
                        
            Connection.WriteInteger(JetBusCommands.LDWZeroValue, scalZeroLoad_d); 
            Connection.WriteInteger(JetBusCommands.LWTNominalValue, Convert.ToInt32(scaleCapacity_d)); 
        }

        ///<inheritdoc/>
        public override bool AdjustZeroSignal()
        {
            Connection.WriteInteger(JetBusCommands.CIA461ScaleCommand, SCALE_COMMAND_CALIBRATE_ZERO);

            int com = Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus));
            while ((com != SCALE_COMMAND_STATUS_ONGOING) && (com != SCALE_COMMAND_STATUS_OK))
            {
                Thread.Sleep(10);
                com = Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus));
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

            int com = Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus));
            while ((com != SCALE_COMMAND_STATUS_ONGOING) && (com != SCALE_COMMAND_STATUS_OK))
            {
                Thread.Sleep(10);
                com = Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus));
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


            int com = Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus));
            while ((com != SCALE_COMMAND_STATUS_ONGOING) && (com != SCALE_COMMAND_STATUS_OK))
            {
                Thread.Sleep(10);
                com = Convert.ToInt32(Connection.ReadFromBuffer(JetBusCommands.CIA461ScaleCommandStatus));
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

        private InputFunction IntToInputFunction(int inputMode)
        {
            InputFunction _result = InputFunction.Off;
            switch (inputMode)
            {
                case 0: _result = InputFunction.Off; break;
                case 1: _result = InputFunction.Tare; break;
                case 2: _result = InputFunction.Trigger; break;
                case 4: _result = InputFunction.BreakFilling; break;
                case 5: _result = InputFunction.RunFilling; break;
                case 6: _result = InputFunction.Redosing; break;
                case 7: _result = InputFunction.RecordWeight; break;
                case 8: _result = InputFunction.Zero; break;
            }
            return _result;
        }

        private int InputFunctionToInt(InputFunction inputMode)
        {
            int _result = 0;
            switch (inputMode)
            {
                case InputFunction.Off: _result = 0; break;
                case InputFunction.Tare: _result = 1; break;
                case InputFunction.Trigger: _result = 2; break;
                case InputFunction.BreakFilling: _result = 4; break;
                case InputFunction.RunFilling: _result = 5; break;
                case InputFunction.Redosing: _result = 6; break;
                case InputFunction.RecordWeight: _result = 7; break;
                case InputFunction.Zero: _result = 8; break;
            }
            return _result;
        }
        private OutputFunction IntToOutputFunction(int outputMode)
        {
            OutputFunction _result = OutputFunction.Off;
            switch (outputMode)
            {
                case 0: _result = OutputFunction.Off; break;
                case 1: _result = OutputFunction.Manually; break;
                case 2: _result = OutputFunction.LimitSwitch1; break;
                case 3: _result = OutputFunction.LimitSwitch2; break;
                case 4: _result = OutputFunction.LimitSwitch3; break;
                case 5: _result = OutputFunction.LimitSwitch4; break;
                case 7: _result = OutputFunction.CoarseFlow; break;
                case 8: _result = OutputFunction.FineFlow; break;
                case 9: _result = OutputFunction.Ready; break;
                case 10: _result = OutputFunction.ToleranceExceeded; break;
                case 11: _result = OutputFunction.ToleranceUnderrun; break;
                case 12: _result = OutputFunction.ToleranceExceededUnderrun; break;
                case 13: _result = OutputFunction.Alert; break;
                case 14: _result = OutputFunction.DL1DL2; break;
                case 21: _result = OutputFunction.Empty; break;
                case 22: _result = OutputFunction.DeviceStatus; break;
            }
            return _result;
        }

        private int OutputFunctionToInt(OutputFunction outputMode)
        {
            int _result = 0;
            switch (outputMode)
            {
                case OutputFunction.Off: _result = 0; break;
                case OutputFunction.Manually: _result = 1; break;
                case OutputFunction.LimitSwitch1: _result = 2; break;
                case OutputFunction.LimitSwitch2: _result = 3; break;
                case OutputFunction.LimitSwitch3: _result = 4; break;
                case OutputFunction.LimitSwitch4: _result = 5; break;
                case OutputFunction.CoarseFlow: _result = 7; break;
                case OutputFunction.FineFlow: _result = 8; break;
                case OutputFunction.Ready: _result = 9; break;
                case OutputFunction.ToleranceExceeded: _result = 10; break;
                case OutputFunction.ToleranceUnderrun: _result = 11; break;
                case OutputFunction.ToleranceExceededUnderrun: _result = 12; break;
                case OutputFunction.Alert: _result = 13; break;
                case OutputFunction.DL1DL2: _result = 14; break;
                case OutputFunction.Empty: _result = 21; break;
                case OutputFunction.DeviceStatus: _result = 22; break;
            }
            return _result;
        }

        #endregion
    }
}