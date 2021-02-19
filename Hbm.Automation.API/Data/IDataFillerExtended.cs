// <copyright file="IDataFillerExtended.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// <summary>
    /// Interface containing the data for the filler extended mode of your WTX device.
    /// A class inheriting from interface IDataFillerExtended contains the input word 
    /// and output words for the extended filler mode of WTX device 120 and 110.
    /// 
    /// This is only available via a JetBus Ethernet connection, not via Modbus. 
    /// </summary>
    public interface IDataFillerExtended : IDataFiller
    {
        #region ======================== properties ========================    

        /// <summary>
        /// Gets the material stream of the last filling cycle
        /// </summary>                 
        double MaterialStreamLastFilling { get; }

        /// <summary>
        /// Gets or sets the special filling functions
        /// </summary>
        int SpecialFillingFunctions { get; set; }

        /// <summary>
        /// Gets or sets the discharge time in ma
        /// </summary>
        int DischargeTime { get; set; }

        /// <summary>
        /// Gets or sets the mode empty weight underflow
        /// </summary>
        bool EmptyWeightBreak { get; set; }

        /// <summary>
        /// Gets or sets the delay 1 after filling complete in ms
        /// </summary>
        int Delay1Dosing { get; set; }

        /// <summary>
        /// Gets or sets the delay 2 after filling complete in ms
        /// </summary>
        int Delay2Dosing { get; set; }

        /// <summary>
        /// Gets or sets the minimum emtpy weight in weight unit
        /// </summary>
        double EmptyWeightTolerance { get; set; }

        /// <summary>
        /// Gets or sets the residual flow from last filling in weight unit
        /// </summary>
        double ResidualFlowDosingCycle { get; set; }

        /// <summary>
        /// Gets or sets the current product (= parameter set) 
        /// </summary>
        new int ParameterSetProduct { get; set; }
        #endregion

    }
}
