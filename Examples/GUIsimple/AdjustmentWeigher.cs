// <copyright file="AdjustmentWeigher.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.Examples.GUISimple
{
    using Hbm.Automation.Api.Weighing;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// This class provides a window to calibrate the WTX with a calibration weight.
    /// First the dead load is set and second the calibration weight is set by the application.
    /// You can step back with button2.
    /// </summary>
    public partial class AdjustmentWeigher : Form
    {

        #region ==================== constants & fields ==================== 

        private BaseWTDevice _wtxDevice;
        private int _state = 0;
        
        private double _calibrationWeight = 0.0;
        private int _wtxDeviceDecimals = 1;      
        private string _calibrationWeightWithComma;

        #endregion

        #region =============== constructors & destructors =================

        /// <summary>
        /// Constructor of class WeightCalibration
        /// </summary>
        /// <param name="wtxDevice"></param>
        public AdjustmentWeigher(BaseWTDevice wtxDevice)
        {
            this._wtxDevice = wtxDevice;

            InitializeComponent();

            if (!wtxDevice.IsConnected)
            {
                txtCalibrationWeight.Enabled = false;
                cmdAdjust.Enabled = false;
                cmdCancel.Text = "Close";
                txtInfo.Text = "No WTX connected!";
            }

            // Get some settings from the connected Device
            _wtxDeviceDecimals = wtxDevice.ProcessData.Decimals;

            lblUnit.Text = wtxDevice.Unit;

            txtInfo.Text = "Enter a calibration weight";
        }

        #endregion

        #region ================ public & internal methods =================

        /// <summary>
        /// Finite state machine to go through the calibration process having 3 states 
        /// State 0 : Start
        /// State 1 : Adjust/set zero 
        /// State 2 : Do calibration/Write calibration weight to WTX
        /// </summary>
        private void AdjustmentStateMachine()
        {

            //Switch depending on the current calibration step described by State
            switch (_state)
            {
                case 0: //start

                    try
                    {
                        _calibrationWeightWithComma = txtCalibrationWeight.Text.Replace(".", ",");  // Accept comma and dot
                        _calibrationWeight = double.Parse(_calibrationWeightWithComma);
                        txtCalibrationWeight.Enabled = false;
                        txtInfo.Text = _calibrationWeight.ToString();

                    }
                    catch (FormatException)
                    {
                        txtInfo.Text = "Wrong format!" + Environment.NewLine + "Accepted format *,***";
                        break;
                    }
                    catch (OverflowException)
                    {
                        txtInfo.Text = "Overflow!";
                        break;
                    }
                    
                    txtInfo.Text = "Unload Scale!";
                    cmdAdjust.Text = "Adjust Zero";
                    cmdCancel.Text = "<Back";
                    _state = 1;
                    break;

                case 1: // Adjust/set zero

                    txtInfo.Text = "Adjusting zero in progess.";
                    Application.DoEvents(); 

                    _wtxDevice.AdjustZeroSignal();
                    
                    txtInfo.Text = "Zero load measured." + Environment.NewLine + "Put weight on scale.";
                    cmdAdjust.Text = "Calibrate";
                    _state = 2;

                    break;

                case 2: // Do calibration/Write calibration weight to WTX 

                    txtInfo.Text = "Calibration in progress.";
                    Application.DoEvents();            

                    if (_wtxDevice.AdjustNominalSignalWithCalibrationWeight(_calibrationWeight))
                        txtInfo.Text = "Calibration finished succesfully";
                    else
                        txtInfo.Text = "Calibration finished incomplete";
                    cmdAdjust.Text = "Close";
                    _state = 3;

                    break;

                default: //close window

                    _state = 0;
                    Close();
                    break;
            }
        }

        #endregion

        #region ==================== events & delegates ====================

        /// <summary>
        /// Calls the correct method depending on the current calibration state "State" and 
        /// adapts respectively the text on the button "Start".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAdjust_Click(object sender, EventArgs e)
        {
            cmdAdjust.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            AdjustmentStateMachine();

            cmdAdjust.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Limits the input of the textbox to digits, ',' and '.'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCakibrationWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == ',')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Choose the action of the cancel/back button, depending on the current
        /// calibration state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            switch (_state)
            {
                case 0:
                    Close();
                    break;
                case 1:
                    _state = 0;
                    cmdCancel.Text = "Cancel";
                    txtCalibrationWeight.Enabled = true;
                    cmdAdjust.Text = "Start";
                    txtInfo.Text = "";
                    break;
                case 2:
                    _state = 1;
                    txtInfo.Text = "Unload Scale!";
                    cmdAdjust.Text = "Adjust Zero";
                    break;
                default:
                    _state = 2;
                    txtInfo.Text = "Zero load measured." + Environment.NewLine + "Put weight on scale.";
                    cmdAdjust.Text = "Calibrate";
                    break;
            }
        }

        private void AdjustmentWeigher_Load(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
