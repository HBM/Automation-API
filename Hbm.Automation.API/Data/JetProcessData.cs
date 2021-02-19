// <copyright file="JetProcessData.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using System.Collections.Generic;
    using Hbm.Automation.Api.Utils;
    using Hbm.Automation.Api.Weighing.WTX.Jet;

    /// <summary>
    /// The class ProcessData contains all process data like net weight, gross weight or tare value.
    /// Jetbus implementation of the interface IProcessData for the process data.
    /// </summary>
    public class JetProcessData : IProcessData
    {

        #region ==================== constants & fields ====================
        private INetConnection _connection;
        #endregion

        #region =============== constructors & destructors =================
        /// <summary>
        /// Constructor of class ProcessDataJet : Initalizes values and connects 
        /// the eventhandler from Connection to the interal update method
        /// </summary>
        /// <param name="Connection">Target connection</param>
        public JetProcessData(INetConnection Connection)
        {
            _connection = Connection;
            _connection.UpdateData += UpdateData;

            PrintableWeight = new PrintableWeightType();
            Weight = new WeightType();
            GeneralScaleError = false;
            ScaleAlarm = false;
            WeightStable = false;
            LegalForTrade = false;
            TareMode = TareMode.None;
            ScaleRange = 0;
            ZeroRequired = false;
            CenterOfZero = false;
            InsideZero = false;
            Decimals = 0;
            Unit = "";
            Underload = false;
            Overload = false;
            HigherSafeLoadLimit = false;
        }
        #endregion

        #region ==================== events & delegates ====================
        ///<inheritdoc/>
        public void UpdateData(object sender, EventArgs e)
        {
            try
            {
                ApplicationMode = (ApplicationMode)Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.IMDApplicationMode));
                GeneralScaleError = Convert.ToBoolean(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusGeneralWeightError)));
                ScaleAlarm = Convert.ToBoolean(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusScaleAlarm)));
                int LimitStatus = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusLimitStatus));
                Underload = (LimitStatus == 1);
                Overload = (LimitStatus == 2);
                HigherSafeLoadLimit = (LimitStatus == 3);
                TareMode = EvaluateTareMode(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusManualTare)), Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusWeightType)));
                WeightStable = !Convert.ToBoolean(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusWeightMoving)));
                LegalForTrade = !Convert.ToBoolean(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusScaleSealIsOpen)));
                ScaleRange = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusScaleRange));
                ZeroRequired = Convert.ToBoolean(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusZeroRequired)));
                CenterOfZero = Convert.ToBoolean(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusCenterOfZero)));
                InsideZero = Convert.ToBoolean(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461WeightStatusInsideZero)));
                Decimals = Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461Decimals));
                Unit = UnitIDToString(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461Unit)));
                Weight.Update(
                    MeasurementUtils.DigitToDouble(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461NetValue)), Decimals), 
                    MeasurementUtils.DigitToDouble(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461GrossValue)), Decimals),
                    MeasurementUtils.DigitToDouble(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461TareValue)), Decimals));
                PrintableWeight.Update(
                    MeasurementUtils.DigitToDouble(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461NetValue)), Decimals), 
                    MeasurementUtils.DigitToDouble(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461GrossValue)), Decimals),
                    MeasurementUtils.DigitToDouble(Convert.ToInt32(_connection.ReadFromBuffer(JetBusCommands.CIA461TareValue)), Decimals),
                    Decimals);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("KeyNotFoundException in class ProcessDataJet, update method");
            }
        }
        #endregion

        #region ======================== properties ========================
        ///<inheritdoc/>
        public ApplicationMode ApplicationMode { get; private set; }

        ///<inheritdoc/>
        public WeightType Weight { get; private set; }

        ///<inheritdoc/>
        public PrintableWeightType PrintableWeight { get; private set; }

        ///<inheritdoc/>
        public string Unit { get; private set; }

        ///<inheritdoc/>
        public int Decimals { get; private set; }

        ///<inheritdoc/>
        public TareMode TareMode { get; private set; }

        ///<inheritdoc/>
        public bool WeightStable { get; private set; }

        ///<inheritdoc/>
        public bool CenterOfZero { get; private set; }

        ///<inheritdoc/>
        public bool InsideZero { get; private set; }

        ///<inheritdoc/>
        public bool ZeroRequired { get; private set; }

        ///<inheritdoc/>
        public int ScaleRange { get; private set; }

        ///<inheritdoc/>
        public bool LegalForTrade { get; private set; }

        ///<inheritdoc/>
        public bool Underload { get; private set; }

        ///<inheritdoc/>
        public bool Overload { get; private set; }

        ///<inheritdoc/>
        public bool HigherSafeLoadLimit { get; private set; }

        ///<inheritdoc/>
        public bool GeneralScaleError { get; private set; }

        ///<inheritdoc/>
        public bool ScaleAlarm { get; private set; }
        #endregion

        #region =============== protected & private methods ================
        ///<inheritdoc/>
        private string UnitIDToString(int id)
        {
            switch (id)
            {
                case 0x00020000:
                    return "kg";
                case 0x004B0000:
                    return "g";
                case 0x004C0000:
                    return "t";
                case 0X00A60000:
                    return "lb";
                case 0x00210000:
                    return "N";
                default:
                    return "";
            }
        }

        ///<inheritdoc/>
        private TareMode EvaluateTareMode(int tare, int presettare)
        {
            if (tare > 0)
                if (presettare > 0)
                    return TareMode.PresetTare;
                else
                    return TareMode.Tare;
            else
                return TareMode.None;
        } 
        #endregion

    }
}
