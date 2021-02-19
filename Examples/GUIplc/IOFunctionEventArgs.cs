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
    /// Event argument for the selected input and output function.
    /// </summary>
    public class IOFunctionEventArgs : EventArgs
    {

        #region ==================== constants & fields ====================

        private OutputFunction _out1;
        private OutputFunction _out2;
        private OutputFunction _out3;
        private OutputFunction _out4;
        private InputFunction _in1;
        private InputFunction _in2;

        #endregion

        #region =============== constructors & destructors =================

        public IOFunctionEventArgs(OutputFunction Out1, OutputFunction Out2, OutputFunction Out3, OutputFunction Out4, InputFunction In1, InputFunction In2)
        {
            _out1 = Out1;
            _out2 = Out2;
            _out3 = Out3;
            _out4 = Out4;

            _in1 = In1;
            _in2 = In2;
        }

        #endregion

        #region ======================== properties ========================

        public OutputFunction FunctionOutputIO1
        {
            get
            {
                return _out1;
            }
            set
            {
                _out1 = value;
            }
        }
        public OutputFunction FunctionOutputIO2
        {
            get
            {
                return _out2;
            }
            set
            {
                _out2 = value;
            }
        }
        public OutputFunction FunctionOutputIO3
        {
            get
            {
                return _out3;
            }
            set
            {
                _out3 = value;
            }
        }
        public OutputFunction FunctionOutputIO4
        {
            get
            {
                return _out4;
            }
            set
            {
                _out4 = value;
            }
        }
        public InputFunction FunctionInputIO1
        {
            get
            {
                return _in1;
            }
            set
            {
                _in1 = value;
            }
        }
        public InputFunction FunctionInputIO2
        {
            get
            {
                return _in2;
            }
            set
            {
                _in2 = value;
            }
        }

        #endregion

    }
}
