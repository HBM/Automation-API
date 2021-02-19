// <copyright file="WeightMemory.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// Holds a weight memory record
    /// </summary>
    public class WeightMemory
    {
        #region ======================== properties ========================

        /// <summary>
        /// Gets a value indicating the date of this weight memory record
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets a value indicating the record ID of this weight memory record
        /// </summary>
        public int RecordID { get; private set; }

        /// <summary>
        /// Gets a value indicating the gross value of this weight memory record
        /// </summary>
        public int Gross { get; private set; }

        /// <summary>
        /// Gets a value indicating the net value of this weight memory record
        /// </summary>
        public int Net { get; private set; }
        #endregion

        #region ================ public & internal methods =================

        /// <summary>
        /// Updates this weight memory record
        /// </summary>
        /// <param name="year">Year of recording</param>
        /// <param name="month">Month of recording</param>
        /// <param name="day">Day of recording</param>
        /// <param name="net">Net value of recording</param>
        /// <param name="gross">Gross value of recording</param>
        /// <param name="id">ID of recording</param>
        public void Update(int year, int month, int day, int net, int gross, int id)
        {
            Date = new DateTime(year, month, day);
            RecordID = id;
            Gross = gross;
            Net = net;
        }
        #endregion
    }
}
