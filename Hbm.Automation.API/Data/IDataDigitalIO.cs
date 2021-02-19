// <copyright file="IDataDigitalIO.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// Interface containing the data for the standard mode of your WTX device.
    /// (e.g. settings for limit switches and digital I/O)
    /// </summary>
    public interface IDataDigitalIO
    {

        #region ==================== events & delegates ====================
        /// <summary>
        /// Updates the data from external
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">EventArgs</param>
        void UpdateDataIO(object sender, EventArgs e);
        #endregion

        #region ======================== properties ========================
        /// <summary>
        /// Gets a value indicating whether the digital input 1 is active or not
        /// </summary>
        bool Input1 { get; }

        /// <summary>
        /// Gets a value indicating whether the digital input 2 is active or not
        /// </summary>
        bool Input2 { get; }

        /// <summary>
        /// Gets a value indicating whether the digital input 3 is active or not
        /// </summary>
        bool Input3 { get; }

        /// <summary>
        /// Gets a value indicating whether the digital input 4 is active or not
        /// </summary>
        bool Input4 { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the digital output 1 is active or not
        /// </summary>
        bool Output1 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the digital output 2 is active or not
        /// </summary>
        bool Output2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the digital output 3 is active or not
        /// </summary>
        bool Output3 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the digital output 4 is active or not
        /// </summary>
        bool Output4 { get; set; }      
        #endregion

    }
}
