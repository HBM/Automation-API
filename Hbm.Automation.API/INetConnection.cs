// <copyright file="INetConnection.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Connection interfaces for HBM weighing devices 
    /// </summary>
    public interface INetConnection
    {
        #region ==================== events & delegates ====================      
        /// <summary>
        /// Event handler for communication log messages
        /// Receive log messages in LogEventArgs.Args
        /// </summary>
        event EventHandler<LogEventArgs> CommunicationLog;
        
        /// <summary>
        /// Event handler called whenever new data is received
        /// </summary>
        event EventHandler<EventArgs> UpdateData;
        #endregion

        #region ======================== properties ========================
        /// <summary>
        /// Gets or sets the ip address of the weighing device
        /// </summary>
        string IpAddress { get; set; }
      
        /// <summary>
        /// Gets the connection type (e.g. JetBus or Modbus)
        /// </summary>
        ConnectionType ConnectionType { get; }

        /// <summary>
        /// Gets a value indicating whether teh connection is active or not
        /// </summary>
        bool IsConnected { get; }
        #endregion

        #region ================ public & internal methods =================
        /// <summary>
        /// Opens a new connection
        /// </summary>
        /// <param name="timeoutMs">Timeout for setting up the connection</param>
        void Connect(int timeoutMs = 20000);
        
        /// <summary>
        /// Closes the connection
        /// </summary>
        void Disconnect();
        #endregion

        #region ==================== Read/Write methods ====================
        /// <summary>
        /// Read data from a device.
        /// </summary>
        /// <param name="command">Variable command type, either ModbusCommand or JetbusCommand</param>
        /// <returns></returns>
        string Read(object command);

        /// <summary>
        /// Write data to a device.
        /// </summary>
        /// <param name="command">Variable command type, either ModbusCommand or JetbusCommand</param>
        bool WriteInteger(object command, int value);

        /// <summary>
        /// Write data to a device.
        /// </summary>
        /// <param name="command">Variable command type, either ModbusCommand or JetbusCommand</param>
        bool Write(object command, string value);

        /// <summary>
        /// Read data asynchonously from a device.
        /// </summary>
        /// <param name="command">Variable command type, either ModbusCommand or JetbusCommand</param>
        Task<string> ReadAsync(object command);

        /// <summary>
        /// Write data asynchonously to a device.
        /// </summary>
        /// <param name="command">Variable command type, either ModbusCommand or JetbusCommand</param>
        Task<int> WriteAsync(object command, int value);

        /// <summary>
        /// Read data from a local buffer.
        /// </summary>
        /// <param name="command">Variable command type, either ModbusCommand or JetbusCommand</param>
        string ReadFromBuffer(object command);

        /// <summary>
        /// Read data from the device without permanently fetching the command (DSE).
        /// </summary>
        /// <param name="command">Variable command type, either ModbusCommand or JetbusCommand</param>
        string ReadFromDevice(object command);

        int ReadIntegerFromBuffer(object command);
        #endregion

    }

}
