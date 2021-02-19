// <copyright file="PrintableWeightType.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using System.Globalization;

    /// <summary>
    /// Holds the current weight values (gross, net, tare)
    /// </summary>
    public class PrintableWeightType
    {
        #region ==================== constants & fields ====================

        private NumberFormatInfo setPrecision;

        #endregion

        #region =============== constructors & destructors =================

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintableWeightType" /> class.
        /// </summary>
        public PrintableWeightType()
        {
            setPrecision = new NumberFormatInfo();

            Net   = "0";
            Gross = "0";
            Tare  = "0";
        }

        #endregion

        #region ======================== properties ========================

        /// <summary>
        /// Gets the gross value of weight in string without a unit
        /// </summary>
        public string Gross { get; private set; }

        /// <summary>
        /// Gets the net value of weight in string without a unit
        /// </summary>
        public string Net { get; private set; }

        /// <summary>
        /// Gets the tare value of weight in string without a unit
        /// </summary>
        public string Tare { get; private set; }

        #endregion

        #region ================ public & internal methods =================

        /// <summary>
        /// Updates the device properties net, gross and tare from external
        /// </summary>
        /// <param name="net"></param>
        /// <param name="gross"></param>
        /// <param name="tare"></param>
        /// <param name="decimals"></param>
        public void Update(double net, double gross, double tare, int decimals)
        {          
            setPrecision.NumberDecimalDigits = decimals;

            Net = ((decimal)net).ToString("F", setPrecision);
            Gross = ((decimal)gross).ToString("F", setPrecision);
            Tare = ((decimal)tare).ToString("F", setPrecision);
        }

        #endregion
    }
}