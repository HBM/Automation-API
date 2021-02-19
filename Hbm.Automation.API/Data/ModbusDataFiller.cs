// <copyright file="ModbusDataFiller.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Data
{
    using Hbm.Automation.Api.Utils;
    using Hbm.Automation.Api.Weighing.WTX.Modbus;
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Implementation of the interface IDataFiller for the filler mode.
    /// The class DataFillerModbus contains the data input word and data output words for the filler mode
    /// of WTX device 120 and 110 via Modbus.
    /// </summary>
    public class ModbusDataFiller : IDataFiller
    {

        #region ==================== constants & fields ====================
        private int _residualFlowTime;
        private int _targetFillingWeight;
        private int _coarseFlowCutOffPointSet;
        private int _fineFlowCutOffPointSet;
        private int _minimumFineFlow;
        private int _optimizationOfCutOffPoints;
        private int _maximumDosingTime;
        private int _startWithFineFlow;
        private int _coarseLockoutTime;
        private int _fineLockoutTime;
        private int _tareMode;
        private int _upperToleranceLimit;
        private int _lowerToleranceLimit;
        private int _minimumStartWeight;
        private int _emptyWeight;
        private int _tareDelay;
        private int _coarseFlowMonitoringTime;
        private int _coarseFlowMonitoring;
        private int _fineFlowMonitoring;
        private int _fineFlowMonitoringTime;
        private int _delayTimeAfterFineFlow;
        private int _activationTimeAfterFineFlow;
        private int _systematicDifference;
        private int _downwardsDosing;
        private int _valveControl;
        private int _emptyingMode;
        private INetConnection _connection;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Constructor of class DataFillerModbus : Initalizes values and connects 
        /// the eventhandler from Connection to the interal update method
        /// </summary>
        /// <param name="Connection">Target connection</param>
        public ModbusDataFiller(INetConnection Connection) : base()
        {
            _connection = Connection;

            _connection.UpdateData += UpdateFillerData;

            CoarseFlow = 0;
            FineFlow = 0;
            Ready = 0;
            ReDosing = 0;
            Emptying = 0;
            FlowError = 0;
            Alarm = 0;
            AdcOverUnderload = 0;
            FillingProcessStatus = 0;
            FillingResultCount = 0;
            FillingResult = 0;
            FillingResultMeanValue = 0;
            FillingResultStandardDeviation = 0;
            FillingResultTotalSum = 0;
            CurrentDosingTime = 0;
            CurrentCoarseFlowTime = 0;
            CurrentFineFlowTime = 0;
            ToleranceErrorPlus = 0;
            ToleranceErrorMinus = 0;
            StatusInput1 = 0;
            GeneralScaleError = 0;
            FineFlowCutOffPoint = 0;
            CoarseFlowCutOffPoint = 0;
            ParameterSetProduct = 0;
            MaxDosingTime = 0;
            _residualFlowTime = 0;
            _targetFillingWeight = 0;
            _coarseFlowCutOffPointSet = 0;
            _fineFlowCutOffPointSet = 0;
            _minimumFineFlow = 0;
            _optimizationOfCutOffPoints = 0;
            _maximumDosingTime = 0;
            _startWithFineFlow = 0;
            _coarseLockoutTime = 0;
            _fineLockoutTime = 0;
            _tareMode = 0;
            _upperToleranceLimit = 0;
            _lowerToleranceLimit = 0;
            _minimumStartWeight = 0;
            _emptyWeight = 0;
            _tareDelay = 0;
            _coarseFlowMonitoringTime = 0;
            _coarseFlowMonitoring = 0;
            _fineFlowMonitoring = 0;
            _fineFlowMonitoringTime = 0;
            _delayTimeAfterFineFlow = 0;
            _activationTimeAfterFineFlow = 0;
            _systematicDifference = 0;
            _downwardsDosing = 0;
            _valveControl = 0;
            _emptyingMode = 0;
        }
        #endregion

        #region ==================== events & delegates ====================

        /// <summary>
        /// Updates and converts the values from buffer
        /// </summary>
        /// <param name="sender">Connection class</param>
        /// <param name="e">EventArgs, Event argument</param>
        public void UpdateFillerData(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.IMDApplicationMode)) == 2 || Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.IMDApplicationMode)) == 3)  // If application mode = filler
                {
                    // Via Modbus and Jetbus IDs: 
                    MaxDosingTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.MDTMaximalFillingTime));
                    FineFlowCutOffPoint = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FFDFineFlowDisconnect));
                    CoarseFlowCutOffPoint = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.CFDCoarseFlowDisconnect));

                    _residualFlowTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.RFTResidualFlowTime));
                    _minimumFineFlow = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FFMMinimumFineFlow));
                    _optimizationOfCutOffPoints = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.OSNOptimization));
                    _maximumDosingTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.MDTMaximalFillingTime));
                    _coarseLockoutTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.CFTCoarseFlowTime));
                    _fineLockoutTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FFTFineFlowTime));
                    _tareMode = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.TMDTareMode));
                    
                    _upperToleranceLimit = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.UTLUpperToleranceLimit));
                    _lowerToleranceLimit = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.LTLLowerToleranceLimit));
                    _minimumStartWeight = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.MSWMinimumStartWeight));
                    _emptyWeight = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.EWTEmptyWeight));
                    _tareDelay = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.TADTareDelay));
                    
                    _coarseFlowMonitoringTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.CBTCoarseFlowMonitoringTime));
                    _coarseFlowMonitoring = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.CBKCoarseFlowMonitoring));
                    _fineFlowMonitoring = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FBKFineFlowMonitoring));

                    _fineFlowMonitoringTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FBTFineFlowMonitoringTime));                 
                    _systematicDifference = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.SYDSystematicDifference));               
                    _valveControl = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.VCTValveControl));
                    _emptyingMode = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.EMDEmptyingMode));
                    _delayTimeAfterFineFlow = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.DL1DosingDelay1));
                    _activationTimeAfterFineFlow = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.DL2DosingDelay2));

                    AdcOverUnderload = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateAdcOverUnderload));
                    LegalForTradeOperation = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateLegalForTradeOperation));
                    StatusInput1 = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateStatusInput1));
                    GeneralScaleError = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateGeneralScaleError));

                    CoarseFlow = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateCoarseFlow));
                    FineFlow = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateFineFlow));
                    Ready = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateReady));
                    ReDosing = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateReDosing));

                    Emptying = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateEmptying));
                    FlowError = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateFlowError));
                    Alarm = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateAlarm));
                    ToleranceErrorPlus = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateToleranceErrorPlus));

                    ToleranceErrorMinus = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FillingStateToleranceErrorMinus));
                    CurrentDosingTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.DSTDosingTime));
                    CurrentCoarseFlowTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.CFTCoarseFlowTime));
                    CurrentFineFlowTime = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FFTFineFlowTime));
                    
                    ParameterSetProduct = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.CurrentProductParameterSet));
                    _downwardsDosing = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.DMDDosingMode));
                    FillingResultTotalSum = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.SUMFillingResultSum));
                    
                    _targetFillingWeight = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FWTFillingTargetWeight));
                    _coarseFlowCutOffPointSet = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.CFDCoarseFlowDisconnect));
                    _fineFlowCutOffPointSet = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FFDFineFlowDisconnect));
                    _startWithFineFlow = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.FFLFirstFineFlow));  // Command 'Run_start_dosing' right

                    WeightMemDay = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.WeightMemDayStandard));
                    WeightMemMonth = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.WeightMemMonthStandard));
                    WeightMemYear = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.WeightMemYearStandard));
                    WeightMemSeqNumber = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.WeightMemSeqNumberStandard));
                    WeightMemGross = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.WeightMemGrossStandard));
                    WeightMemNet = Convert.ToInt32(_connection.ReadFromBuffer(ModbusCommands.WeightMemNetStandard));
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("KeyNotFoundException in class DataFillerModbus, update method");
            }
        }
        #endregion

        #region ======================== properties ========================

        // Get-properties - input words filler mode

        public int CoarseFlow { get; private set; }
        public int FineFlow { get; private set; }
        public int Ready { get; private set; }
        public int ReDosing { get; private set; }
        public int Emptying { get; private set; }
        public int FlowError { get; private set; }
        public int Alarm { get; private set; }
        public int AdcOverUnderload { get; private set; }
        public int FillingProcessStatus { get; private set; }
        public int FillingResultCount { get; private set; }
        public double FillingResult { get; private set; }
        public double FillingResultMeanValue { get; private set; }
        public double FillingResultStandardDeviation { get; private set; }
        public double FillingResultTotalSum { get; private set; }
        public int CurrentDosingTime { get; private set; }
        public int CurrentCoarseFlowTime { get; private set; }
        public int CurrentFineFlowTime { get; private set; }
        public int ToleranceErrorPlus { get; private set; }
        public int ToleranceErrorMinus { get; private set; }
        public int StatusInput1 { get; private set; }
        public int GeneralScaleError { get; private set; }
        public int FineFlowCutOffPoint { get; set; }
        public int CoarseFlowCutOffPoint { get; set; }
        public int ParameterSetProduct { get; private set; }
        public int MaxDosingTime { get; private set; }
        public int LegalForTradeOperation { get; private set; }
        public int WeightMemDay { get; private set; }
        public int WeightMemMonth { get; private set; }
        public int WeightMemYear { get; private set; }
        public int WeightMemSeqNumber { get; private set; }
        public int WeightMemGross { get; private set; }
        public int WeightMemNet { get; private set; }

        // Get/Set-properties - output words filler mode:

        public int ResidualFlowTime // Type : unsigned integer 16 Bit
        {
            get { return _residualFlowTime; }
            set
            {
                _connection.WriteInteger(ModbusCommands.RFTResidualFlowTime, value);
                this._residualFlowTime = value;
            }
        }
        public double TargetFillingWeight // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_targetFillingWeight, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.FWTFillingTargetWeight, MeasurementUtils.DoubleToDigit(value, decimals));
                this._targetFillingWeight = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public double CoarseFlowCutOffLevel // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_coarseFlowCutOffPointSet, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.CFDCoarseFlowDisconnect, MeasurementUtils.DoubleToDigit(value, decimals));
                this._coarseFlowCutOffPointSet = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public double FineFlowCutOffLevel // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_fineFlowCutOffPointSet, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.FFDFineFlowDisconnect, MeasurementUtils.DoubleToDigit(value, decimals));
                this._fineFlowCutOffPointSet = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public double MinimumFineFlow // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_minimumFineFlow, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.FFMMinimumFineFlow, MeasurementUtils.DoubleToDigit(value, decimals));
                this._minimumFineFlow = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public int OptimizationMode // Type : unsigned integer 8 Bit
        {
            get { return _optimizationOfCutOffPoints; }
            set
            {
                _connection.WriteInteger(ModbusCommands.OSNOptimization, value);
                this._optimizationOfCutOffPoints = value;
            }
        }
        public int MaxFillingTime // Type : unsigned integer 16 Bit
        {
            get { return _maximumDosingTime; }
            set
            {
                _connection.WriteInteger(ModbusCommands.MDTMaximalFillingTime, value);
                this._maximumDosingTime = value;
            }
        }
        public int StartWithFineFlow // Type : unsigned integer 16 Bit
        {
            get { return _startWithFineFlow; }
            set
            {
                _connection.WriteInteger(ModbusCommands.FFLFirstFineFlow, value);
                this._startWithFineFlow = value;
            }
        }
        public int CoarseLockoutTime // Type : unsigned integer 16 Bit
        {
            get { return _coarseLockoutTime; }
            set
            {
                _connection.WriteInteger(ModbusCommands.LTCLockoutTimeCoarseFlow, value);
                this._coarseLockoutTime = value;
            }
        }
        public int FineLockoutTime // Type : unsigned integer 16 Bit
        {
            get { return _fineLockoutTime; }
            set
            {
                _connection.WriteInteger(ModbusCommands.LTFLockoutTimeFineFlow, value);
                this._fineLockoutTime = value;
            }
        }
        public int TareMode // Type : unsigned integer 8 Bit
        {
            get { return _tareMode; }
            set
            {
                _connection.WriteInteger(ModbusCommands.TMDTareMode, value);
                this._tareMode = value;
            }
        }
        public double UpperToleranceLimit // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_upperToleranceLimit, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.UTLUpperToleranceLimit, MeasurementUtils.DoubleToDigit(value, decimals));
                this._upperToleranceLimit = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public double LowerToleranceLimit // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_lowerToleranceLimit, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.LTLLowerToleranceLimit, MeasurementUtils.DoubleToDigit(value, decimals));
                this._lowerToleranceLimit = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public double MinimumStartWeight // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_minimumStartWeight, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.MSWMinimumStartWeight, MeasurementUtils.DoubleToDigit(value, decimals));
                this._minimumStartWeight = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public double EmptyWeight // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_emptyWeight, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.EWTEmptyWeight, MeasurementUtils.DoubleToDigit(value, decimals));
                this._emptyWeight = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public int TareDelay // Type : unsigned integer 16 Bit
        {
            get { return _tareDelay; }
            set
            {
                _connection.WriteInteger(ModbusCommands.TADTareDelay, value);
                this._tareDelay = value;
            }
        }
        public int CoarseFlowMonitoringTime // Type : unsigned integer 16 Bit
        {
            get { return _coarseFlowMonitoringTime; }
            set
            {
                _connection.WriteInteger(ModbusCommands.CBTCoarseFlowMonitoringTime, value);
                this._coarseFlowMonitoringTime = value;
            }
        }
        public double CoarseFlowMonitoring  // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_coarseFlowMonitoring, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.CBKCoarseFlowMonitoring, MeasurementUtils.DoubleToDigit(value, decimals));
                this._coarseFlowMonitoring = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public double FineFlowMonitoring  // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_fineFlowMonitoring, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.FBKFineFlowMonitoring, MeasurementUtils.DoubleToDigit(value, decimals));
                this._fineFlowMonitoring = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public int FineFlowMonitoringTime // Type : unsigned integer 16 Bit
        {
            get { return _fineFlowMonitoringTime; }
            set
            {
                _connection.WriteInteger(ModbusCommands.FBTFineFlowMonitoringTime, value);
                this._fineFlowMonitoringTime = value;
            }
        }
        public int DelayTimeAfterFilling  // Type : unsigned integer 8 Bit
        {
            get { return _delayTimeAfterFineFlow; }
            set
            {
                _connection.WriteInteger(ModbusCommands.DL1DosingDelay1, value);
                this._delayTimeAfterFineFlow = value;
            }
        }
        public int ActivationTimeAfterFilling  // Type : unsigned integer 8 Bit
        {
            get { return _activationTimeAfterFineFlow; }
            set
            {
                _connection.WriteInteger(ModbusCommands.DL2DosingDelay2, value);
                this._activationTimeAfterFineFlow = value;
            }
        }
        public double SystematicDifference // Type : double value in weight unit
        {
            get {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                return MeasurementUtils.DigitToDouble(_systematicDifference, decimals); }
            set
            {
                int decimals = _connection.ReadIntegerFromBuffer(ModbusCommands.CIA461Decimals);
                _connection.WriteInteger(ModbusCommands.SYDSystematicDifference, MeasurementUtils.DoubleToDigit(value, decimals));
                this._systematicDifference = MeasurementUtils.DoubleToDigit(value, decimals);
            }
        }
        public int FillingMode
        {
            get { return _downwardsDosing; }
            set
            {
                _connection.WriteInteger(ModbusCommands.DMDDosingMode, value);
                this._downwardsDosing = value;
            }
        }
        public int ValveControl  // Type : unsigned integer 8 Bit
        {
            get { return _valveControl; }
            set
            {
                _connection.WriteInteger(ModbusCommands.VCTValveControl, value);
                this._valveControl = value;
            }
        }

        public int EmptyingMode  // Type : unsigned integer 8 Bit
        {
            get { return _emptyingMode; }
            set
            {
                _connection.WriteInteger(ModbusCommands.EMDEmptyingMode, value);
                this._emptyingMode = value;
            }
        }
        #endregion

        #region ================ public & internal methods ================= 
        public void StartFilling()
        {
        }

        public void BreakFilling()
        {
        }


        public void ClearFillingResult()
        {
        }
        #endregion
    }
}