// <copyright file="IProcessData.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// Interface containing the process data of a device.
    /// Among these are gross weight, net and weight status signals (e.g. TareMode or ScaleRange).
    /// </summary>
    public interface IProcessData
    {

        #region ==================== events & delegates ====================
        /// <summary>
        /// Use this Method to update all properties of this interface.
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">EventArgs of this event</param>
        void UpdateData(object sender, EventArgs e);
        #endregion

        #region ======================== properties ========================
        /// <summary>
        /// Gets the application mode (e.g. Standard/Filler)
        /// </summary>
        ApplicationMode ApplicationMode { get; }

        /// <summary>
        /// Gets the weight values gross, net , tare as double
        /// </summary>
        WeightType Weight { get; }

        /// <summary>
        /// Gets the weight values gross, net , tare as string
        /// </summary>
        PrintableWeightType PrintableWeight { get; }

        /// <summary>
        /// Gets the engineering unit (e.g. "g", "kg",´"t", "lb", "N")
        /// </summary>
        string Unit { get; }

        /// <summary>
        /// Gets the number of decimals
        /// </summary>
        int Decimals { get; }

        /// <summary>
        /// Gets the current tare mode
        /// </summary>
        TareMode TareMode { get; }

        /// <summary>
        /// Gets a value indicating whether the weight is stable
        /// </summary>
        bool WeightStable { get; }

        /// <summary>
        /// Gets a value indicating whether the scale is in the center-of-zero status
        /// </summary>
        bool CenterOfZero { get; }

        /// <summary>
        /// Gets a value indicating whether the weight is inside zeroing range
        /// </summary>
        bool InsideZero { get; }

        /// <summary>
        /// Gets a value indicating whether zeroing is required
        /// </summary>
        bool ZeroRequired { get; }

        /// <summary>
        /// Gets a value indicating the scale range (ranges 1, 2, 3 are valid)
        /// </summary>
        int ScaleRange { get; }

        /// <summary>
        /// Gets a value indicating whether the scale is in legal-for-trade mode
        /// </summary>
        bool LegalForTrade { get; }

        /// <summary>
        /// Gets a value indicating whether the scale is in underload 
        /// </summary>
        bool Underload { get; }

        /// <summary>
        /// Gets a value indicating whether the scale is in overload 
        /// </summary>
        bool Overload { get; }

        /// <summary>
        /// Gets a value indicating whether the scale is higher than safe load limit
        /// </summary>
        bool HigherSafeLoadLimit { get; }

        /// <summary>
        /// Gets a value indicating whether the scale is in error status
        /// </summary>
        bool GeneralScaleError { get; }

        /// <summary>
        /// Gets a value indicating whether the scale alarm is active
        /// </summary>
        bool ScaleAlarm { get; }
        #endregion


    }
}