// <copyright file="Settings.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    using System.Windows.Forms;

    // This class implements a windows form to change the specific values of the connection, like
    // IP Adress, number of inputs read out in the register and the sending interval, which
    // is the interval of the timer. 

    partial class SettingsForm : Form
    {
        public EventHandler<SettingsEventArgs> ValuesChanged;

        private string _ipAddressBefore;
        private string _ipAddress;

        private int _sendingInterval; 

        // Constructor of class 'SettingForm': 
        public SettingsForm(string ipAddressParam, int sendingIntervalParam)
        {
            InitializeComponent();
           
            this._ipAddressBefore = ipAddressParam;    // IP_address_before is used to change the IP adress. 
            this._ipAddress = ipAddressParam;
            this._sendingInterval = sendingIntervalParam;

            textBox1.Text = this._ipAddress;
            textBox2.Text = this._sendingInterval.ToString();
            
            label2.Text = "IP address";
            label3.Text = "Read timer interval";
        }
        // This method sets and actualize the attributes of the connection
        // (IP adress, sending/timer interval, number of inputs), if they have changed. 
        private void button2_Click(object sender, EventArgs e)
        {
            this._ipAddress = textBox1.Text;

            this._sendingInterval = Convert.ToInt32(textBox2.Text);

            ValuesChanged.Invoke(this, new SettingsEventArgs(this._ipAddress, this._sendingInterval));

            //Store IPAddress in Settings .settings
            GUIplc.Properties.Settings.Default.IPAddress = this._ipAddress;
            GUIplc.Properties.Settings.Default.Save();

            this.Close();
        }
        public string GetIpAddress
        {
            get
            {
                return this._ipAddress;
            }
        }
        public int GetSendingInterval
        {
            get
            {
                return this._sendingInterval;
            }
        }
        private void Settings_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
