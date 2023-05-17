/* @@@@ HOTTINGER BALDWIN MESSTECHNIK - DARMSTADT @@@@@
 * 
 * TCP/MODBUS Interface for WTX120_Modbus | 02/2018
 * 
 * Author : Felix Retsch 
 * 
 *  */

namespace Hbm.Automation.Api.Weighing.Examples.GUISimple.NETStandard
{
    using Hbm.Automation.Api.Weighing;
    using System;
    using System.Globalization;
    using System.Windows.Forms;
    /// <summary>
    /// This class provides a window to perform a calibration without a calibration weight,
    /// based on known values for dead load and nominal load in mV/V
    /// </summary>
    public partial class AdjustmentCalculator : Form
    {
        
        #region ==================== constants & fields ==================== 

        private BaseWTDevice _wtxDevice;

        private bool _finished;
        private double _preload;
        private double _capacity;
        private IFormatProvider _provider;
        private const double MULTIPLIER_MV2_D = 500000; //   2 / 1000000; // 2mV/V correspond 1 million digits (d)
        private string _strCommaDot;

        #endregion

        #region =============== constructors & destructors =================

        /// <summary>
        /// Constructor of class 'CalcCalibration' : Initialze values
        /// </summary>
        /// <param name="wtxDevice"></param>
        public AdjustmentCalculator(BaseWTDevice wtxDevice)
        { 
            this._wtxDevice = wtxDevice;

            _finished = false;
            //Provider for english number format
            _provider = CultureInfo.InvariantCulture;

            _strCommaDot = "";

            InitializeComponent();

            if (!wtxDevice.IsConnected)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                buttonCalculate.Text = "Close";
                _finished = true;
                label5.Visible = true;
                label5.Text = "No WTX connected!";
            }
        }

        #endregion

        #region ==================== events & delegates ====================

        /// <summary>
        /// Checks the input values of the textboxes and start the calibration calculation.
        /// If the caluclation is finished, the window can be closed with the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (!_finished)
            {
                label5.Visible = true;
                bool abort = false;
                try
                {
                    _strCommaDot = textBox1.Text.Replace(".", ",");          
                    _preload = double.Parse(_strCommaDot);
                    textBox1.Enabled = false;
                }
                catch (FormatException)
                {
                    label5.Text = "wrong number format";
                    abort = true;
                }
                catch (OverflowException)
                {
                    label5.Text = "Overflow! Number to big.";
                    abort = true;
                }

                try
                {
                    //Capacity = Convert.ToDouble(textBox2.Text, Provider);
                    _strCommaDot = textBox2.Text.Replace(".", ",");                   
                    _capacity = double.Parse(_strCommaDot);
                    textBox2.Enabled = false;
                }
                catch (FormatException)
                {
                    label5.Text = "wrong number format";
                    abort = true;
                }
                catch (OverflowException)
                {
                    label5.Text = "Overflow! Number to big.";
                    abort = true;
                }
                if (abort) return;
                
                _wtxDevice.CalculateAdjustment(_preload,_capacity);

                label5.Text = "Calibration Successful!";
                _finished = true;
                buttonCalculate.Text = "Close";
            }
            else
            {

                Close();
            }
        }

        /// <summary>
        /// Limits the input of the textbox 1/2 to digits, ',' and '.'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
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

        #endregion

    }
}