// <copyright file="MeasurementUtils.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Utils
{
    using System;

    /// <summary>
    /// Some basic utilities for conversion of measurement values.
    /// </summary>
    public static class MeasurementUtils
    {
        #region ====================== public methods =======================

        /// <summary>
        /// Converts value from int to double, taking the relevant number of decimals into account.
        /// (e.g. 12340 with 2 decimals => returns 123,4)
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="decimals">Number of relevant decimals in value</param>
        /// <returns>Converted value</returns>
        public static double DigitToDouble(int value, int decimals)
        {
            return (double)value / Math.Pow(10, decimals);
        }

        /// <summary>
        /// Converts value from double to int, taking the relevant number of decimals into account.
        /// (e.g. 123,4 with 2 decimals => returns 12340)
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="decimals">Number of relevant decimals in value</param>
        /// <returns>Converted value</returns>
        public static int DoubleToDigit(double value, int decimals)
        {
            return (int)(value * Math.Pow(10, decimals));
        }

        /// <summary>
        /// Convert string to bool ("0"=False, "1"=True)
        /// </summary>
        /// <param name="boolAsString">Sring representing a boolean</param>
        /// <returns></returns>
        public static bool StringToBool(string boolAsString)
        {
            return boolAsString != "0";
        }

        #endregion
    }
}
