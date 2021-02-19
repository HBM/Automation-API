// <copyright file="SettingsEventArgs.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.Examples.GUIplc
{
    using System;
    /// <summary>
    /// Event argument for the ip adress and timer interval.
    /// </summary>
    public class SettingsEventArgs : EventArgs
    {

        #region ==================== constants & fields ====================

        private string _ipAdress;
        private int _timer;

        #endregion

        #region =============== constructors & destructors =================

        /// <summary>
        /// Constructor of SettingsEvent. Sets the IP address and timer interval
        /// </summary>
        /// <param name="ipAdress"></param>
        /// <param name="timer"></param>
        public SettingsEventArgs(string ipAddress, int timer)
        {
            _ipAdress = ipAddress;
            _timer = timer;
        }

        #endregion

        #region ======================== properties ========================

        public string IPAdress
        {
            get
            {
                return _ipAdress;
            }
        }
        public int TimerInterval
        {
            get
            {
                return _timer;
            }
        }

        #endregion

    }
}
