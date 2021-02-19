// <copyright file="JetBusCommands.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.WTX.Jet
{
    /// <summary>
    /// Class with all JetBus commands and additional command information 
    /// </summary>
    public static class JetBusCommands
    {
        #region =============== constructors & destructors =================
        static JetBusCommands()
        {
            CIA461CalibrationWeight = new JetBusCommand(DataType.S32, "6152/00", 0, 0);
            CIA461CertificaitonInfoNTEP = new JetBusCommand(DataType.ASCII, "6138/02", 0, 0);
            CIA461CertificaitonInfoOIML = new JetBusCommand(DataType.ASCII, "6138/01", 0, 0);
            CIA461Decimals = new JetBusCommand(DataType.U08, "6013/01", 0, 0);
            CIA461FilterBesselCutOffFrequency = new JetBusCommand(DataType.U08, "60B1/02", 0, 0);
            CIA461FilterBesselFilterOrder = new JetBusCommand(DataType.U08, "60B1/01", 0, 0);
            CIA461FilterButterworthCutOffFrequency = new JetBusCommand(DataType.U08, "60A2/02", 0, 0);
            CIA461FilterButterworthFilterOrder = new JetBusCommand(DataType.U08, "60A2/01", 0, 0);
            CIA461FilterCriticallyDampedCutOffFrequency = new JetBusCommand(DataType.U08, "60A1/02", 0, 0);
            CIA461FilterCriticallyDampedFilterOrder = new JetBusCommand(DataType.U08, "60A1/01", 0, 0);
            CIA461GrossValue = new JetBusCommand(DataType.S32, "6144/00", 0, 0);
            CIA461ImplementedProfileSpecification = new JetBusCommand(DataType.U32, "1030/01", 0, 0);
            CIA461LoadCellCapability = new JetBusCommand(DataType.U32, "6000/01", 0, 0);
            CIA461LocalGravityFactor = new JetBusCommand(DataType.S32, "6021/01", 0, 0);
            CIA461MaximumZeroingTime = new JetBusCommand(DataType.U16, "6141/02", 0, 0);
            CIA461MultiIntervalRangeControl = new JetBusCommand(DataType.U08, "611C/01", 0, 0);
            CIA461MultiLimit1 = new JetBusCommand(DataType.S32, "611C/02", 0, 0);
            CIA461MultiLimit2 = new JetBusCommand(DataType.S32, "611C/03", 0, 0);
            CIA461NetValue = new JetBusCommand(DataType.S32, "601A/01", 0, 0);
            CIA461OutputWeight = new JetBusCommand(DataType.S32, "601A/01", 0, 0);
            CIA461PeakValueGrossMax = new JetBusCommand(DataType.S32, "6149/01", 0, 0);
            CIA461PeakValueGrossMin = new JetBusCommand(DataType.S32, "6149/02", 0, 0);
            CIA461PeakValueMax = new JetBusCommand(DataType.S32, "6149/03", 0, 0);
            CIA461PeakValuMin = new JetBusCommand(DataType.S32, "6149/04", 0, 0);
            CIA461ProductCode = new JetBusCommand(DataType.U32, "1018/02", 0, 0);
            CIA461RestoreAllDefaultParameters = new JetBusCommand(DataType.U32, "1011/01", 0, 0);
            DSERestoreAllDefaultParameters = new JetBusCommand(DataType.U32, "1011/03", 0, 0);
            CIA461SampleRate = new JetBusCommand(DataType.U32, "6050/01", 0, 0);
            CIA461SaveAllParameters = new JetBusCommand(DataType.U32, "1010/01", 0, 0);
            CIA461ScaleAccuracyClass = new JetBusCommand(DataType.U08, "6111/01", 0, 0);
            CIA461ScaleApportionmentFactor = new JetBusCommand(DataType.U08, "6116/01", 0, 0);
            CIA461ScaleCommand = new JetBusCommand(DataType.U32, "6002/01", 0, 0);
            CIA461ScaleCommandStatus = new JetBusCommand(DataType.U32, "6002/02", 0, 0);
            CIA461ScaleFilter = new JetBusCommand(DataType.U16, "6040/01", 0, 0);
            CIA461ScaleMaximumCapacity = new JetBusCommand(DataType.S32, "6113/01", 0, 0);
            CIA461ScaleMaximumNumberOfVerifications = new JetBusCommand(DataType.U32, "6114/01", 0, 0);
            CIA461ScaleMinimumDeadLoad = new JetBusCommand(DataType.S32, "6112/01", 0, 0);
            CIA461ScaleMinimumVerificationInterval = new JetBusCommand(DataType.U32, "611B/01", 0, 0);
            CIA461ScaleOperationTemperatureMaximal = new JetBusCommand(DataType.S16, "6118/03", 0, 0);
            CIA461ScaleOperationTemperatureMinimal = new JetBusCommand(DataType.S16, "6118/02", 0, 0);
            CIA461ScaleOperationTemperatureNominal = new JetBusCommand(DataType.S16, "6118/01", 0, 0);
            CIA461ScaleSafeLoadLimit = new JetBusCommand(DataType.U32, "6117/01", 0, 0);
            CIA461ScaleSettings = new JetBusCommand(DataType.S32, "6020/01", 0, 0);
            CIA461SerialNumber = new JetBusCommand(DataType.U32, "1018/04", 0, 0);
            CIA461SuppyVoltageMaximal = new JetBusCommand(DataType.U08, "6110/03", 0, 0);
            CIA461SuppyVoltageNominal = new JetBusCommand(DataType.U08, "6110/01", 0, 0);
            CIA461SuppyVoltageNominalMinimal = new JetBusCommand(DataType.U08, "6110/02", 0, 0);
            CIA461TareValue = new JetBusCommand(DataType.S32, "6143/00", 0, 0);
            CIA461Unit = new JetBusCommand(DataType.U32, "6015/01", 16, 8);
            CIA461UnitPrefixFixedParameters = new JetBusCommand(DataType.U32, "6014/01", 0, 0);
            CIA461VendorID = new JetBusCommand(DataType.U32, "1018/01", 0, 0);
            CIA461WeightMovingDetection = new JetBusCommand(DataType.U08, "6153/00", 0, 0);
            CIA461WeightStatus = new JetBusCommand(DataType.U16, "6012/01", 0, 0);
            CIA461WeightStatusCenterOfZero = new JetBusCommand(DataType.BIT, "6012/01", 11, 1);
            CIA461WeightStatusGeneralWeightError = new JetBusCommand(DataType.BIT, "6012/01", 0, 1);
            CIA461WeightStatusImplementationSpecificWeightStatus1 = new JetBusCommand(DataType.BIT, "6012/01", 14, 1);
            CIA461WeightStatusImplementationSpecificWeightStatus2 = new JetBusCommand(DataType.BIT, "6012/01", 14, 1);
            CIA461WeightStatusInsideZero = new JetBusCommand(DataType.BIT, "6012/01", 12, 1);
            CIA461WeightStatusLimitStatus = new JetBusCommand(DataType.BIT, "6012/01", 2, 2);
            CIA461WeightStatusManualTare = new JetBusCommand(DataType.BIT, "6012/01", 6, 1);
            CIA461WeightStatusReserved = new JetBusCommand(DataType.BIT, "6012/01", 13, 1);
            CIA461WeightStatusScaleAlarm = new JetBusCommand(DataType.BIT, "6012/01", 1, 1);
            CIA461WeightStatusScaleRange = new JetBusCommand(DataType.BIT, "6012/01", 8, 2);
            CIA461WeightStatusScaleSealIsOpen = new JetBusCommand(DataType.BIT, "6012/01", 5, 1);
            CIA461WeightStatusWeightMoving = new JetBusCommand(DataType.BIT, "6012/01", 4, 1);
            CIA461WeightStatusWeightType = new JetBusCommand(DataType.BIT, "6012/01", 7, 1);
            CIA461WeightStatusZeroRequired = new JetBusCommand(DataType.BIT, "6012/01", 10, 1);
            CIA461WeightStep = new JetBusCommand(DataType.U08, "6016/01", 0, 0);
            CIA461ZeroValue = new JetBusCommand(DataType.S32, "6142/00", 0, 0);
            ADRDeviceAddress = new JetBusCommand(DataType.U08, "2600/01", 0, 0);
            Alarms = new JetBusCommand(DataType.U16, "6018/01", 0, 0);
            BRKBreakFilling = new JetBusCommand(DataType.NIL, "2240/01", 0, 0);
            CBKCoarseFlowMonitoring = new JetBusCommand(DataType.S32, "2210/01", 0, 0);
            CBTCoarseFlowMonitoringTime = new JetBusCommand(DataType.U16, "2220/01", 0, 0);
            CFDCoarseFlowDisconnect = new JetBusCommand(DataType.S32, "2210/02", 0, 0);
            CFTCoarseFlowTime = new JetBusCommand(DataType.U16, "2230/01", 0, 0);
            CSNClearFillingResult = new JetBusCommand(DataType.NIL, "2230/02", 0, 0);
            DL1DosingDelay1 = new JetBusCommand(DataType.U16, "2220/0B", 0, 0);
            DL2DosingDelay2 = new JetBusCommand(DataType.U16, "2220/0C", 0, 0);
            DMDFillingMode = new JetBusCommand(DataType.U08, "2200/04", 0, 0);
            DSTDosingTime = new JetBusCommand(DataType.U16, "2230/03", 0, 0);
            EMDEmptyingMode = new JetBusCommand(DataType.U08, "2200/05", 0, 0);
            EPTDischargeTime = new JetBusCommand(DataType.U16, "2220/02", 0, 0);
            ESRErrorRegister = new JetBusCommand(DataType.U08, "1001/00", 0, 0);
            EWBEmptyWeightBreak = new JetBusCommand(DataType.U08, "2200/0F", 0, 0);
            EWTEmptyWeight = new JetBusCommand(DataType.S32, "2210/03", 0, 0);
            FBKFineFlowMonitoring = new JetBusCommand(DataType.S32, "2210/04", 0, 0);
            FBTFineFlowMonitoringTime = new JetBusCommand(DataType.U16, "2220/03", 0, 0);
            FFDFineFlowDisconnect = new JetBusCommand(DataType.S32, "2210/05", 0, 0);
            FFLFirstFineFlow = new JetBusCommand(DataType.U16, "2220/0A", 0, 0);
            FFMMinimumFineFlow = new JetBusCommand(DataType.S32, "2210/06", 0, 0);
            FFTFineFlowTime = new JetBusCommand(DataType.U16, "2230/04", 0, 0);
            FRS1FillingResult = new JetBusCommand(DataType.S32, "2000/05", 0, 0);
            FRS2FillingResultState = new JetBusCommand(DataType.U16, "2000/06", 0, 0);
            FWTFillingTargetWeight = new JetBusCommand(DataType.S32, "2210/07", 0, 0);
            HWVHardwareVersion = new JetBusCommand(DataType.ASCII, "2520/0A", 0, 0);
            DSEHWRevision = new JetBusCommand(DataType.ASCII, "1009/00", 0, 0);
            DSEIDNDeviceIdentification = new JetBusCommand(DataType.ASCII, "1008/00", 0, 0);
            DSESerialNumber = new JetBusCommand(DataType.U32, "4280/04", 0, 0);
            DSEFirmwareVersion = new JetBusCommand(DataType.ASCII, "100A/00", 0, 0);
            DSEFilterSetup = new JetBusCommand(DataType.U32, "6040/00", 0, 0);
            DSELowPassCutOffFrequencyFIR = new JetBusCommand(DataType.U32, "3311/02", 0, 0);
            DSELowPassCutOffFrequencyIIR = new JetBusCommand(DataType.U32, "60A1/02", 0, 0);
            DSEFilterModeStage2 = new JetBusCommand(DataType.U32, "6040/02", 0, 0);
            DSEFilterModeStage3 = new JetBusCommand(DataType.U32, "6040/03", 0, 0);
            DSEFilterModeStage4 = new JetBusCommand(DataType.U32, "6040/04", 0, 0);
            DSEFilterModeStage5 = new JetBusCommand(DataType.U32, "6040/05", 0, 0);
            DSECombFilterFrequencyStage2 = new JetBusCommand(DataType.U32, "3321/00", 0, 0);
            DSECombFilterFrequencyStage3 = new JetBusCommand(DataType.U32, "3322/00", 0, 0);
            DSECombFilterFrequencyStage4 = new JetBusCommand(DataType.U32, "3323/00", 0, 0);
            DSECombFilterFrequencyStage5 = new JetBusCommand(DataType.U32, "3324/00", 0, 0);
            DSEMovAvFilterFrequencyStage2 = new JetBusCommand(DataType.U32, "3331/00", 0, 0);
            DSEMovAvFilterFrequencyStage3 = new JetBusCommand(DataType.U32, "3332/00", 0, 0);
            DSEMovAvFilterFrequencyStage4 = new JetBusCommand(DataType.U32, "3333/00", 0, 0);
            DSEMovAvFilterFrequencyStage5 = new JetBusCommand(DataType.U32, "3334/00", 0, 0);
            SDLScaleZeroSignal = new JetBusCommand(DataType.S32, "6150/00", 0, 0);
            SNLScaleNominalSignal = new JetBusCommand(DataType.S32, "6151/00", 0, 0);
            DSEZeroValue = new JetBusCommand(DataType.S32, "6142/00", 0, 0);
            IDNDeviceIdentification = new JetBusCommand(DataType.ASCII, "2520/01", 0, 0);
            IM1DigitalInput1Mode = new JetBusCommand(DataType.U08, "2022/01", 0, 0);
            IM2DigitalInput2Mode = new JetBusCommand(DataType.U08, "2022/02", 0, 0);
            IM3DigitalInput3Mode = new JetBusCommand(DataType.U08, "2022/03", 0, 0);
            IM4DigitalInput4Mode = new JetBusCommand(DataType.U08, "2022/04", 0, 0);
            IMDApplicationMode = new JetBusCommand(DataType.U08, "2010/07", 0, 0);
            IS1DigitalInput1 = new JetBusCommand(DataType.U08, "2020/18", 0, 0);
            IS2DigitalInput2 = new JetBusCommand(DataType.U08, "2020/19", 0, 0);
            IS3DigitalInput3 = new JetBusCommand(DataType.U08, "2020/1A", 0, 0);
            IS4DigitalInput4 = new JetBusCommand(DataType.U08, "2020/1B", 0, 0);
            LDWZeroValue = new JetBusCommand(DataType.S32, "2110/06", 0, 0);
            LIV1LimitSwitchHysteresis = new JetBusCommand(DataType.S32, "2030/04", 0, 0);
            LIV1LimitSwitchLevel = new JetBusCommand(DataType.S32, "2030/03", 0, 0);
            LIV1LimitSwitchMode = new JetBusCommand(DataType.U08, "2030/01", 0, 0);
            LIV1LimitSwitchSource = new JetBusCommand(DataType.U08, "2030/02", 0, 0);
            LIV2LimitSwitchHysteresis = new JetBusCommand(DataType.S32, "2030/08", 0, 0);
            LIV2LimitSwitchLevel = new JetBusCommand(DataType.S32, "2030/07", 0, 0);
            LIV2LimitSwitchMode = new JetBusCommand(DataType.U08, "2030/05", 0, 0);
            LIV2LimitSwitchSource = new JetBusCommand(DataType.U08, "2030/06", 0, 0);
            LIV3LimitSwitchHysteresis = new JetBusCommand(DataType.S32, "2030/0C", 0, 0);
            LIV3LimitSwitchLevel = new JetBusCommand(DataType.S32, "2030/0B", 0, 0);
            LIV3LimitSwitchMode = new JetBusCommand(DataType.U08, "2030/09", 0, 0);
            LIV3LimitSwitchSource = new JetBusCommand(DataType.U08, "2030/0A", 0, 0);
            LIV4LimitSwitchHysteresis = new JetBusCommand(DataType.S32, "2030/10", 0, 0);
            LIV4LimitSwitchLevel = new JetBusCommand(DataType.S32, "2030/0F", 0, 0);
            LIV4LimitSwitchMode = new JetBusCommand(DataType.U08, "2030/0D", 0, 0);
            LIV4LimitSwitchSource = new JetBusCommand(DataType.U08, "2030/0E", 0, 0);
            LTCLockoutTimeCoarseFlow = new JetBusCommand(DataType.U16, "2220/04", 0, 0);
            LTFLockoutTimeFineFlow = new JetBusCommand(DataType.U16, "2220/05", 0, 0);
            LTLLowerToleranceLimit = new JetBusCommand(DataType.S32, "2210/08", 0, 0);
            LVSLimitValueStatus = new JetBusCommand(DataType.U08, "2020/25", 0, 1);
            LVSLimitValue1Status = new JetBusCommand(DataType.BIT, "2020/25", 0, 1);
            LVSLimitValue2Status = new JetBusCommand(DataType.BIT, "2020/25", 1, 1);
            LVSLimitValue3Status = new JetBusCommand(DataType.BIT, "2020/25", 2, 1);
            LVSLimitValue4Status = new JetBusCommand(DataType.BIT, "2020/25", 3, 1);
            LWTNominalValue = new JetBusCommand(DataType.S32, "2110/07", 0, 0);
            MDTMaxFillingTime = new JetBusCommand(DataType.U16, "2220/06", 0, 0);
            MFOMaterialFlow = new JetBusCommand(DataType.S32, "2000/0E", 0, 0);
            MSWMinimumStartWeight = new JetBusCommand(DataType.S32, "2210/0B", 0, 0);
            NDSFillingCounter = new JetBusCommand(DataType.U16, "2230/05", 0, 0);
            NOVScaleCapacity = new JetBusCommand(DataType.S32, "2110/0A", 0, 0);
            OM1DigitalOutput1Mode = new JetBusCommand(DataType.U08, "2021/01", 0, 0);
            OM2DigitalOutput2Mode = new JetBusCommand(DataType.U08, "2021/02", 0, 0);
            OM3DigitalOutput3Mode = new JetBusCommand(DataType.U08, "2021/03", 0, 0);
            OM4DigitalOutput4Mode = new JetBusCommand(DataType.U08, "2021/04", 0, 0);
            OS1DigitalOutput1 = new JetBusCommand(DataType.U08, "2020/1E", 0, 0);
            OS2DigitalOutput2 = new JetBusCommand(DataType.U08, "2020/1F", 0, 0);
            OS3DigitalOutput3 = new JetBusCommand(DataType.U08, "2020/20", 0, 0);
            OS4DigitalOutput4 = new JetBusCommand(DataType.U08, "2020/21", 0, 0);
            OSNOptimization = new JetBusCommand(DataType.U08, "2200/07", 0, 0);
            PDTFirmwareDate = new JetBusCommand(DataType.ASCII, "2520/05", 0, 0);
            RDPActivateParameterSet = new JetBusCommand(DataType.U08, "2200/02", 0, 0);
            RDSRedosing = new JetBusCommand(DataType.U08, "2200/08", 0, 0);
            RESResetDevice = new JetBusCommand(DataType.NIL, "2D00/04", 0, 0);
            RFOResidualFlow = new JetBusCommand(DataType.S32, "2000/0F", 0, 0);
            RFTResidualFlowTime = new JetBusCommand(DataType.U16, "2220/07", 0, 0);
            RIODigitalIOStatus = new JetBusCommand(DataType.U16, "2020/12", 0, 0);
            RUNStartFilling = new JetBusCommand(DataType.NIL, "2240/02", 0, 0);
            SDFSpecialDosingFunctions = new JetBusCommand(DataType.U08, "2200/0A", 0, 0);
            SDMFillingResultMeanValue = new JetBusCommand(DataType.S32, "2230/06", 0, 0);
            SDOFillingState = new JetBusCommand(DataType.U08, "2D00/02", 0, 0);
            SDSFillingResultStandardDeviation = new JetBusCommand(DataType.S32, "2230/07", 0, 0);
            SMDRecordWeightMode = new JetBusCommand(DataType.U08, "2300/08", 0, 0);
            STORecordWeight = new JetBusCommand(DataType.U08, "2040/05", 0, 0);
            STTStabilzationTime = new JetBusCommand(DataType.U16, "2220/08", 0, 0);
            SUMFillingResultSum = new JetBusCommand(DataType.U32, "2230/08", 0, 0);
            SWISoftwareIdentification = new JetBusCommand(DataType.U32, "2600/22", 0, 0);
            SWVSoftwareVersion = new JetBusCommand(DataType.U32, "2600/16", 0, 0);
            SYDSystematicDifference = new JetBusCommand(DataType.S32, "2210/09", 0, 0);
            TADTareDelay = new JetBusCommand(DataType.U16, "2220/09", 0, 0);
            TIMCurrentDatetime = new JetBusCommand(DataType.U32, "2E00/02", 0, 0);
            TMDTareMode = new JetBusCommand(DataType.U08, "2200/0B", 0, 0);
            UTLUpperToleranceLimit = new JetBusCommand(DataType.S32, "2210/0A", 0, 0);
            VCTValveControl = new JetBusCommand(DataType.U08, "2200/0C", 0, 0);
            WDPWriteDosingParameterSet = new JetBusCommand(DataType.U08, "2200/01", 0, 0);
            WRSWeightMemoryEntry = new JetBusCommand(DataType.ASCII, "2040/06", 0, 0);
        }
        #endregion

        #region ======================== properties ========================
        public static JetBusCommand CIA461CalibrationWeight { get; private set; }

        public static JetBusCommand CIA461CertificaitonInfoNTEP { get; private set; }

        public static JetBusCommand CIA461CertificaitonInfoOIML { get; private set; }

        public static JetBusCommand CIA461Decimals { get; private set; }

        public static JetBusCommand CIA461FilterBesselCutOffFrequency { get; private set; }

        public static JetBusCommand CIA461FilterBesselFilterOrder { get; private set; }

        public static JetBusCommand CIA461FilterButterworthCutOffFrequency { get; private set; }

        public static JetBusCommand CIA461FilterButterworthFilterOrder { get; private set; }

        public static JetBusCommand CIA461FilterCriticallyDampedCutOffFrequency { get; private set; }

        public static JetBusCommand CIA461FilterCriticallyDampedFilterOrder { get; private set; }

        public static JetBusCommand CIA461GrossValue { get; private set; }

        public static JetBusCommand CIA461ImplementedProfileSpecification { get; private set; }

        public static JetBusCommand CIA461LoadCellCapability { get; private set; }

        public static JetBusCommand CIA461LocalGravityFactor { get; private set; }

        public static JetBusCommand CIA461MaximumZeroingTime { get; private set; }

        public static JetBusCommand CIA461MultiIntervalRangeControl { get; private set; }

        public static JetBusCommand CIA461MultiLimit1 { get; private set; }

        public static JetBusCommand CIA461MultiLimit2 { get; private set; }

        public static JetBusCommand CIA461NetValue { get; private set; }

        public static JetBusCommand CIA461OutputWeight { get; private set; }

        public static JetBusCommand CIA461PeakValueGrossMax { get; private set; }

        public static JetBusCommand CIA461PeakValueGrossMin { get; private set; }

        public static JetBusCommand CIA461PeakValueMax { get; private set; }

        public static JetBusCommand CIA461PeakValuMin { get; private set; }

        public static JetBusCommand CIA461ProductCode { get; private set; }

        public static JetBusCommand CIA461RestoreAllDefaultParameters { get; private set; }

        public static JetBusCommand DSERestoreAllDefaultParameters { get; private set; }

        public static JetBusCommand CIA461SampleRate { get; private set; }

        public static JetBusCommand CIA461SaveAllParameters { get; private set; }

        public static JetBusCommand CIA461ScaleAccuracyClass { get; private set; }

        public static JetBusCommand CIA461ScaleApportionmentFactor { get; private set; }

        public static JetBusCommand CIA461ScaleCommand { get; private set; }

        public static JetBusCommand CIA461ScaleCommandStatus { get; private set; }

        public static JetBusCommand CIA461ScaleFilter { get; private set; }

        public static JetBusCommand CIA461ScaleMaximumCapacity { get; private set; }

        public static JetBusCommand CIA461ScaleMaximumNumberOfVerifications { get; private set; }

        public static JetBusCommand CIA461ScaleMinimumDeadLoad { get; private set; }

        public static JetBusCommand CIA461ScaleMinimumVerificationInterval { get; private set; }

        public static JetBusCommand CIA461ScaleOperationTemperatureMaximal { get; private set; }

        public static JetBusCommand CIA461ScaleOperationTemperatureMinimal { get; private set; }

        public static JetBusCommand CIA461ScaleOperationTemperatureNominal { get; private set; }

        public static JetBusCommand CIA461ScaleSafeLoadLimit { get; private set; }

        public static JetBusCommand CIA461ScaleSettings { get; private set; }

        public static JetBusCommand CIA461SerialNumber { get; private set; }

        public static JetBusCommand CIA461SuppyVoltageMaximal { get; private set; }

        public static JetBusCommand CIA461SuppyVoltageNominal { get; private set; }

        public static JetBusCommand CIA461SuppyVoltageNominalMinimal { get; private set; }

        public static JetBusCommand CIA461TareValue { get; private set; }

        public static JetBusCommand CIA461Unit { get; private set; }

        public static JetBusCommand CIA461UnitPrefixFixedParameters { get; private set; }

        public static JetBusCommand CIA461VendorID { get; private set; }

        public static JetBusCommand CIA461WeightMovingDetection { get; private set; }

        public static JetBusCommand CIA461WeightStatus { get; private set; }

        public static JetBusCommand CIA461WeightStatusCenterOfZero { get; private set; }

        public static JetBusCommand CIA461WeightStatusGeneralWeightError { get; private set; }

        public static JetBusCommand CIA461WeightStatusImplementationSpecificWeightStatus1 { get; private set; }

        public static JetBusCommand CIA461WeightStatusImplementationSpecificWeightStatus2 { get; private set; }

        public static JetBusCommand CIA461WeightStatusInsideZero { get; private set; }

        public static JetBusCommand CIA461WeightStatusLimitStatus { get; private set; }

        public static JetBusCommand CIA461WeightStatusManualTare { get; private set; }

        public static JetBusCommand CIA461WeightStatusReserved { get; private set; }

        public static JetBusCommand CIA461WeightStatusScaleAlarm { get; private set; }

        public static JetBusCommand CIA461WeightStatusScaleRange { get; private set; }

        public static JetBusCommand CIA461WeightStatusScaleSealIsOpen { get; private set; }

        public static JetBusCommand CIA461WeightStatusWeightMoving { get; private set; }

        public static JetBusCommand CIA461WeightStatusWeightType { get; private set; }

        public static JetBusCommand CIA461WeightStatusZeroRequired { get; private set; }

        public static JetBusCommand CIA461WeightStep { get; private set; }

        public static JetBusCommand CIA461ZeroValue { get; private set; }

        public static JetBusCommand ADRDeviceAddress { get; private set; }

        public static JetBusCommand Alarms { get; private set; }

        public static JetBusCommand BRKBreakFilling { get; private set; }

        public static JetBusCommand CBKCoarseFlowMonitoring { get; private set; }

        public static JetBusCommand CBTCoarseFlowMonitoringTime { get; private set; }

        public static JetBusCommand CFDCoarseFlowDisconnect { get; private set; }

        public static JetBusCommand CFTCoarseFlowTime { get; private set; }

        public static JetBusCommand CSNClearFillingResult { get; private set; }

        public static JetBusCommand DL1DosingDelay1 { get; private set; }

        public static JetBusCommand DL2DosingDelay2 { get; private set; }

        public static JetBusCommand DMDFillingMode { get; private set; }

        public static JetBusCommand DSTDosingTime { get; private set; }

        public static JetBusCommand EMDEmptyingMode { get; private set; }

        public static JetBusCommand EPTDischargeTime { get; private set; }

        public static JetBusCommand ESRErrorRegister { get; private set; }

        public static JetBusCommand EWBEmptyWeightBreak { get; private set; }

        public static JetBusCommand EWTEmptyWeight { get; private set; }

        public static JetBusCommand FBKFineFlowMonitoring { get; private set; }

        public static JetBusCommand FBTFineFlowMonitoringTime { get; private set; }

        public static JetBusCommand FFDFineFlowDisconnect { get; private set; }

        public static JetBusCommand FFLFirstFineFlow { get; private set; }

        public static JetBusCommand FFMMinimumFineFlow { get; private set; }

        public static JetBusCommand FFTFineFlowTime { get; private set; }

        public static JetBusCommand FRS1FillingResult { get; private set; }

        public static JetBusCommand FRS2FillingResultState { get; private set; }

        public static JetBusCommand FWTFillingTargetWeight { get; private set; }

        public static JetBusCommand HWVHardwareVersion { get; private set; }

        public static JetBusCommand DSEHWRevision { get; private set; }

        public static JetBusCommand DSEIDNDeviceIdentification { get; private set; }

        public static JetBusCommand DSESerialNumber { get; private set; }

        public static JetBusCommand DSEFirmwareVersion { get; private set; }

        public static JetBusCommand DSEFilterSetup { get; private set; }

        public static JetBusCommand DSELowPassCutOffFrequencyFIR { get; private set; }

        public static JetBusCommand DSELowPassCutOffFrequencyIIR { get; private set; }

        public static JetBusCommand DSEFilterModeStage2 { get; private set; }
 
        public static JetBusCommand DSEFilterModeStage3 { get; private set; }
 
        public static JetBusCommand DSEFilterModeStage4 { get; private set; }

        public static JetBusCommand DSEFilterModeStage5 { get; private set; }

        public static JetBusCommand DSECombFilterFrequencyStage2 { get; private set; }

        public static JetBusCommand DSECombFilterFrequencyStage3 { get; private set; }

        public static JetBusCommand DSECombFilterFrequencyStage4 { get; private set; }

        public static JetBusCommand DSECombFilterFrequencyStage5 { get; private set; }

        public static JetBusCommand DSEMovAvFilterFrequencyStage2 { get; private set; }

        public static JetBusCommand DSEMovAvFilterFrequencyStage3 { get; private set; }

        public static JetBusCommand DSEMovAvFilterFrequencyStage4 { get; private set; }

        public static JetBusCommand DSEMovAvFilterFrequencyStage5 { get; private set; }

        public static JetBusCommand SDLScaleZeroSignal { get; private set; }

        public static JetBusCommand SNLScaleNominalSignal { get; private set; }

        public static JetBusCommand DSEZeroValue { get; }

        public static JetBusCommand IDNDeviceIdentification { get; private set; }

        public static JetBusCommand IM1DigitalInput1Mode { get; private set; }

        public static JetBusCommand IM2DigitalInput2Mode { get; private set; }

        public static JetBusCommand IM3DigitalInput3Mode { get; private set; }

        public static JetBusCommand IM4DigitalInput4Mode { get; private set; }

        public static JetBusCommand IMDApplicationMode { get; private set; }

        public static JetBusCommand IS1DigitalInput1 { get; private set; }

        public static JetBusCommand IS2DigitalInput2 { get; private set; }

        public static JetBusCommand IS3DigitalInput3 { get; private set; }

        public static JetBusCommand IS4DigitalInput4 { get; private set; }

        public static JetBusCommand LDWZeroValue { get; private set; }

        public static JetBusCommand LIV1LimitSwitchHysteresis { get; private set; }

        public static JetBusCommand LIV1LimitSwitchLevel { get; private set; }

        public static JetBusCommand LIV1LimitSwitchMode { get; private set; }

        public static JetBusCommand LIV1LimitSwitchSource { get; private set; }

        public static JetBusCommand LIV2LimitSwitchHysteresis { get; private set; }

        public static JetBusCommand LIV2LimitSwitchLevel { get; private set; }

        public static JetBusCommand LIV2LimitSwitchMode { get; private set; }

        public static JetBusCommand LIV2LimitSwitchSource { get; private set; }

        public static JetBusCommand LIV3LimitSwitchHysteresis { get; private set; }

        public static JetBusCommand LIV3LimitSwitchLevel { get; private set; }

        public static JetBusCommand LIV3LimitSwitchMode { get; private set; }

        public static JetBusCommand LIV3LimitSwitchSource { get; private set; }

        public static JetBusCommand LIV4LimitSwitchHysteresis { get; private set; }

        public static JetBusCommand LIV4LimitSwitchLevel { get; private set; }

        public static JetBusCommand LIV4LimitSwitchMode { get; private set; }

        public static JetBusCommand LIV4LimitSwitchSource { get; private set; }

        public static JetBusCommand LTCLockoutTimeCoarseFlow { get; private set; }

        public static JetBusCommand LTFLockoutTimeFineFlow { get; private set; }

        public static JetBusCommand LTLLowerToleranceLimit { get; private set; }

        public static JetBusCommand LVSLimitValueStatus { get; private set; }

        public static JetBusCommand LVSLimitValue1Status { get; private set; }

        public static JetBusCommand LVSLimitValue2Status { get; private set; }

        public static JetBusCommand LVSLimitValue3Status { get; private set; }

        public static JetBusCommand LVSLimitValue4Status { get; private set; }

        public static JetBusCommand LWTNominalValue { get; private set; }

        public static JetBusCommand MDTMaxFillingTime { get; private set; }

        public static JetBusCommand MFOMaterialFlow { get; private set; }

        public static JetBusCommand MSWMinimumStartWeight { get; private set; }

        public static JetBusCommand NDSFillingCounter { get; private set; }

        public static JetBusCommand NOVScaleCapacity { get; private set; }

        public static JetBusCommand OM1DigitalOutput1Mode { get; private set; }

        public static JetBusCommand OM2DigitalOutput2Mode { get; private set; }

        public static JetBusCommand OM3DigitalOutput3Mode { get; private set; }

        public static JetBusCommand OM4DigitalOutput4Mode { get; private set; }

        public static JetBusCommand OS1DigitalOutput1 { get; private set; }

        public static JetBusCommand OS2DigitalOutput2 { get; private set; }

        public static JetBusCommand OS3DigitalOutput3 { get; private set; }

        public static JetBusCommand OS4DigitalOutput4 { get; private set; }

        public static JetBusCommand OSNOptimization { get; private set; }

        public static JetBusCommand PDTFirmwareDate { get; private set; }

        public static JetBusCommand RDPActivateParameterSet { get; private set; }

        public static JetBusCommand RDSRedosing { get; private set; }

        public static JetBusCommand RESResetDevice { get; private set; }

        public static JetBusCommand RFOResidualFlow { get; private set; }

        public static JetBusCommand RFTResidualFlowTime { get; private set; }

        public static JetBusCommand RIODigitalIOStatus { get; private set; }

        public static JetBusCommand RUNStartFilling { get; private set; }

        public static JetBusCommand SDFSpecialDosingFunctions { get; private set; }

        public static JetBusCommand SDMFillingResultMeanValue { get; private set; }

        public static JetBusCommand SDOFillingState { get; private set; }

        public static JetBusCommand SDSFillingResultStandardDeviation { get; private set; }

        public static JetBusCommand SMDRecordWeightMode { get; private set; }

        public static JetBusCommand STORecordWeight { get; private set; }

        public static JetBusCommand STTStabilzationTime { get; private set; }

        public static JetBusCommand SUMFillingResultSum { get; private set; }

        public static JetBusCommand SWISoftwareIdentification { get; private set; }

        public static JetBusCommand SWVSoftwareVersion { get; private set; }

        public static JetBusCommand SYDSystematicDifference { get; private set; }

        public static JetBusCommand TADTareDelay { get; private set; }

        public static JetBusCommand TIMCurrentDatetime { get; private set; }

        public static JetBusCommand TMDTareMode { get; private set; }

        public static JetBusCommand UTLUpperToleranceLimit { get; private set; }

        public static JetBusCommand VCTValveControl { get; private set; }

        public static JetBusCommand WDPWriteDosingParameterSet { get; private set; }

        public static JetBusCommand WRSWeightMemoryEntry { get; private set; }
        
        #endregion
    }
}
