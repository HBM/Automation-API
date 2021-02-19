// <copyright file="WTXModbus.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Utils;
    using Hbm.Automation.Api.Weighing.WTX.Modbus;

    /// <summary>
    /// This class represents a WTX device with the Modbus/TCP interface.
    /// Subscribe to ProcessDataReceived over the constructor to automatically get weight values.
    /// </summary>
    ///
    public class WTXModbus : BaseWTXDevice
    {
        #region ==================== constants & fields ====================        
        private const string NOT_AVAILABLE_VIA_MODBUS = "Not available via WTX Modbus interface";
        private double _manualTareValue;
        private bool _result;
        #endregion

        #region ==================== events & delegates ====================
        /// <summary>
        /// Event handler to raise an event whenever new process data from the device is available
        /// </summary>
        public override event EventHandler<ProcessDataReceivedEventArgs> ProcessDataReceived;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Initializes a new instance of the <see cref="WTXJet" /> class.
        /// </summary>
        /// <param name="connection">Inject connection (e.g. JetBusConnection)</param>
        /// <param name="onProcessData">This event is automatically called when new ProcessData is available</param>
        public WTXModbus(INetConnection connection, int timerIntervalms, EventHandler<ProcessDataReceivedEventArgs> onProcessData)
            : base(connection, timerIntervalms)
        {
            ProcessData = new ModbusDataProcess(base.Connection);
            LimitSwitch = new ModbusDataLimitSwitch(base.Connection);
            DigitalIO = new ModbusDataDigitalIO(base.Connection);
            ProcessDataReceived += onProcessData;
        }

        /// <summary>
        /// For a more simple solution : Constructor with no asynchronous callback and no timer interval for continously updating the data. 
        /// </summary>
        /// <param name="connection"></param>
        public WTXModbus(INetConnection connection) : base(connection)
        {
            ProcessData = new ModbusDataProcess(base.Connection);
            Filler = new ModbusDataFiller(base.Connection);
            DigitalIO = new ModbusDataDigitalIO(base.Connection);
            LimitSwitch = new ModbusDataLimitSwitch(base.Connection);
        }
        #endregion

        #region ======================== properties ========================

        /// <summary>
        /// Gets or sets the current limit switch data 
        /// </summary>
        public override IDataLimitSwitch LimitSwitch { get; set; }

        /// <summary>
        /// Gets or sets the current digital IO data 
        /// </summary>
        public override IDataDigitalIO DigitalIO { get; set; }

        /// <summary>
        /// Gets or sets the current filler data 
        /// </summary>
        public IDataFiller Filler { get; set; }

        ///<inheritdoc/>
        public override bool IsConnected
        {
            get
            {
                return Connection.IsConnected;
            }
        }

        ///<inheritdoc/>
        public override string ConnectionType
        {
            get
            {
                return "Modbus";
            }
        }

        ///<inheritdoc/>
        public override void Connect(double timeoutMs = 2000)
        {
            this.Connection.Connect();
            ProcessDataTimer.Change(0, ProcessDataInterval);
        }

        ///<inheritdoc/>
        public override void Connect(Action<bool> ConnectCompleted, double timeoutMs)
        {
            this.Connection.Connect();
            ProcessDataTimer.Change(0, ProcessDataInterval);
        }

        ///<inheritdoc/>
        public override void Disconnect(Action<bool> DisconnectCompleted)
        {
            this.Connection.Disconnect();
        }

        ///<inheritdoc/>
        public override void Disconnect()
        {
            this.Connection.Disconnect();
        }
        
        ///<inheritdoc/>
        protected override void ProcessDataUpdateTick(object info)
        {
            if (IsConnected)
            {
                ((ModbusTCPConnection)Connection).SyncData();
                ProcessDataReceived?.Invoke(this, new ProcessDataReceivedEventArgs(ProcessData));
            }
        }

        ///<inheritdoc/>
        public override string SerialNumber => throw new NotImplementedException(NOT_AVAILABLE_VIA_MODBUS);

        ///<inheritdoc/>
        public override string Identification { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ///<inheritdoc/>
        public override string FirmwareVersion => throw new NotImplementedException(NOT_AVAILABLE_VIA_MODBUS);

        ///<inheritdoc/>
        public override int MaximumCapacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ///<inheritdoc/>
        public override void RestoreAllDefaultParameters()
        {
            throw new NotImplementedException(NOT_AVAILABLE_VIA_MODBUS);
        }

        ///<inheritdoc/>
        public override void SaveAllParameters()
        {
            throw new NotImplementedException(NOT_AVAILABLE_VIA_MODBUS);
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
        public override double ManualTareValue 
        {
            get
            {
                return _manualTareValue;
            }
            set
            {
                _manualTareValue = value;
                Connection.WriteInteger(ModbusCommands.CIA461TareValue, MeasurementUtils.DoubleToDigit(value, ProcessData.Decimals));                
            }
        }

        ///<inheritdoc/>
        public override bool GeneralScaleError
        {
            get
            {
                return ProcessData.GeneralScaleError;
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
                throw new NotSupportedException();
            }

        }

        ///<inheritdoc/>
        public override double CalibrationWeight { get; set; }

        public override double ZeroValue { get; }

        ///<inheritdoc/>
        public override int ZeroSignal { get; set; }

        ///<inheritdoc/>
        public override int NominalSignal { get; set; }
        #endregion

        #region ================ public & internal methods =================
        ///<inheritdoc/>
        public override bool AdjustZeroSignal()
        {
            Stop();                        
            Connection.WriteInteger(ModbusCommands.LDWZeroSignal, 0x7FFFFFFF);
            _result = Connection.WriteInteger(ModbusCommands.ControlWordAdjustZero, 1);
            Restart();
            return _result;
        }

        ///<inheritdoc/>
        public override bool AdjustNominalSignal()
        {
            this.Stop();
            Connection.WriteInteger(ModbusCommands.LWTNominalSignal, 0x7FFFFFFF);
            _result = Connection.WriteInteger(ModbusCommands.ControlWordAdjustZero, 1);
            this.Restart();
            return _result;
        }

        ///<inheritdoc/>
        public override bool AdjustNominalSignalWithCalibrationWeight(double adjustmentWeight)
        {
            Stop();
            Connection.WriteInteger(ModbusCommands.CWTScaleCalibrationWeight, MeasurementUtils.DoubleToDigit(adjustmentWeight,ProcessData.Decimals));    
            Connection.WriteInteger(ModbusCommands.LWTNominalSignal, 0x7FFFFFFF);          
            _result = Connection.WriteInteger(ModbusCommands.ControlWordAdjustNominal, 1);
            this.Restart();
            return _result;
        }

        ///<inheritdoc/>
        public override void CalculateAdjustment(double preload, double capacity)
        {
            double multiplierMv2D = 500000.0;
            int _preLoad = (int)(preload * multiplierMv2D);
            int _nominalLoad = (int)(preload + (capacity * multiplierMv2D));
                                       
            Stop();       
            Connection.WriteInteger(ModbusCommands.LDWZeroSignal, _preLoad);
            Connection.WriteInteger(ModbusCommands.ControlWordAdjustZero, 1);  
            Connection.WriteInteger(ModbusCommands.LWTNominalSignal, _nominalLoad);
            Connection.WriteInteger(ModbusCommands.ControlWordAdjustNominal, 1);
            Restart();
        }

        ///<inheritdoc/>
        public override void SetGross()
        {
            Connection.Write(ModbusCommands.ControlWordGrossNet, "1");
        }

        ///<inheritdoc/>
        public override void Tare()
        {
            Connection.Write(ModbusCommands.ControlWordTare, "1");
        }

        ///<inheritdoc/>
        public override void Zero()
        {
            Connection.Write(ModbusCommands.ControlWordZeroing, "1");
        }
        
        /// <summary>
        /// Activate data after transmitting ober Modbus
        /// </summary>
        public void ActivateData()
        {
            Connection.Write(ModbusCommands.ControlWordActivateData, "1");
        }

        ///<inheritdoc/>
        public override void TareManually(double manualTareValue)
        {
            Connection.Write(ModbusCommands.ControlWordTare, "1");
        }

        ///<inheritdoc/>
        public override void RecordWeight()
        {
            Connection.Write(ModbusCommands.ControlWordRecordWeight, "1");
        }
        #endregion

    }
}