// <copyright file="ProcessDataReceivedEventArgs.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// Event to extend the event based call with an interface containing real-time process data.
    /// Called by the update method ("OnData()") of the classes WtxJet and WtxModbus to send the
    /// process data to the application class.
    /// </summary>
    public class ProcessDataReceivedEventArgs : EventArgs
    {
        #region =============== constructors & destructors =================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessDataReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="processData">Instance of interface IProcessData</param>
        public ProcessDataReceivedEventArgs(IProcessData processData)
        {
            ProcessData = processData;
        }
        #endregion

        #region ======================== properties ========================
        /// <summary>
        /// Gets or sets an instance of interface IProcessData containing the process data
        /// </summary>
        public IProcessData ProcessData { get; set; }
        #endregion
    }
}