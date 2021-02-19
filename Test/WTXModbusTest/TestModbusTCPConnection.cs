// <copyright file="TestModbusTCPConnection.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using Hbm.Automation.Api;
    using Hbm.Automation.Api.Weighing.WTX.Modbus;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    public enum Behavior
    {

         ConnectionFail, 
         ConnectionSuccess,

         DisconnectionFail,
         DisconnectionSuccess,
         
         ReadFail,
         ReadSuccess,

         WriteFail,
         WriteSuccess,

         WriteSyncFail,
         WriteSyncSuccess,

         WriteArrayFail,
         WriteArraySuccess,

         MeasureZeroFail,
         MeasureZeroSuccess,

         TareFail,
         TareSuccess,

         AsyncWriteBackgroundworkerFail,
         AsyncWriteBackgroundworkerSuccess,

         HandshakeFail,
         HandshakeSuccess,

         CalibrationFail,
         CalibrationSuccess,

         InStandardMode,
         InFillerMode,

         LogEvent_Fail,
         LogEvent_Success,

         t_UnitValue_Fail,
         t_UnitValue_Success,
         kg_UnitValue_Success,
         kg_UnitValue_Fail,
         g_UnitValue_Success,
         g_UnitValue_Fail,
         lb_UnitValue_Success,
         lb_UnitValue_Fail,

         NetGrossValueStringComment_0D_Fail,
         NetGrossValueStringComment_0D_Success,
         NetGrossValueStringComment_1D_Fail,
         NetGrossValueStringComment_1D_Success,
         NetGrossValueStringComment_2D_Fail,
         NetGrossValueStringComment_2D_Success,
         NetGrossValueStringComment_3D_Fail,
         NetGrossValueStringComment_3D_Success,
         NetGrossValueStringComment_4D_Fail,
         NetGrossValueStringComment_4D_Success,
         NetGrossValueStringComment_5D_Fail,
         NetGrossValueStringComment_5D_Success,
         NetGrossValueStringComment_6D_Fail,
         NetGrossValueStringComment_6D_Success,

         ScaleRangeStringComment_Range1_Fail,
         ScaleRangeStringComment_Range1_Success,
         ScaleRangeStringComment_Range2_Fail,
         ScaleRangeStringComment_Range2_Success,
         ScaleRangeStringComment_Range3_Fail,
         ScaleRangeStringComment_Range3_Success,

         LimitStatusStringComment_Case0_Fail,
         LimitStatusStringComment_Case0_Success,
         LimitStatusStringComment_Case1_Fail,
         LimitStatusStringComment_Case1_Success,
         LimitStatusStringComment_Case2_Fail,
         LimitStatusStringComment_Case2_Success,
         LimitStatusStringComment_Case3_Fail,
         LimitStatusStringComment_Case3_Success,

         WeightMovingStringComment_Case0_Fail,
         WeightMovingStringComment_Case0_Success,
         WeightMovingStringComment_Case1_Fail,
         WeightMovingStringComment_Case1_Success,

         WeightTypeStringComment_Case0_Fail,
         WeightTypeStringComment_Case0_Success,
         WeightTypeStringComment_Case1_Fail,
         WeightTypeStringComment_Case1_Success,

         WriteHandshakeTestSuccess,
         WriteHandshakeTestFail,

         GrosMethodTestSuccess,
         GrosMethodTestFail, 

         TareMethodTestSuccess,
         TareMethodTestFail,

         ZeroMethodTestSuccess,
         ZeroMethodTestFail,

         AdjustingZeroMethodSuccess,
         AdjustingZeroMethodFail,

         AdjustNominalMethodTestSuccess,
         AdjustNominalMethodTestFail,

         ActivateDataMethodTestSuccess,
         ActivateDataMethodTestFail,

         ManualTaringMethodTestSuccess,
         ManualTaringMethodTestFail,

         ClearDosingResultsMethodTestSuccess,
         ClearDosingResultsMethodTestFail,

         AbortDosingMethodTestSuccess,
         AbortDosingMethodTestFail,

         StartDosingMethodTestSuccess,
         StartDosingMethodTestFail,
 
         RecordWeightMethodTestSuccess,
         RecordWeightMethodTestFail,

         ManualRedosingMethodTestSuccess,
         ManualRedosingMethodTestFail,

         WriteS32ArrayTestSuccess,
         WriteS32ArrayTestFail,
 
         WriteU16ArrayTestSuccess,
         WriteU16ArrayTestFail,

         ResetTimerTestSuccess,
        //ResetTimerTestFail,

         WriteU08ArrayTestSuccess,
         WriteU08ArrayTestFail,

         UpdateOutputTestSuccess,
         UpdateOutputTestFail,

         WriteLimitSwitch1ModeTestSuccess,
         WriteLimitSwitch1ModeTestFail,
         WriteLimitSwitch2ModeTestSuccess,
         WriteLimitSwitch2ModeTestFail,
         WriteLimitSwitch3ModeTestSuccess,
         WriteLimitSwitch3ModeTestFail,
         WriteLimitSwitch4ModeTestSuccess,
         WriteLimitSwitch4ModeTestFail,
    }

    public class TestModbusTCPConnection : INetConnection, IDisposable
    {
        private Behavior behavior;

        private ushort arrayElement1;
        private ushort arrayElement2;
        private ushort arrayElement3;
        private ushort arrayElement4;

        private bool _connected;

        private ushort[] _dataWTX;

        public int command;

        public event EventHandler<LogEventArgs> CommunicationLog;
        public event EventHandler<EventArgs> UpdateData;
        
        private Dictionary<string, string> _dataBuffer;

        private string IP;
        private int interval;
        private int wordNumberIndex; 

        private int numPoints;

        private ushort[] _data;

        public LogEventArgs _logObj;

        public TestModbusTCPConnection(Behavior behavior,string ipAddress) 
        {
            _dataWTX = new ushort[38];
            // size of 38 elements for the standard and filler application mode.            

             _dataBuffer = new Dictionary<string, string>();

            this.CreateDictionary();
           
            this.behavior = behavior;

            this.numPoints = 6;

             _data = new ushort[2];
            //_index = 0;

            for (int index = 0; index < _dataWTX.Length; index++)
                _dataWTX[index] = 0x00;

            _dataWTX[0] = 0x00;
            _dataWTX[1] = 0x2710;
            _dataWTX[2] = 0x00;
            _dataWTX[3] = 0x2710;
            _dataWTX[4] = 0x00;
            _dataWTX[5] = 0x00;
        }


        public void Connect(int timeoutMs = 20000)
        {
            switch(this.behavior)
            {
                case Behavior.ConnectionFail:
                    _connected = false;
                    break;

                case Behavior.ConnectionSuccess:
                    _connected = true;
                    break;

                default:
                    _connected = false;
                    break; 
            }

            WriteInteger(ModbusCommands.ControlWordTare, 0);
    }

        public bool IsConnected
        {
            get
            {
                return this._connected;
            }
            set
            {
                this._connected = value;
            }
        }

        public void Disconnect()
        {
            switch (this.behavior)
            {
                case Behavior.DisconnectionFail:
                    _connected = true;
                    break;

                case Behavior.DisconnectionSuccess:
                    _connected = false;
                    break;

                default:
                    _connected = true;
                    break;
            }
        }

        private void CreateDictionary()
        {
            _dataBuffer.Clear();

            Type pType = typeof(ModbusCommands);
            PropertyInfo[] pInfos = pType.GetProperties();
            foreach (PropertyInfo pInfo in pInfos)
            {
                object propertyValue = pInfo.GetValue(typeof(ModbusCommands), null);
                if (propertyValue != null)
                {
                    Type propertyValueType = propertyValue.GetType();

                    if (propertyValueType == typeof(ModbusCommand))
                        _dataBuffer.Add(((ModbusCommand)propertyValue).Path, "0");
                }
                else
                    Console.WriteLine("Prop: {0} ", pInfo.Name);
            }
        }

        private void UpdateDictionary()
        {
            this.ReadFromBuffer(ModbusCommands.CIA461NetValue);
            this.ReadFromBuffer(ModbusCommands.CIA461GrossValue);

            this.ReadFromBuffer(ModbusCommands.CIA461WeightStatusWeightMoving);
            this.ReadFromBuffer(ModbusCommands.CIA461WeightStatusScaleSealIsOpen);
            this.ReadFromBuffer(ModbusCommands.CIA461WeightStatusManualTare);
            this.ReadFromBuffer(ModbusCommands.TMDTareMode);
            this.ReadFromBuffer(ModbusCommands.CIA461WeightStatusScaleRange);
            this.ReadFromBuffer(ModbusCommands.CIA461WeightStatusZeroRequired);
            this.ReadFromBuffer(ModbusCommands.CIA461WeightStatusCenterOfZero);
            this.ReadFromBuffer(ModbusCommands.CIA461WeightStatusInsideZero);

            this.ReadFromBuffer(ModbusCommands.IMDApplicationMode);     // application mode 
            this.ReadFromBuffer(ModbusCommands.CIA461Decimals);             // decimals
            this.ReadFromBuffer(ModbusCommands.CIA461Unit);                 // unit
            this.ReadFromBuffer(ModbusCommands.PLCComHandshake);            // handshake

            this.ReadFromBuffer(ModbusCommands.IS1DigitalInput1);
            this.ReadFromBuffer(ModbusCommands.OS1DigitalOutput1);
            this.ReadFromBuffer(ModbusCommands.LVSLimitValueStatus);

            this.ReadFromBuffer(ModbusCommands.FFDFineFlowDisconnect);
            this.ReadFromBuffer(ModbusCommands.CFDCoarseFlowDisconnect);
            this.ReadFromBuffer(ModbusCommands.CBKCoarseFlowMonitoring);
            this.ReadFromBuffer(ModbusCommands.FBKFineFlowMonitoring);

            this.ReadFromBuffer(ModbusCommands.FillingStateReady);
            this.ReadFromBuffer(ModbusCommands.FillingStateReDosing);

            //this.GetDataFromDictionary(ModbusCommands.Emptying_mode);
            this.ReadFromBuffer(ModbusCommands.MDTMaximalFillingTime);
            this.ReadFromBuffer(ModbusCommands.UTLUpperToleranceLimit);
            this.ReadFromBuffer(ModbusCommands.LTLLowerToleranceLimit);
            this.ReadFromBuffer(ModbusCommands.FillingStateStatusInput1);
            this.ReadFromBuffer(ModbusCommands.FillingStateLegalForTradeOperation);

            this.ReadFromBuffer(ModbusCommands.WeightMemDayStandard);
            this.ReadFromBuffer(ModbusCommands.WeightMemMonthStandard);
            this.ReadFromBuffer(ModbusCommands.WeightMemYearStandard);
            this.ReadFromBuffer(ModbusCommands.WeightMemSeqNumberStandard);
            this.ReadFromBuffer(ModbusCommands.WeightMemGrossStandard);
            this.ReadFromBuffer(ModbusCommands.WeightMemNetStandard);

            this.ReadFromBuffer(ModbusCommands.FillingStateEmptying);
            this.ReadFromBuffer(ModbusCommands.FillingStateFlowError);
            this.ReadFromBuffer(ModbusCommands.FillingStateAlarm);
            this.ReadFromBuffer(ModbusCommands.FillingStateAdcOverUnderload);

            this.ReadFromBuffer(ModbusCommands.FillingStateStatusInput1);
            this.ReadFromBuffer(ModbusCommands.FillingStateGeneralScaleError);
            this.ReadFromBuffer(ModbusCommands.SUMFillingResultSum);

            // Undefined IDs:
            /*
            _dataBuffer[IDCommands.DOSING_RESULT]      = _data[12];
            _dataBuffer[IDCommands.MEAN_VALUE_DOSING_RESULTS] = _data[14];
            _dataBuffer[IDCommands.STANDARD_DEVIATION] = _data[16];
            _dataBuffer[IDCommands.CURRENT_DOSING_TIME]        = _data[24];    // _currentDosingTime = _data[24];

            _dataBuffer[IDCommands.CURRENT_COARSE_FLOW_TIME] = _data[25];      // _currentCoarseFlowTime
            _dataBuffer[IDCommands.CURRENT_FINE_FLOW_TIME]   = _data[26];      // _currentFineFlowTime
            _dataBuffer[IDCommands.RANGE_SELECTION_PARAMETER] = _data[27];     // _parameterSetProduct

            _dataBuffer[IDCommands.] = _fillingProcessStatus = _data[9];  // Undefined
            _dataBuffer[IDCommands.] = _numberDosingResults = _data[11];        
            */
        }


        public string Read(object index)
        {
            switch (this.behavior)
            {
                case Behavior.WriteHandshakeTestSuccess:

                    if (_dataWTX[4] == 0x0000)
                        _dataWTX[4] = 0x4000;
                    else
                        if (_dataWTX[4] == 0x4000)
                        _dataWTX[4] = 0x0000;
                    break;

                case Behavior.InFillerMode:

                    //data word for a application mode being in filler mode: Bit .0-1 = 1 || 2 (2 is the given value for filler mode according to the manual, but actually it is 1.)
                    _dataWTX[5] = 0x1;
                    break;

                case Behavior.InStandardMode:

                    //data word for a application mode being in standard mode, not in filler mode: Bit .0-1 = 0
                    _dataWTX[5] = 0x00;

                    break;

                case Behavior.CalibrationFail:

                    //Handshake bit:

                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                case Behavior.CalibrationSuccess:

                    //Handshake bit:

                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                case Behavior.MeasureZeroFail:

                    // Net value in hexadecimal: 
                    _dataWTX[0] = 0x00;
                    _dataWTX[1] = 0x2710;

                    // Gross value in hexadecimal:
                    _dataWTX[2] = 0x00;
                    _dataWTX[3] = 0x2710;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;

                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                case Behavior.MeasureZeroSuccess:

                    // Net value in hexadecimal: 
                    _dataWTX[0] = 0x00;
                    _dataWTX[1] = 0x00;

                    // Gross value in hexadecimal:
                    _dataWTX[2] = 0x00;
                    _dataWTX[3] = 0x00;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;
                    
                case Behavior.WriteSyncSuccess:

                    this.command = 0x100;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                case Behavior.WriteSyncFail:

                    this.command = 0;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;
                    
                case Behavior.WriteFail:

                    this.command = 0;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                default:
                    /*
                    for (int index = 0; index < _dataWTX.Length; index++)
                    {
                        _dataWTX[index] = 0;
                    }
                    _logObj = new LogEvent("Read failed : Registers have not been read");
                    Trace?.Invoke(this, _logObj);
                    */
                    break;
            }

            _dataBuffer["0"] = "1";

            this.UpdateDictionary();
            // Updata data in data classes : 
            this.UpdateData?.Invoke(this, new EventArgs());

            return _dataWTX[Convert.ToInt16(index)].ToString();
        }

        public int GetCommand
        {
            get { return this.command; }
        }


        // This method writes a data word to the WTX120 device synchronously. 
        private void DoHandshake(ushort register)
        {
        
        }

        public bool WriteInteger(object command, int data)
        {
            ModbusCommand _command = (ModbusCommand)command;

            this.wordNumberIndex = Convert.ToInt16(_command.Register);

            switch (this.behavior)
            {
                case Behavior.MeasureZeroFail:

                    // Net value in hexadecimal: 
                    _dataWTX[0] = 0x00;
                    _dataWTX[1] = 0x2710;

                    // Gross value in hexadecimal:
                    _dataWTX[2] = 0x00;
                    _dataWTX[3] = 0x2710;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;

                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                case Behavior.MeasureZeroSuccess:

                    // Net value in hexadecimal: 
                    _dataWTX[0] = 0x00;
                    _dataWTX[1] = 0x00;

                    // Gross value in hexadecimal:
                    _dataWTX[2] = 0x00;
                    _dataWTX[3] = 0x00;

                    this.arrayElement1 = (ushort)((0x7FFFFFFF & 0xffff0000) >> 16);
                    this.arrayElement2 = (ushort)(0x7FFFFFFF & 0x0000ffff);
        
                    //Handshake bit:
                        if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                case Behavior.UpdateOutputTestSuccess:
                    this.command = 0x800;
                    break;

                case Behavior.UpdateOutputTestFail:
                    this.command = 0x00;
                    break;

                case Behavior.WriteU08ArrayTestSuccess:
                    this.wordNumberIndex = (ushort)Convert.ToUInt16(_command.Register);
                    this.arrayElement1 = (ushort)data;
                    break;

                case Behavior.WriteU08ArrayTestFail:
                    this.wordNumberIndex = 0;
                    this.arrayElement1 = 0;
                    break;
                case Behavior.WriteU16ArrayTestSuccess:
                    this.wordNumberIndex = (ushort)Convert.ToUInt16(_command.Register);
                    
                    this.arrayElement1 = (ushort)((data & 0xffff0000) >> 16);
                    this.arrayElement2 = (ushort)(data & 0x0000ffff);

                    break;
                case Behavior.WriteU16ArrayTestFail:
                    this.wordNumberIndex = 0;
                    this.arrayElement1 = 0;
                    break;
                case Behavior.WriteS32ArrayTestSuccess:
                    this.wordNumberIndex = (ushort)Convert.ToUInt16(_command.Register);

                    this.arrayElement1 = (ushort)((data & 0xffff0000) >> 16);
                    this.arrayElement2 = (ushort)(data & 0x0000ffff);
                    break;

                case Behavior.WriteS32ArrayTestFail:
                    this.wordNumberIndex = 0;
                    this.arrayElement1 = 0;
                    this.arrayElement2 = 0;
                    break;
                case Behavior.GrosMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.GrosMethodTestFail:
                    this.command = 0;
                    break;
                case Behavior.TareMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.TareMethodTestFail:
                    this.command = 0;
                    break;

                case Behavior.AdjustingZeroMethodSuccess:
                    this.command = data;
                    break;
                case Behavior.AdjustingZeroMethodFail:
                    this.command = 0;
                    break;
                case Behavior.AdjustNominalMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.AdjustNominalMethodTestFail:
                    this.command = 0;
                    break;
                case Behavior.ActivateDataMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.ActivateDataMethodTestFail:
                    this.command = 0;
                    break;
                case Behavior.ManualTaringMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.ManualTaringMethodTestFail:
                    this.command = 0;
                    break;
                case Behavior.ClearDosingResultsMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.ClearDosingResultsMethodTestFail:
                    this.command = 0;
                    break;

                case Behavior.StartDosingMethodTestSuccess:
                    command = data;
                    break;
                case Behavior.StartDosingMethodTestFail:
                    this.command = 0;
                    break;
                case Behavior.RecordWeightMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.RecordWeightMethodTestFail:
                    this.command = 0;
                    break;
                case Behavior.ManualRedosingMethodTestSuccess:
                    this.command = data;
                    break;
                case Behavior.ManualRedosingMethodTestFail:
                    this.command = 0;
                    break;
                case Behavior.WriteHandshakeTestSuccess:

                    if (_dataWTX[4] == 0x0000)
                    {
                        this.command = data;
                        _dataWTX[4] = 0x4000;
                    }
                    else
                    if (_dataWTX[4] == 0x4000)
                    {
                        this.command = 0x0;
                        _dataWTX[4] = 0x0000;
                    }

                    break;

                case Behavior.WriteHandshakeTestFail:
                    _dataWTX[4] = 0x0000;
                    break;

                case Behavior.InFillerMode:
                    //data word for a application mode being in filler mode: Bit .0-1 = 1 || 2 (2 is the given value for filler mode according to the manual, but actually it is 1.)
                    _dataWTX[5] = 0x1;
                    break;

                case Behavior.InStandardMode:
                    //data word for a application mode being in standard mode, not in filler mode: Bit .0-1 = 0
                    _dataWTX[5] = 0x00;
                    break;

                case Behavior.CalibrationFail:
                    this.arrayElement1 = 0;
                    this.arrayElement2 = 0;
                    break;

                case Behavior.CalibrationSuccess:

                    if (this.wordNumberIndex == 48 || this.wordNumberIndex == 46)       // According to the index 48 (=wordnumber) the preload is written. 
                    {
                        this.arrayElement1 = (ushort)((Convert.ToInt32(data) & 0xffff0000) >> 16);
                        this.arrayElement2 = (ushort)(Convert.ToInt32(data) & 0x0000ffff);
                    }
                    else
                    if (this.wordNumberIndex == 50)       // According to the index 50 (=wordnumber) the nominal load is written. 
                    {
                        this.arrayElement3 = _data[0];
                        this.arrayElement4 = _data[1];

                        this.arrayElement3 = (ushort)((Convert.ToInt32(data) & 0xffff0000) >> 16);
                        this.arrayElement4 = (ushort)((Convert.ToInt32(data) & 0x0000ffff));
                    }
                    break;

                case Behavior.WriteSyncSuccess:

                    this.command = data;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;

                case Behavior.WriteSyncFail:

                    this.command = 0x100;

                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;

                    break;
                    
                case Behavior.WriteFail:
                    this.command = 0x2;
                    break;

                case Behavior.WriteSuccess:
                    command = 0;
                    break;

                case Behavior.HandshakeSuccess:
                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;
                    break;

                case Behavior.HandshakeFail:
                    //Handshake bit:
                    if (_dataWTX[5] >> 14 == 0)
                        _dataWTX[5] = 0x4000;
                    else if (_dataWTX[5] >> 14 == 1)
                        _dataWTX[5] = 0x0000;
                    break;

                case Behavior.WriteLimitSwitch1ModeTestSuccess:
                    this.command = 4; 
                    break;
                case Behavior.WriteLimitSwitch1ModeTestFail:
                    this.command = 0;
                    break;
                case Behavior.WriteLimitSwitch2ModeTestSuccess:
                    this.command = 11;
                    break;
                case Behavior.WriteLimitSwitch2ModeTestFail:
                    this.command = 0;
                    break;
                case Behavior.WriteLimitSwitch3ModeTestSuccess:
                    this.command = 17;
                    break;
                case Behavior.WriteLimitSwitch3ModeTestFail:
                    this.command = 0;
                    break;
                case Behavior.WriteLimitSwitch4ModeTestSuccess:
                    this.command = 23;
                    break;
                case Behavior.WriteLimitSwitch4ModeTestFail:
                    this.command = 0;
                    break;
            }
            return true;
        }

        public bool Write(object command, string data)
        {
            return this.WriteInteger(command, Convert.ToInt32(data));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<ushort[]> SyncData()
        {
            ushort[] value = new ushort[1];
            await Task.Run( () =>
            {
            switch (behavior)
            {
                case Behavior.ReadFail:

                    // If there is a connection fail, all data attributes get 0 as value.

                    for (int i = 0; i < _dataWTX.Length; i++)
                    {
                        _dataWTX[i] = 0x0000;
                    }

                    _logObj = new LogEventArgs("Read failed : Registers have not been read");

                    CommunicationLog?.Invoke(this, _logObj);

                    break;

                case Behavior.ReadSuccess:


                    // The most important data attributes from the WTX120 device: 
                    
                    _dataWTX[0] = 0x0000;
                    _dataWTX[1] = 0x4040;
                    _dataWTX[2] = 0x0000;
                    _dataWTX[3] = 0x4040;
                    _dataWTX[4] = 0x0000;
                    _dataWTX[5] = 0x0000;

                    _logObj = new LogEventArgs("Read successful: Registers have been read");
                    CommunicationLog?.Invoke(this, _logObj);
                    break;


                    case Behavior.NetGrossValueStringComment_0D_Success:
                        _dataWTX[5] = 0x0000;
                        break;

                    case Behavior.NetGrossValueStringComment_0D_Fail:
                        _dataWTX[5] = 0x60;
                        break;

                    case Behavior.NetGrossValueStringComment_1D_Success:
                        _dataWTX[5] = 0x10;
                        break;

                    case Behavior.NetGrossValueStringComment_2D_Success:
                        _dataWTX[5] = 0x20;
                        break;

                    case Behavior.NetGrossValueStringComment_3D_Success:
                        _dataWTX[5] = 0x30;
                        break;

                    case Behavior.NetGrossValueStringComment_3D_Fail:
                        _dataWTX[5] = 0x00;
                        break;

                    case Behavior.NetGrossValueStringComment_4D_Success:
                        _dataWTX[5] = 0x40;
                        break;

                    case Behavior.NetGrossValueStringComment_5D_Success:
                        _dataWTX[5] = 0x50;
                        break;

                    case Behavior.NetGrossValueStringComment_6D_Success:
                        _dataWTX[5] = 0x60;
                        break;


                    // Simulate for testing 'Unit': 

                    case Behavior.t_UnitValue_Success:
                    _dataWTX[5] = 0x100;
                    break;
                case Behavior.t_UnitValue_Fail:
                    _dataWTX[5] = 0x0000;
                    break;

                case Behavior.kg_UnitValue_Success:
                    _dataWTX[5] = 0x0000;
                    break;

                case Behavior.kg_UnitValue_Fail:
                    _dataWTX[5] = 0xFFFF;
                    break;

                case Behavior.g_UnitValue_Success:
                    _dataWTX[5] = 0x80;
                    break;

                case Behavior.g_UnitValue_Fail:
                    _dataWTX[5] = 0x0000;
                    break;

                case Behavior.lb_UnitValue_Success:
                    _dataWTX[5] = 0x180;
                    break;


                case Behavior.lb_UnitValue_Fail:
                    _dataWTX[5] = 0x0000;
                    break;


                // Simulate for testing 'Limit status': 

                case Behavior.LimitStatusStringComment_Case0_Fail:
                    _dataWTX[4] = 0xC;
                    break;
                case Behavior.LimitStatusStringComment_Case1_Fail:
                    _dataWTX[4] = 0x8;
                    break;
                case Behavior.LimitStatusStringComment_Case2_Fail:
                    _dataWTX[4] = 0x0000;
                    break;
                case Behavior.LimitStatusStringComment_Case3_Fail:
                    _dataWTX[4] = 0x4;
                    break;

                case Behavior.LimitStatusStringComment_Case0_Success:
                    _dataWTX[4] = 0x0000;
                    break;
                case Behavior.LimitStatusStringComment_Case1_Success:
                    _dataWTX[4] = 0x4;
                    break;
                case Behavior.LimitStatusStringComment_Case2_Success:
                    _dataWTX[4] = 0x8;
                    break;
                case Behavior.LimitStatusStringComment_Case3_Success:
                    _dataWTX[4] = 0xC;
                    break;

                // Simulate for testing 'Weight moving': 
                case Behavior.WeightMovingStringComment_Case0_Fail:
                    _dataWTX[4] = 0x0010;
                    break;
                case Behavior.WeightMovingStringComment_Case1_Fail:
                    _dataWTX[4] = 0x0000;
                    break;
                case Behavior.WeightMovingStringComment_Case0_Success:
                    _dataWTX[4] = 0x0000;
                    break;
                case Behavior.WeightMovingStringComment_Case1_Success:
                    _dataWTX[4] = 0x0010;
                    break;

                // Simulate for testing 'Weight type': 
                case Behavior.WeightTypeStringComment_Case0_Fail:
                    _dataWTX[4] = 0x0080;
                    break;
                case Behavior.WeightTypeStringComment_Case1_Fail:
                    _dataWTX[4] = 0x0000;
                    break;

                case Behavior.WeightTypeStringComment_Case0_Success:
                    _dataWTX[4] = 0x0000;
                    break;
                case Behavior.WeightTypeStringComment_Case1_Success:
                    _dataWTX[4] = 0x0080;
                    break;
                // Simulate for testing 'Scale range': 

                case Behavior.ScaleRangeStringComment_Range1_Fail:
                    _dataWTX[4] = 0x200;
                    break;

                case Behavior.ScaleRangeStringComment_Range2_Fail:
                    _dataWTX[4] = 0x0000;
                    break;

                case Behavior.ScaleRangeStringComment_Range3_Fail:
                    _dataWTX[4] = 0x100;
                    break;

                case Behavior.ScaleRangeStringComment_Range1_Success:
                    _dataWTX[4] = 0x0000;
                    break;

                case Behavior.ScaleRangeStringComment_Range2_Success:
                    _dataWTX[4] = 0x100;
                    break;

                case Behavior.ScaleRangeStringComment_Range3_Success:
                    _dataWTX[4] = 0x200;
                    break;

                case Behavior.LogEvent_Fail:

                    _logObj = new LogEventArgs("Read failed : Registers have not been read");
                    CommunicationLog?.Invoke(this, _logObj);
                    break;

                case Behavior.LogEvent_Success:

                    _logObj = new LogEventArgs("Read successful: Registers have been read");
                    CommunicationLog?.Invoke(this, _logObj);
                    break;
            }
            if (_dataWTX[5] == 0x0000)
            {
                _dataWTX[5] = 0x4000;
            }
            else
            if (_dataWTX[5] == 0x4000)
            {
                _dataWTX[5] = 0x0000;
            }
            
                this.UpdateDictionary();
                // Update data in data classes : 
                this.UpdateData?.Invoke(this, new EventArgs());

                return _dataWTX;

             });

            this.UpdateDictionary();
            // Update data in data classes : 
            this.UpdateData?.Invoke(this, new EventArgs());

            return _dataWTX;
        }

        public async Task<string> ReadAsync(object index)
        {
            ushort[] value = new ushort[1];
            await Task.Run( () =>
            {
                switch (behavior)
                {
                    case Behavior.ReadFail:

                        // If there is a connection fail, all data attributes get 0 as value.

                        for (int i = 0; i < _dataWTX.Length; i++)
                        {
                            _dataWTX[i] = 0x0000;
                        }

                        _logObj = new LogEventArgs("Read failed : Registers have not been read");

                        CommunicationLog?.Invoke(this, _logObj);

                        break;

                    case Behavior.ReadSuccess:


                        // The most important data attributes from the WTX120 device: 

                        _dataWTX[0] = 0x0000;
                        _dataWTX[1] = 0x4040;
                        _dataWTX[2] = 0x0000;
                        _dataWTX[3] = 0x4040;
                        _dataWTX[4] = 0x0000;
                        _dataWTX[5] = 0x0000;

                        _logObj = new LogEventArgs("Read successful: Registers have been read");
                        CommunicationLog?.Invoke(this, _logObj);
                        break;


                    case Behavior.NetGrossValueStringComment_0D_Success:
                        _dataWTX[5] = 0x0000;
                        break;

                    case Behavior.NetGrossValueStringComment_0D_Fail:
                        _dataWTX[5] = 0x60;
                        break;

                    case Behavior.NetGrossValueStringComment_1D_Success:
                        _dataWTX[5] = 0x10;
                        break;

                    case Behavior.NetGrossValueStringComment_2D_Success:
                        _dataWTX[5] = 0x20;
                        break;

                    case Behavior.NetGrossValueStringComment_3D_Success:
                        _dataWTX[5] = 0x30;
                        break;

                    case Behavior.NetGrossValueStringComment_3D_Fail:
                        _dataWTX[5] = 0x00;
                        break;

                    case Behavior.NetGrossValueStringComment_4D_Success:
                        _dataWTX[5] = 0x40;
                        break;

                    case Behavior.NetGrossValueStringComment_5D_Success:
                        _dataWTX[5] = 0x50;
                        break;

                    case Behavior.NetGrossValueStringComment_6D_Success:
                        _dataWTX[5] = 0x60;
                        break;


                    // Simulate for testing 'Unit': 

                    case Behavior.t_UnitValue_Success:
                        _dataWTX[5] = 0x100;
                        break;
                    case Behavior.t_UnitValue_Fail:
                        _dataWTX[5] = 0x0000;
                        break;

                    case Behavior.kg_UnitValue_Success:
                        _dataWTX[5] = 0x0000;
                        break;

                    case Behavior.kg_UnitValue_Fail:
                        _dataWTX[5] = 0xFFFF;
                        break;

                    case Behavior.g_UnitValue_Success:
                        _dataWTX[5] = 0x80;
                        break;

                    case Behavior.g_UnitValue_Fail:
                        _dataWTX[5] = 0x0000;
                        break;

                    case Behavior.lb_UnitValue_Success:
                        _dataWTX[5] = 0x180;
                        break;


                    case Behavior.lb_UnitValue_Fail:
                        _dataWTX[5] = 0x0000;
                        break;


                    // Simulate for testing 'Limit status': 

                    case Behavior.LimitStatusStringComment_Case0_Fail:
                        _dataWTX[4] = 0xC;
                        break;
                    case Behavior.LimitStatusStringComment_Case1_Fail:
                        _dataWTX[4] = 0x8;
                        break;
                    case Behavior.LimitStatusStringComment_Case2_Fail:
                        _dataWTX[4] = 0x0000;
                        break;
                    case Behavior.LimitStatusStringComment_Case3_Fail:
                        _dataWTX[4] = 0x4;
                        break;

                    case Behavior.LimitStatusStringComment_Case0_Success:
                        _dataWTX[4] = 0x0000;
                        break;
                    case Behavior.LimitStatusStringComment_Case1_Success:
                        _dataWTX[4] = 0x4;
                        break;
                    case Behavior.LimitStatusStringComment_Case2_Success:
                        _dataWTX[4] = 0x8;
                        break;
                    case Behavior.LimitStatusStringComment_Case3_Success:
                        _dataWTX[4] = 0xC;
                        break;

                    // Simulate for testing 'Weight moving': 
                    case Behavior.WeightMovingStringComment_Case0_Fail:
                        _dataWTX[4] = 0x0010;
                        break;
                    case Behavior.WeightMovingStringComment_Case1_Fail:
                        _dataWTX[4] = 0x0000;
                        break;
                    case Behavior.WeightMovingStringComment_Case0_Success:
                        _dataWTX[4] = 0x0000;
                        break;
                    case Behavior.WeightMovingStringComment_Case1_Success:
                        _dataWTX[4] = 0x0010;
                        break;

                    // Simulate for testing 'Weight type': 
                    case Behavior.WeightTypeStringComment_Case0_Fail:
                        _dataWTX[4] = 0x0080;
                        break;
                    case Behavior.WeightTypeStringComment_Case1_Fail:
                        _dataWTX[4] = 0x0000;
                        break;

                    case Behavior.WeightTypeStringComment_Case0_Success:
                        _dataWTX[4] = 0x0000;
                        break;
                    case Behavior.WeightTypeStringComment_Case1_Success:
                        _dataWTX[4] = 0x0080;
                        break;
                    // Simulate for testing 'Scale range': 

                    case Behavior.ScaleRangeStringComment_Range1_Fail:
                        _dataWTX[4] = 0x200;
                        break;

                    case Behavior.ScaleRangeStringComment_Range2_Fail:
                        _dataWTX[4] = 0x0000;
                        break;

                    case Behavior.ScaleRangeStringComment_Range3_Fail:
                        _dataWTX[4] = 0x100;
                        break;

                    case Behavior.ScaleRangeStringComment_Range1_Success:
                        _dataWTX[4] = 0x0000;
                        break;

                    case Behavior.ScaleRangeStringComment_Range2_Success:
                        _dataWTX[4] = 0x100;
                        break;

                    case Behavior.ScaleRangeStringComment_Range3_Success:
                        _dataWTX[4] = 0x200;
                        break;

                    case Behavior.LogEvent_Fail:

                        _logObj = new LogEventArgs("Read failed : Registers have not been read");
                        CommunicationLog?.Invoke(this, _logObj);
                        break;

                    case Behavior.LogEvent_Success:

                        _logObj = new LogEventArgs("Read successful: Registers have been read");
                        CommunicationLog?.Invoke(this, _logObj);
                        break;
                }
                if (_dataWTX[5] == 0x0000)
                {
                    _dataWTX[5] = 0x4000;
                }
                else
                if (_dataWTX[5] == 0x4000)
                {
                    _dataWTX[5] = 0x0000;
                }

                this.UpdateDictionary();
                // Update data in data classes : 
                this.UpdateData?.Invoke(this, new EventArgs());

                return _dataWTX;

            });

            this.UpdateDictionary();
            // Update data in data classes : 
            this.UpdateData?.Invoke(this, new EventArgs());

            return _dataWTX.ToString();
        }

        public async Task<int> WriteAsync(object command, int value)
        {
            this.command = value;

            await Task.Run(() =>
            {
                switch (behavior)
                {
                    case Behavior.ZeroMethodTestSuccess:
                        this.command = value;
                        break;
                    case Behavior.ZeroMethodTestFail:
                        this.command = 0x00;
                        break;

                    case Behavior.AbortDosingMethodTestSuccess:
                        this.command = value;
                        break;
                    case Behavior.AbortDosingMethodTestFail:
                        this.command = 0;
                        break;
                }
                // Change the handshake bit : bit .14 from 0 to 1.
                if (_dataWTX[5] == 0x0000)
                    _dataWTX[5] = 0x4000;
                else
                    if (_dataWTX[5] == 0x4000)
                    _dataWTX[5] = 0x0000;

                this.command = value;
            });
            return this.command;
        }
        
        public string ReadFromBuffer(object frame)
        {
            int _register = 0;
            ushort _bitMask = 0;
            ushort _mask = 0;

            ModbusCommand ConvertedFrame = frame as ModbusCommand;

            if (ConvertedFrame.DataType == DataType.S32)
                _dataBuffer[ConvertedFrame.Path] = (_dataWTX[Convert.ToInt16(ConvertedFrame.Register) + 1] + (_dataWTX[Convert.ToInt16(ConvertedFrame.Register)] << 16)).ToString();

            if (ConvertedFrame.DataType != DataType.S32 && ConvertedFrame.DataType != DataType.S32 && ConvertedFrame.DataType != DataType.U32)
            {
                switch (ConvertedFrame.BitLength)
                {
                    case 0: _bitMask = 0xFFFF; break;
                    case 1: _bitMask = 1; break;
                    case 2: _bitMask = 3; break;
                    case 3: _bitMask = 7; break;

                    default: _bitMask = 1; break;
                }

                _mask = (ushort)(_bitMask << ConvertedFrame.BitIndex);

                _register = Convert.ToInt32(ConvertedFrame.Register);
                _dataBuffer[ConvertedFrame.Path] = ((_dataWTX[_register] & _mask) >> ConvertedFrame.BitIndex).ToString();
            }

            return _dataBuffer[ConvertedFrame.Path];
        }

        public string ReadFromDevice(object command)
        {
            return null; //tbd
        }

        public int ReadIntegerFromBuffer(object command)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> AllData
        {
            get
            {
                return this._dataBuffer;
            }
        }

        public int getWordNumber
        {
            get
            {
                return this.wordNumberIndex;
            }
        }

        public ushort getArrElement1
        {
            get
            {
                return this.arrayElement1;
            }
        }

        public ushort getArrElement2
        {
            get
            {
                return this.arrayElement2;
            }
        }

        public ushort getArrElement3
        {
            get
            {
                return this.arrayElement3;
            }
        }

        public ushort getArrElement4
        {
            get
            {
                return this.arrayElement4;
            }
        }

        public int NumofPoints
        {
            get
            {
                return this.numPoints;
            }
            set
            {
                this.numPoints = value; 
            }
        }
        
        public string IpAddress
        {
            get
            {
                return this.IP;
            }
            set
            {
                this.IP = value; 
            }
        }

        public int SendingInterval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }

        public ushort[] getData {

            get
            {
                return this._dataWTX;
            }
            set
            {
                this._dataWTX = value; 
            }

        }
        public ConnectionType ConnectionType
        {
            get { return ConnectionType.Modbus; }
        }
    }
}
