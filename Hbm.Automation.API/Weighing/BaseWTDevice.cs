// <copyright file="BaseWTDevice.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing
{
    using System;
    using System.Threading;
    using Hbm.Automation.Api.Data;

    /// <summary>
    /// Basic device class, holds the most important properties, available in all weighing devices.
    /// <para>- Weight data properties (e.g. Gross/Net value or tare value) and methods (e.g. Tare(), Zero() )</para>
    /// <para>- Adjustment  properties (e.g. zero signal or nominal signal) and methods (e.g. CalibrateZero() )</para>
    /// </summary>
    public abstract class BaseWTDevice
    {
        #region ==================== constants & fields ====================

        /// <summary>
        /// Internal timer for sending <see cref="ProcessDataReceived"/>  event
        /// </summary>
        private int _processDataInterval = 500;

        #endregion

        #region ==================== events & delegates ====================

        /// <summary>
        /// Event for receiving process data in the desired interval (<see cref="ProcessDataInterval"/>
        /// </summary>
        public abstract event EventHandler<ProcessDataReceivedEventArgs> ProcessDataReceived;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWTDevice" /> class.
        /// </summary>
        /// <param name="connection">Target connection of the device</param>
        /// <param name="timerIntervalms">Interval for updating ProcessData</param>
        public BaseWTDevice(INetConnection connection, int timerIntervalms)           
        {
            Connection = connection;
            _processDataInterval = timerIntervalms;
            ProcessDataTimer = new Timer(ProcessDataUpdateTick, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWTDevice" /> class.
        /// </summary>
        /// <param name="connection">Target connection of the device</param>
        public BaseWTDevice(INetConnection connection) : this(connection, 500)
        {            
        }
        #endregion

        #region ======================== properties ========================
        /// <summary>
        /// Gets or sets the current process data (e.g. weighing data and device status)
        /// </summary>
        public IProcessData ProcessData { get; protected set; }

        /// <summary>
        /// Gets or sets the current application mode of the device (e.g. Standard or filler)
        /// </summary>
        public abstract ApplicationMode ApplicationMode { get; set; }

        /// <summary>
        ///  Gets or sets the current connection of the device
        /// </summary>
        public INetConnection Connection { get; protected set; }

        /// <summary>
        ///  Gets a value indicating whether the device is connected or not
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        /// Gets the type of the connection (e.g. "JET" or "Modbus")
        /// </summary>
        public abstract string ConnectionType { get; }

        /// <summary>
        /// Gets or sets the interval for updating process data
        /// </summary>
        public int ProcessDataInterval
        {
            get
            {
                return _processDataInterval;
            }

            set
            {
                _processDataInterval = value;
                Restart();
            }
        }

        /// <summary>
        /// Gets or sets the device identification (e.g. "WTX120")
        /// </summary>
        public abstract string Identification { get; set; }

        /// <summary>
        /// Gets the serial number
        /// </summary>
        public abstract string SerialNumber { get; }

        /// <summary>
        /// Gets the firmware version
        /// </summary>
        public abstract string FirmwareVersion { get; }

        /// <summary>
        /// Gets the current weight values (net, gross and tare) of the device depending as double
        /// </summary>
        /// <returns>Current weight values</returns>
        public abstract WeightType Weight { get; }

        /// <summary>
        /// Gets the current weight values (net, gross and tare) of the device depending as string (e.g. "12,3")
        /// </summary>
        /// <returns>Current weight values</returns>
        public abstract PrintableWeightType PrintableWeight { get; }

        /// <summary>
        /// Gets or sets the engineering unit (e.g. "g", "kg", "t") 
        /// </summary>
        /// <returns></returns>
        public abstract string Unit { get; set; }

        /// <summary>
        /// Gets a value indicating whether a general scale error is active or not (e.g. if no sensor is connected)
        /// </summary>
        public abstract bool GeneralScaleError { get; }

        /// <summary>
        /// Gets the weight type, Gross or Net
        /// </summary>
        /// <returns></returns>
        public abstract TareMode TareMode { get; }

        /// <summary>
        /// Gets a value indicating whether the weight is in stable condition or not
        /// </summary>
        /// <returns></returns>
        public abstract bool WeightStable { get;  }

        /// <summary>
        /// Gets the scale range 1-3
        /// </summary>
        /// <returns></returns>
        public abstract int ScaleRange { get; }

        /// <summary>
        /// Gets or sets the manual tare value
        /// </summary>
        public abstract double ManualTareValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum capacity
        /// </summary>
        public abstract int MaximumCapacity { get; set; }

        /// <summary>
        /// Gets or sets the calibration weight for the next adjustment
        /// </summary>
        public abstract double CalibrationWeight { get; set; }

        /// <summary>
        /// Gets the zero value in nV/V
        /// </summary>
        public abstract double ZeroValue { get; }

        /// <summary>
        /// Gets or sets the zero signal
        /// </summary>
        public abstract int ZeroSignal { get; set; }

        /// <summary>
        /// Gets or sets the nominal signal
        /// </summary>
        public abstract int NominalSignal { get; set; }

        /// <summary>
        /// Gets or sets the process data timer 
        /// </summary>
        protected Timer ProcessDataTimer { get; set; }

        #endregion

        #region ================ public & internal methods =================

        /// <summary>
        /// Synchronous call to connect
        /// </summary>
        /// <param name="timeoutMs">Timeout to wait for connect response</param>
        public abstract void Connect(double timeoutMs);

        /// <summary>
        /// Asynchronous call to connect
        /// </summary>
        /// <param name="completed">Callback raised after connection completed</param>
        /// <param name="timeoutMs">Timeout to wait for connect response</param>
        public abstract void Connect(Action<bool> completed, double timeoutMs);

        /// <summary>
        /// Synchronous call to disconnect
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Asynchronous call to disconnect
        /// </summary>
        /// <param name="disconnectCompleted">Callback raised after disconnect completed</param>
        public abstract void Disconnect(Action<bool> disconnectCompleted);
               
        /// <summary>
        /// Sets the the device to gross
        /// </summary>
        public abstract void SetGross();

        /// <summary>
        /// Zeroes the device
        /// </summary>
        public abstract void Zero();

        /// <summary>
        /// Tares the device
        /// </summary>
        public abstract void Tare();

        /// <summary>
        /// Tares the device manually
        /// </summary>
        /// <param name="manualTareValue">Tare value in weight unit (e.g. 2.0)</param>
        public abstract void TareManually(double manualTareValue);

        /// <summary>
        /// Records the weight to legal-for-trade memory
        /// </summary>
        public abstract void RecordWeight();

        /// <summary>
        /// Adjust the zero signal 
        /// </summary>
        /// <returns>Adjustment success status</returns>
        public abstract bool AdjustZeroSignal();

        /// <summary>
        /// Adjust the nominal signal 
        /// </summary>
        /// <returns>Adjustment success status</returns>
        public abstract bool AdjustNominalSignal();

        /// <summary>
        /// Adjust the nominal signal with an individual adjustment weight. 
        /// </summary>
        /// <param name="adjustmentWeight">Weight for the next adjustment</param>
        /// <returns>Adjustment success status</returns>
        public abstract bool AdjustNominalSignalWithCalibrationWeight(double adjustmentWeight);

        /// <summary>
        /// Adjust manually with calculated zero signal and span in weight unit. 
        /// </summary>
        /// <param name="scaleZeroSingal">Zero load for calculating the adjustment</param>
        /// <param name="scaleCapacity">Scale capacity for calculating the adjustment</param>
        public abstract void CalculateAdjustment(double scaleZeroSingal, double scaleCapacity);

        /// <summary>
        /// Stop updating process data
        /// </summary>
        public void Stop()
        {
            ProcessDataTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        
        /// <summary>
        /// Restart updating process data
        /// </summary>        
        public void Restart()
        {
            ProcessDataTimer.Change(0, _processDataInterval);
        }

        /// <summary>
        /// Saves all parameters in non-volatile memory
        /// </summary>
        public abstract void SaveAllParameters();

        /// <summary>
        /// Restores all parameters from non-volatile memory
        /// </summary>
        public abstract void RestoreAllDefaultParameters();

        /// <summary>
        /// Tick handler for the ProcessData timer
        /// </summary>
        /// <param name="info">Unused info for timer</param>
        protected abstract void ProcessDataUpdateTick(object info);

        #endregion
    }
}
