// <copyright file="IDataDigitalFilter.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// Class containing extended data and functionality to BaseWtDevice.
    /// It inherits from BaseWtdevice. Used by a Jetbus connection and application.
    /// </summary>
    public interface IDataDigitalFilter
    {

        #region ==================== events & delegates ====================
        #endregion

        #region ======================== properties ========================

        /// <summary>
        /// Gets or sets the data rate in Hz
        /// </summary>
        int DataRate { get; set; }

        /// <summary>
        /// Gets or sets the mode of the low-pass filter
        /// </summary>
        LowPassFilter LowPassFilterMode { get; set; }

        /// <summary>
        /// Gets or sets the order of the low-pass filter
        /// </summary>
        int LowPassFilterOrder { get; set; }

        /// <summary>
        /// Gets or sets the cut-off frequency of the low-pass filter
        /// </summary>
        int LowPasCutOffFrequency { get; set; }

        #endregion

    }
}
