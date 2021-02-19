// <copyright file="BaseWTXDevice.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing
{
    using Hbm.Automation.Api;
    using Hbm.Automation.Api.Data;

    /// <summary>
    /// Class containing extended data and functionality to BaseWtDevice.
    /// It inherits from BaseWtdevice. Used by a Jetbus connection and application.
    /// </summary>
    public abstract class BaseWTXDevice : BaseWTDevice
    {
        #region =============== constructors & destructors =================
        public BaseWTXDevice(INetConnection connection, int timerIntervalms) : base(connection, timerIntervalms)
        {
        }

        public BaseWTXDevice(INetConnection connection) : base(connection)
        {
        }
        #endregion

        #region ======================== properties ========================

        /// <summary>
        /// Gets or sets the current digital IO status
        /// </summary>
        public abstract IDataDigitalIO DigitalIO { get; set; }

        /// <summary>
        /// Gets or sets the current limit switch configuration and status
        /// </summary>
        public abstract IDataLimitSwitch LimitSwitch { get; set; }

        #endregion

    }
}
