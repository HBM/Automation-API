// <copyright file="GUIsimpleForm.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.Examples.GUIsimple
{
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing;
    using Hbm.Automation.Api.Weighing.DSE;
    using Hbm.Automation.Api.Weighing.DSE.Jet;
    using Hbm.Automation.Api.Weighing.Examples.GUISimple;
    using Hbm.Automation.Api.Weighing.WTX;
    using Hbm.Automation.Api.Weighing.WTX.Jet;
    using Hbm.Automation.Api.Weighing.WTX.Modbus;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// This application example demonstrates the usage of HBM Weighing-API.
    /// It shows how to connect a WTX device via Modbus and Jetbus, how to get weight values, to calibrate and how to adjust the scale.
    /// </summary>
    public partial class GUIsimpleForm : Form
    {
        #region ==================== constants & fields ====================

        private const string DEFAULT_IP_ADDRESS = "192.168.100.88";
        private const string MESSAGE_CONNECTION_FAILED = "Connection failed!";
        private const string MESSAGE_CONNECTING = "Connecting...";
        private const int WAIT_DISCONNECT = 2000;

        private static BaseWTDevice _wtxDevice;
        private AdjustmentCalculator _adjustmentCalculator;
        private AdjustmentWeigher _adjustmentWeigher;
        private string _ipAddress = DEFAULT_IP_ADDRESS;
        private int _timerInterval = 200;

        #endregion

        #region =============== constructors & destructors =================

        /// <summary>
        /// Constructor of GUIsimpleForm
        /// </summary>
        /// <param name="args">Parameter from command line</param>
        public GUIsimpleForm(string[] args)
        {
            InitializeComponent();
            DisplayText("Check IP address, select 'Jet' or 'Modbus/TCP' and press 'Connect'.");
            EvaluateCommandLine(args);
            txtIPAddress.Text = _ipAddress;
            picNE107.Image = GUISimple.Properties.Resources.NE107_DiagnosisPassive;
        }
        #endregion

        #region =============== protected & private methods ================

        /// <summary>
        /// Initialze a Jetbus or Modbus/Tcp connection, creates objects of INetConnection derivations, BaseWtDevice derivations
        /// </summary>
        private void InitializeConnection()
        {
            this._ipAddress = txtIPAddress.Text;

            if (this.cboDeviceType.SelectedIndex == 0)
            {
                // Creating objects of JetBusConnection and WTXJet: 
                JetBusConnection _jetConnection = new JetBusConnection(_ipAddress, "Administrator", "wtx");
                _wtxDevice = new WTXJet(_jetConnection, 500, update);
            }
            else if (this.cboDeviceType.SelectedIndex == 1)
            {
                // Creating objects of ModbusTcpConnection and WTXModbus: 
                ModbusTCPConnection _modbusConnection = new ModbusTCPConnection(this._ipAddress);
                _wtxDevice = new WTXModbus(_modbusConnection, this._timerInterval, this.update);

            }
            else
            {
                // Creating objects of DSEJetConnection: 
                DSEJetConnection _jetConnection = new DSEJetConnection(_ipAddress);
                _wtxDevice = new DSEJet(_jetConnection, 500, update);
            }

            // Connection establishment via Modbus or Jetbus            
            try
            {
                _wtxDevice.Connect(5000);
            }
            catch (Exception)
            {
                DisplayText(MESSAGE_CONNECTION_FAILED);
            }

            if (_wtxDevice.IsConnected)
            {
                picNE107.Image = GUISimple.Properties.Resources.NE107_DiagnosisActive;
                GUISimple.Properties.Settings.Default.IPAddress = this._ipAddress;
                GUISimple.Properties.Settings.Default.DeviceType = cboDeviceType.SelectedIndex;
                GUISimple.Properties.Settings.Default.Save();
            }
            else
            {
                picNE107.Image = GUISimple.Properties.Resources.NE107_DiagnosisPassive;
                DisplayText(MESSAGE_CONNECTION_FAILED);
            }

        }

        /// <summary>
        /// How to get process data automatically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Here you find the current process data (e.g. weight value)</param>
        private void update(object sender, ProcessDataReceivedEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                DisplayText("Net:" + _wtxDevice.PrintableWeight.Net + _wtxDevice.Unit + Environment.NewLine
                + "Gross:" + _wtxDevice.PrintableWeight.Gross + _wtxDevice.Unit + Environment.NewLine
                + "Tara:" + _wtxDevice.PrintableWeight.Tare + _wtxDevice.Unit);

                if (e.ProcessData.Underload == true)
                {
                    DisplayText("Underload : Lower than minimum" + Environment.NewLine);
                    picNE107.Image = GUISimple.Properties.Resources.NE107_OutOfSpecification;

                }
                else if (e.ProcessData.Overload == true)
                {
                    DisplayText("Overload : Higher than maximum capacity" + Environment.NewLine);
                    picNE107.Image = GUISimple.Properties.Resources.NE107_OutOfSpecification;

                }
                else if (e.ProcessData.HigherSafeLoadLimit == true)
                {
                    DisplayText("Higher than safe load limit" + Environment.NewLine);
                    picNE107.Image = GUISimple.Properties.Resources.NE107_OutOfSpecification;
                }
                else
                    picNE107.Image = GUISimple.Properties.Resources.NE107_DiagnosisActive;
            }));
        }

        /// <summary>
        /// Command line control
        /// </summary>
        /// <param name="args">Possible arguments: modbus or jet, ip address</param>
        private void EvaluateCommandLine(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].ToLower() == "jet")
                {
                    cboDeviceType.SelectedIndex = 0;
                }
                else if (args[0].ToLower() == "modbus")
                {
                    cboDeviceType.SelectedIndex = 1;
                }
                else if (args[0].ToLower() == "dse")
                {
                    cboDeviceType.SelectedIndex = 2;
                }
            }
            else
            {
                cboDeviceType.SelectedIndex = GUISimple.Properties.Settings.Default.DeviceType;
            }

            if (args.Length > 1)
                _ipAddress = args[1];
            else
                _ipAddress = GUISimple.Properties.Settings.Default.IPAddress;

            if (args.Length > 2)
                this._timerInterval = Convert.ToInt32(args[2]);
        }

        /// <summary>
        /// Displays the string, the measured values, unit etc. 
        /// </summary>
        /// <param name="text"></param>
        private void DisplayText(string text)
        {
            txtInfo.Text = text;
            Application.DoEvents();
        }

        /// <summary>
        /// Connects to wtx device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (_wtxDevice != null)
            {
                DisplayText("Disconnecting...");
                _wtxDevice.Connection.Disconnect();
                _wtxDevice = null;
            }

            DisplayText("Connecting...");
            this.InitializeConnection();
        }

        /// <summary>
        /// button click event for switching to gross or net value. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdGrossNet_Click(object sender, EventArgs e)
        {
            _wtxDevice.SetGross();
        }

        /// <summary>
        /// button click event for zeroing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdZero_Click(object sender, EventArgs e)
        {
            try
            {
                _wtxDevice.Zero();
            }
            catch (JetBusException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// button click event for taring 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdTare_Click(object sender, EventArgs e)
        {
            _wtxDevice.Tare();
        }

        //Method for calculate adjustment with dead load and span: 
        private void calibrationWithWeightToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (_wtxDevice != null)
            {
                _adjustmentCalculator = new AdjustmentCalculator(_wtxDevice);
                DialogResult res = _adjustmentCalculator.ShowDialog();
            }
        }

        /// <summary>
        /// Adjustment with weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calibrationToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (_wtxDevice != null)
            {
                _adjustmentWeigher = new AdjustmentWeigher(_wtxDevice);
                DialogResult res = _adjustmentWeigher.ShowDialog();
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            string display = "";
            switch (comboBox1.SelectedItem)
            {
                /**
                 *  Serial number
                    Device identification
                    Firmware version
                    Weight step
                    Scale range
                    Tare mode
                    Weight stable
                    Manual tare value
                    Maximum capacity
                    Calibration weight
                    Zero signal
                    Nominal signal
                    Connection
                    Connection type
                    Application mode
                 * **/

                case "Serial number":
                    display = _wtxDevice.SerialNumber;
                    break;
                case "Device identification":
                    display = _wtxDevice.Identification;
                    break;
                case "Firmware version":
                    display = _wtxDevice.FirmwareVersion;
                    break;
                case "Weight step (DSE)":
                    display = ((DSEJet)_wtxDevice).WeightStep.ToString() + " " + _wtxDevice.Unit;
                    break;
                case "Scale range":
                    display = _wtxDevice.ScaleRange.ToString();
                    break;
                case "Tare mode":
                    display = _wtxDevice.TareMode.ToString();
                    break;
                case "Weight stable":
                    display = _wtxDevice.WeightStable.ToString();
                    break;
                case "Manual tare value":
                    display = _wtxDevice.ManualTareValue.ToString() + " " + _wtxDevice.Unit;
                    break;
                case "Maximum capacity":
                    display = _wtxDevice.MaximumCapacity.ToString() + " " + _wtxDevice.Unit;
                    break;
                case "Calibration weight":
                    display = _wtxDevice.CalibrationWeight.ToString() + " " + _wtxDevice.Unit;
                    break;
                case "LDW - Zero signal":
                    display = _wtxDevice.ZeroSignal.ToString() + " nV/V";
                    break;
                case "LWT - Nominal signal":
                    display = _wtxDevice.NominalSignal.ToString() + " nV/V";
                    break;
                case "Connection type":
                    display = _wtxDevice.ConnectionType;
                    break;
                case "Application mode":
                    display = _wtxDevice.ApplicationMode.ToString();
                    break;
                case "Zero value":
                    display = _wtxDevice.ZeroValue.ToString() + " " + _wtxDevice.Unit;
                    break;
            }
            textBox1.Text = display;
        }
    }
}
