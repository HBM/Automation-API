// <copyright file="ModbusCommands.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.WTX.Modbus
{
    /// <summary>
    /// Class with all Modbus commands and additional command information 
    /// </summary>
    public static class ModbusCommands
    {
        #region =============== constructors & destructors =================
        static ModbusCommands()
        {
            WeightMemDayStandard   = new ModbusCommand(DataType.S16, 9, IOType.Input, ApplicationMode.Standard, 0, 0);
            WeightMemMonthStandard = new ModbusCommand(DataType.S16, 10, IOType.Input, ApplicationMode.Standard, 0, 0);
            WeightMemYearStandard = new ModbusCommand(DataType.S16, 11, IOType.Input, ApplicationMode.Standard, 0, 0);
            WeightMemSeqNumberStandard = new ModbusCommand(DataType.S16, 12, IOType.Input, ApplicationMode.Standard, 0, 0);
            WeightMemGrossStandard = new ModbusCommand(DataType.S16, 13, IOType.Input, ApplicationMode.Standard, 0, 0);
            WeightMemNetStandard = new ModbusCommand(DataType.S16, 14, IOType.Input, ApplicationMode.Standard, 0, 0);
            WeightMemDayFiller   = new ModbusCommand(DataType.S16, 32, IOType.Output, ApplicationMode.Filler, 0, 0);
            WeightMemMonthFiller = new ModbusCommand(DataType.S16, 33, IOType.Output, ApplicationMode.Filler, 0, 0);
            WeightMemYearFiller = new ModbusCommand(DataType.S16, 34, IOType.Output, ApplicationMode.Filler, 0, 0);
            WeightMemSeqNumberFiller = new ModbusCommand(DataType.S16, 35, IOType.Output, ApplicationMode.Filler, 0, 0);
            WeightMemGrossFiller = new ModbusCommand(DataType.S16, 36, IOType.Output, ApplicationMode.Filler, 0, 0);
            WeightMemNetFiller = new ModbusCommand(DataType.S16, 37, IOType.Output, ApplicationMode.Filler, 0, 0);
            CWTScaleCalibrationWeight = new ModbusCommand(DataType.S32, 46, IOType.Output, ApplicationMode.Standard, 0, 0);
            LDWZeroSignal   = new ModbusCommand(DataType.S32, 48, IOType.Output, ApplicationMode.Standard, 0, 0);
            LWTNominalSignal = new ModbusCommand(DataType.S32, 50, IOType.Output, ApplicationMode.Standard, 0, 0);            
            CIA461NetValue   = new ModbusCommand(DataType.S32, 0, IOType.Input, ApplicationMode.Standard, 32, 0);
            CIA461GrossValue = new ModbusCommand(DataType.S32, 2, IOType.Input, ApplicationMode.Standard, 32, 0);            
            CIA461WeightStatusGeneralWeightError = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 0, 1);
            CIA461WeightStatusScaleAlarm = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 1, 1);
            CIA461WeightStatusLimitStatus = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 2, 2);            
            CIA461WeightStatusWeightMoving = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 4, 1);
            CIA461WeightStatusScaleSealIsOpen = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 5, 1);
            CIA461WeightStatusManualTare = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 6, 1);
            CIA461WeightStatusWeightType = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 7, 1);
            CIA461WeightStatusScaleRange = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 8, 2);
            CIA461WeightStatusZeroRequired = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 10, 1);
            CIA461WeightStatusCenterOfZero = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 11, 1);
            CIA461WeightStatusInsideZero = new ModbusCommand(DataType.BIT, 4, IOType.Input, ApplicationMode.Standard, 12, 1);
            IMDApplicationMode = new ModbusCommand(DataType.BIT, 5, IOType.Input, ApplicationMode.Standard, 0, 2);  
            CIA461Decimals = new ModbusCommand(DataType.BIT, 5, IOType.Input, ApplicationMode.Standard, 4, 3);         
            CIA461Unit = new ModbusCommand(DataType.BIT, 5, IOType.Input, ApplicationMode.Standard, 7, 2);              
            PLCComHandshake = new ModbusCommand(DataType.BIT, 5, IOType.Input, ApplicationMode.Standard, 14, 1);            
            PLCComStatus = new ModbusCommand(DataType.BIT, 5, IOType.Input, ApplicationMode.Standard, 15, 1);       
            IS1DigitalInput1 = new ModbusCommand(DataType.BIT, 6, IOType.Input, ApplicationMode.Standard, 1, 1); 
            IS2DigitalInput2 = new ModbusCommand(DataType.BIT, 6, IOType.Input, ApplicationMode.Standard, 2, 1);  
            IS3DigitalInput3 = new ModbusCommand(DataType.BIT, 6, IOType.Input, ApplicationMode.Standard, 3, 1);
            IS4DigitalInput4 = new ModbusCommand(DataType.BIT, 6, IOType.Input, ApplicationMode.Standard, 4, 1);
            OS1DigitalOutput1 = new ModbusCommand(DataType.BIT, 7, IOType.Input, ApplicationMode.Standard, 1, 1); 
            OS2DigitalOutput2 = new ModbusCommand(DataType.BIT, 7, IOType.Input, ApplicationMode.Standard, 2, 1);
            OS3DigitalOutput3 = new ModbusCommand(DataType.BIT, 7, IOType.Input, ApplicationMode.Standard, 3, 1);
            OS4DigitalOutput4 = new ModbusCommand(DataType.BIT, 7, IOType.Input, ApplicationMode.Standard, 4, 1);
            LVSLimitValueStatus = new ModbusCommand(DataType.U08, 8, IOType.Input, ApplicationMode.Standard, 0, 0);
            CIA461TareValue = new ModbusCommand(DataType.U08, 2, IOType.Input, ApplicationMode.Standard, 0, 0); 
            LIV1LimitSwitchSource = new ModbusCommand(DataType.U08, 4, IOType.Output, ApplicationMode.Standard, 0, 0); 
            LIV1LimitSwitchMode = new ModbusCommand(DataType.U08, 5, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV1LimitSwitchLevel = new ModbusCommand(DataType.S32, 6, IOType.Output, ApplicationMode.Standard, 0, 0);       
            LIV1LimitSwitchHysteresis = new ModbusCommand(DataType.S32, 8, IOType.Output, ApplicationMode.Standard, 0, 0);     
            LIV2LimitSwitchSource = new ModbusCommand(DataType.U08, 10, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV2LimitSwitchMode = new ModbusCommand(DataType.U08, 11, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV2LimitSwitchLevel = new ModbusCommand(DataType.S32, 12, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV2LimitSwitchHysteresis = new ModbusCommand(DataType.S32, 14, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV3LimitSwitchSource = new ModbusCommand(DataType.U08, 16, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV3LimitSwitchMode = new ModbusCommand(DataType.U08, 17, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV3LimitSwitchLevel = new ModbusCommand(DataType.S32, 18, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV3LimitSwitchHysteresis = new ModbusCommand(DataType.S32, 20, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV4LimitSwitchSource = new ModbusCommand(DataType.U08, 22, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV4LimitSwitchMode = new ModbusCommand(DataType.U08, 23, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV4LimitSwitchLevel = new ModbusCommand(DataType.S32, 24, IOType.Output, ApplicationMode.Standard, 0, 0);
            LIV4LimitSwitchHysteresis = new ModbusCommand(DataType.S32, 26, IOType.Output, ApplicationMode.Standard, 0, 0);
            FillingStateCoarseFlow = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 0, 1);          
            FillingStateFineFlow = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 1, 1);           
            FillingStateReady = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 2, 1);                
            FillingStateReDosing = new ModbusCommand(DataType.BIT, 8,  IOType.Input, ApplicationMode.Filler, 3, 1);                
            FillingStateEmptying = new ModbusCommand(DataType.BIT, 8,  IOType.Input, ApplicationMode.Filler, 4, 1);  
            FillingStateFlowError = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 5, 1);  
            FillingStateAlarm = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 6, 1);
            FillingStateAdcOverUnderload = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 7, 1);  
            FillingStateMaximalDosingTimeInput = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 8, 1);
            FillingStateLegalForTradeOperation = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 9, 1);  
            FillingStateToleranceErrorPlus = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 10, 1); 
            FillingStateToleranceErrorMinus = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 11, 1);  
            FillingStateStatusInput1 = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 14, 1);  
            FillingStateGeneralScaleError = new ModbusCommand(DataType.BIT, 8, IOType.Input, ApplicationMode.Filler, 15, 1);
            SUMFillingResultSum = new ModbusCommand(DataType.S32, 18, IOType.Input, ApplicationMode.Filler, 0, 0);
            DSTDosingTime = new ModbusCommand(DataType.U16, 24, IOType.Input, ApplicationMode.Filler, 0, 0); 
            CFTCoarseFlowTime = new ModbusCommand(DataType.U16, 25, IOType.Input, ApplicationMode.Filler, 0, 0); 
            FFTFineFlowTime = new ModbusCommand(DataType.U16, 26, IOType.Input, ApplicationMode.Filler, 0, 0); 
            CurrentProductParameterSet = new ModbusCommand(DataType.U08, 27, IOType.Input, ApplicationMode.Filler, 0, 0);
            FWTFillingTargetWeight = new ModbusCommand(DataType.S32, 10, IOType.Output, ApplicationMode.Filler, 0, 0);       
            RFTResidualFlowTime = new ModbusCommand(DataType.U16, 9, IOType.Output, ApplicationMode.Filler, 0, 0); 
            CFDCoarseFlowDisconnect = new ModbusCommand(DataType.S32, 12, IOType.Output, ApplicationMode.Filler, 0, 0); 
            FFDFineFlowDisconnect = new ModbusCommand(DataType.S32, 14, IOType.Output, ApplicationMode.Filler, 0, 0);
            FFMMinimumFineFlow = new ModbusCommand(DataType.S32, 16, IOType.Output, ApplicationMode.Filler, 0, 0); 
            OSNOptimization = new ModbusCommand(DataType.U08, 18, IOType.Output, ApplicationMode.Filler, 0, 0); 
            MDTMaximalFillingTime = new ModbusCommand(DataType.U16, 19, IOType.Output, ApplicationMode.Filler, 0, 0);  
            FFLFirstFineFlow = new ModbusCommand(DataType.U16, 20, IOType.Output, ApplicationMode.Filler, 0, 0); 
            LTCLockoutTimeCoarseFlow = new ModbusCommand(DataType.U16, 21, IOType.Output, ApplicationMode.Filler, 0, 0); 
            LTFLockoutTimeFineFlow = new ModbusCommand(DataType.U16, 22, IOType.Output, ApplicationMode.Filler, 0, 0); 
            TMDTareMode = new ModbusCommand(DataType.U08, 23, IOType.Output, ApplicationMode.Filler, 0, 0); 
            UTLUpperToleranceLimit = new ModbusCommand(DataType.S32, 24, IOType.Output, ApplicationMode.Filler, 0, 0);  
            LTLLowerToleranceLimit = new ModbusCommand(DataType.S32, 26, IOType.Output, ApplicationMode.Filler, 0, 0);   
            MSWMinimumStartWeight = new ModbusCommand(DataType.S32, 28, IOType.Output, ApplicationMode.Filler, 0, 0);
            EWTEmptyWeight = new ModbusCommand(DataType.S32, 30, IOType.Output, ApplicationMode.Filler, 0, 0);
            TADTareDelay = new ModbusCommand(DataType.U16, 32, IOType.Output, ApplicationMode.Filler, 0, 0);     
            CBTCoarseFlowMonitoringTime = new ModbusCommand(DataType.U16, 33, IOType.Output, ApplicationMode.Filler, 0, 0); 
            CBKCoarseFlowMonitoring = new ModbusCommand(DataType.U32, 34, IOType.Output, ApplicationMode.Filler, 0, 0);      
            FBKFineFlowMonitoring = new ModbusCommand(DataType.U32, 36, IOType.Output, ApplicationMode.Filler, 0, 0);       
            FBTFineFlowMonitoringTime = new ModbusCommand(DataType.U16, 38, IOType.Output, ApplicationMode.Filler, 0, 0);  
            DL1DosingDelay1 = new ModbusCommand(DataType.U08, 39, IOType.Output, ApplicationMode.Filler, 0, 0);
            DL2DosingDelay2 = new ModbusCommand(DataType.U08, 40, IOType.Output, ApplicationMode.Filler, 0, 0);
            SYDSystematicDifference = new ModbusCommand(DataType.U32, 41, IOType.Output, ApplicationMode.Filler, 0, 0); 
            DMDDosingMode = new ModbusCommand(DataType.U08, 42, IOType.Output, ApplicationMode.Filler, 0, 0);       
            VCTValveControl = new ModbusCommand(DataType.U08, 43, IOType.Output, ApplicationMode.Filler, 0, 0);       
            EMDEmptyingMode = new ModbusCommand(DataType.U08, 44, IOType.Output, ApplicationMode.Filler, 0, 0);      
            PLCComResetHandshake = new ModbusCommand(DataType.U16, 0, IOType.Output, ApplicationMode.Standard, 0, 0);
            ControlWordTare   = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 0, 1);
            ControlWordGrossNet = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 1, 1);
            ControlWordClearDosingResults = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 2, 1);
            ControlWordAbortDosing    = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 3, 1);
            ControlWordStartDosing    = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 4, 1);
            ControlWordZeroing        = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 6, 1);
            ControlWordAdjustZero     = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 7, 1);
            ControlWordAdjustNominal  = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 8, 1);
            ControlWordActivateData   = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 11, 1);
            ControlWordRecordWeight   = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 14, 1);
            ControlWordManualReDosing = new ModbusCommand(DataType.BIT, 0, IOType.Output, ApplicationMode.Standard, 15, 1);            
        }
        #endregion

        #region ======================== properties ========================          
        public static ModbusCommand CIA461Decimals { get; private set; }

        public static ModbusCommand CIA461GrossValue { get; private set; }

        public static ModbusCommand CIA461NetValue { get; private set; }

        public static ModbusCommand CIA461TareValue { get; private set; }

        public static ModbusCommand CIA461Unit { get; private set; }

        public static ModbusCommand CIA461WeightStatusCenterOfZero { get; private set; }

        public static ModbusCommand CIA461WeightStatusGeneralWeightError { get; private set; }

        public static ModbusCommand CIA461WeightStatusInsideZero { get; private set; }

        public static ModbusCommand CIA461WeightStatusLimitStatus { get; private set; }

        public static ModbusCommand CIA461WeightStatusManualTare { get; private set; }

        public static ModbusCommand CIA461WeightStatusScaleAlarm { get; private set; }

        public static ModbusCommand CIA461WeightStatusScaleRange { get; private set; }

        public static ModbusCommand CIA461WeightStatusScaleSealIsOpen { get; private set; }

        public static ModbusCommand CIA461WeightStatusWeightMoving { get; private set; }

        public static ModbusCommand CIA461WeightStatusWeightType { get; private set; }

        public static ModbusCommand CIA461WeightStatusZeroRequired { get; private set; }

        public static ModbusCommand ControlWordAbortDosing { get; private set; }

        public static ModbusCommand ControlWordActivateData { get; private set; }

        public static ModbusCommand ControlWordAdjustNominal { get; private set; }

        public static ModbusCommand ControlWordAdjustZero { get; private set; }

        public static ModbusCommand ControlWordClearDosingResults { get; private set; }

        public static ModbusCommand ControlWordGrossNet { get; private set; }

        public static ModbusCommand ControlWordManualReDosing { get; private set; }

        public static ModbusCommand ControlWordRecordWeight { get; private set; }

        public static ModbusCommand ControlWordStartDosing { get; private set; }

        public static ModbusCommand ControlWordTare { get; private set; }

        public static ModbusCommand ControlWordZeroing { get; private set; }

        public static ModbusCommand PLCComHandshake { get; private set; }

        public static ModbusCommand PLCComResetHandshake { get; private set; }

        public static ModbusCommand PLCComStatus { get; private set; }

        public static ModbusCommand CurrentProductParameterSet { get; private set; }

        public static ModbusCommand FillingStateAdcOverUnderload { get; private set; }

        public static ModbusCommand FillingStateAlarm { get; private set; }

        public static ModbusCommand FillingStateCoarseFlow { get; private set; }

        public static ModbusCommand FillingStateEmptying { get; private set; }

        public static ModbusCommand FillingStateFineFlow { get; private set; }

        public static ModbusCommand FillingStateFlowError { get; private set; }

        public static ModbusCommand FillingStateGeneralScaleError { get; private set; }

        public static ModbusCommand FillingStateLegalForTradeOperation { get; private set; }

        public static ModbusCommand FillingStateMaximalDosingTimeInput { get; private set; }

        public static ModbusCommand FillingStateReady { get; private set; }

        public static ModbusCommand FillingStateReDosing { get; private set; }

        public static ModbusCommand FillingStateStatusInput1 { get; private set; }

        public static ModbusCommand FillingStateToleranceErrorMinus { get; private set; }

        public static ModbusCommand FillingStateToleranceErrorPlus { get; private set; }

        public static ModbusCommand WeightMemDayFiller { get; private set; }

        public static ModbusCommand WeightMemDayStandard { get; private set; }

        public static ModbusCommand WeightMemGrossFiller { get; private set; }

        public static ModbusCommand WeightMemGrossStandard { get; private set; }

        public static ModbusCommand WeightMemMonthFiller { get; private set; }

        public static ModbusCommand WeightMemMonthStandard { get; private set; }

        public static ModbusCommand WeightMemNetFiller { get; private set; }

        public static ModbusCommand WeightMemNetStandard { get; private set; }

        public static ModbusCommand WeightMemSeqNumberFiller { get; private set; }

        public static ModbusCommand WeightMemSeqNumberStandard { get; private set; }

        public static ModbusCommand WeightMemYearFiller { get; private set; }

        public static ModbusCommand WeightMemYearStandard { get; private set; }

        public static ModbusCommand CBKCoarseFlowMonitoring { get; private set; }

        public static ModbusCommand CBTCoarseFlowMonitoringTime { get; private set; }

        public static ModbusCommand CFDCoarseFlowDisconnect { get; private set; }

        public static ModbusCommand CFTCoarseFlowTime { get; private set; }

        public static ModbusCommand CWTScaleCalibrationWeight { get; private set; }

        public static ModbusCommand DL1DosingDelay1 { get; private set; }

        public static ModbusCommand DL2DosingDelay2 { get; private set; }

        public static ModbusCommand DMDDosingMode { get; private set; }

        public static ModbusCommand DSTDosingTime { get; private set; }

        public static ModbusCommand EMDEmptyingMode { get; private set; }

        public static ModbusCommand EWTEmptyWeight { get; private set; }

        public static ModbusCommand FBKFineFlowMonitoring { get; private set; }

        public static ModbusCommand FBTFineFlowMonitoringTime { get; private set; }

        public static ModbusCommand FFDFineFlowDisconnect { get; private set; }

        public static ModbusCommand FFLFirstFineFlow { get; private set; }

        public static ModbusCommand FFMMinimumFineFlow { get; private set; }

        public static ModbusCommand FFTFineFlowTime { get; private set; }

        public static ModbusCommand FWTFillingTargetWeight { get; private set; }

        public static ModbusCommand IMDApplicationMode { get; private set; }

        public static ModbusCommand IS1DigitalInput1 { get; private set; }

        public static ModbusCommand IS2DigitalInput2 { get; private set; }

        public static ModbusCommand IS3DigitalInput3 { get; private set; }

        public static ModbusCommand IS4DigitalInput4 { get; private set; }

        public static ModbusCommand LDWZeroSignal { get; private set; }

        public static ModbusCommand LIV1LimitSwitchHysteresis { get; private set; }

        public static ModbusCommand LIV1LimitSwitchLevel { get; private set; }

        public static ModbusCommand LIV1LimitSwitchMode { get; private set; }

        public static ModbusCommand LIV1LimitSwitchSource { get; private set; }

        public static ModbusCommand LIV2LimitSwitchHysteresis { get; private set; }

        public static ModbusCommand LIV2LimitSwitchLevel { get; private set; }

        public static ModbusCommand LIV2LimitSwitchMode { get; private set; }

        public static ModbusCommand LIV2LimitSwitchSource { get; private set; }

        public static ModbusCommand LIV3LimitSwitchHysteresis { get; private set; }

        public static ModbusCommand LIV3LimitSwitchLevel { get; private set; }

        public static ModbusCommand LIV3LimitSwitchMode { get; private set; }

        public static ModbusCommand LIV3LimitSwitchSource { get; private set; }

        public static ModbusCommand LIV4LimitSwitchHysteresis { get; private set; }

        public static ModbusCommand LIV4LimitSwitchLevel { get; private set; }

        public static ModbusCommand LIV4LimitSwitchMode { get; private set; }

        public static ModbusCommand LIV4LimitSwitchSource { get; private set; }

        public static ModbusCommand LTCLockoutTimeCoarseFlow { get; private set; }

        public static ModbusCommand LTFLockoutTimeFineFlow { get; private set; }

        public static ModbusCommand LTLLowerToleranceLimit { get; private set; }

        public static ModbusCommand LVSLimitValueStatus { get; private set; }

        public static ModbusCommand LWTNominalSignal { get; private set; }

        public static ModbusCommand MDTMaximalFillingTime { get; private set; }

        public static ModbusCommand MSWMinimumStartWeight { get; private set; }

        public static ModbusCommand OS1DigitalOutput1 { get; private set; }

        public static ModbusCommand OS2DigitalOutput2 { get; private set; }

        public static ModbusCommand OS3DigitalOutput3 { get; private set; }

        public static ModbusCommand OS4DigitalOutput4 { get; private set; }

        public static ModbusCommand OSNOptimization { get; private set; }

        public static ModbusCommand RFTResidualFlowTime { get; private set; }

        public static ModbusCommand SUMFillingResultSum { get; private set; }

        public static ModbusCommand SYDSystematicDifference { get; private set; }

        public static ModbusCommand TADTareDelay { get; private set; }

        public static ModbusCommand TMDTareMode { get; private set; }

        public static ModbusCommand UTLUpperToleranceLimit { get; private set; }

        public static ModbusCommand VCTValveControl { get; private set; }

        public static ModbusCommand Zero { get; private set; }
        #endregion

    }
}
