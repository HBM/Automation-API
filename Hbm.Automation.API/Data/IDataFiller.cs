// <copyright file="IDataFiller.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using System;

    /// <summary>
    /// Interface containing the data for the filler mode of your WTX device.
    /// A class inheriting from interface IDataFiller contains the input word 
    /// and output words for the filler mode of WTX device 120 and 110.
    /// </summary>
    public interface IDataFiller
    {

        #region ==================== events & delegates ====================
        void UpdateFillerData(object sender, EventArgs e);
        #endregion

        #region ======================== properties ========================
        int CoarseFlow { get; }

        int FineFlow { get; }

        int Ready { get; }

        int ReDosing { get; }

        int Emptying { get; }

        int FlowError { get; }

        int Alarm { get; }

        int AdcOverUnderload { get; }

        int FillingProcessStatus { get; }

        double FillingResult { get; }

        int FillingResultCount { get; }

        double FillingResultMeanValue { get; }

        double FillingResultStandardDeviation { get; }

        double FillingResultTotalSum { get; }

        int CurrentDosingTime { get; }

        int CurrentCoarseFlowTime { get; }

        int CurrentFineFlowTime { get; }

        int ToleranceErrorPlus { get; }

        int ToleranceErrorMinus { get; }

        int ParameterSetProduct { get; }
        
        int ResidualFlowTime { get; set; }

        double TargetFillingWeight { get;  set; }

        double CoarseFlowCutOffLevel { get;  set; }

        double FineFlowCutOffLevel { get;  set; }

        double MinimumFineFlow { get;  set; }

        int OptimizationMode { get;  set; }

        int MaxFillingTime { get;  set; }

        int StartWithFineFlow { get;  set; }
        
        int TareMode { get;  set; }

        int TareDelay { get;  set; }

        double UpperToleranceLimit { get;  set; }

        double LowerToleranceLimit { get;  set; }

        double MinimumStartWeight { get;  set; }

        double EmptyWeight { get;  set; }

        int CoarseLockoutTime { get;  set; }

        int FineLockoutTime { get;  set; }

        double CoarseFlowMonitoring { get;  set; }

        int CoarseFlowMonitoringTime { get;  set; }

        double FineFlowMonitoring { get;  set; }

        int FineFlowMonitoringTime { get;  set; }

        int DelayTimeAfterFilling { get;  set; }

        int ActivationTimeAfterFilling { get;  set; }

        double SystematicDifference { get;  set; }

        int FillingMode { get;  set; }

        int ValveControl { get;  set; }

        int EmptyingMode { get;  set; }
        #endregion

        #region ================ public & internal methods ================= 
        void StartFilling();

        void BreakFilling();

        void ClearFillingResult();
        #endregion
    }
}