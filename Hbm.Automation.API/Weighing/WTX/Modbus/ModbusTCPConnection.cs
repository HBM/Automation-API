// <copyright file="ModbusTCPConnection.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
namespace Hbm.Automation.Api.Weighing.WTX.Modbus
{
    using NModbus;
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This class holds a connection via Modbus/TCP,  starts/ends the connection, reads/writes and buffers data.
    /// </summary>
    public class ModbusTCPConnection : INetConnection
    {
        #region ==================== constants & fields ====================
        private const int MODBUS_TCP_PORT = 502;
        private const int WTX_SLAVE_ADDRESS = 0;
        private const int WTX_REGISTER_START_ADDRESS = 0;
        private const int WTX_REGISTER_DATAWORD_COUNT = 38;
        private const int WTX_REGISTER_EXECUTION_COMMANDS = 0;

        private IModbusMaster _master;
        private TcpClient _client;   
        #endregion

        #region ==================== events & delegates ====================
        public event EventHandler<LogEventArgs> CommunicationLog;
        public event EventHandler<EventArgs> UpdateData;
        #endregion
                     
        #region =============== constructors & destructors =================
        public ModbusTCPConnection(string ipAddress)
        {
            IpAddress = ipAddress;            
            CreateDictionary();
        }
        #endregion

        #region ======================== properties ========================
        ///<inheritdoc/>
        public bool IsConnected { get; private set; }

        ///<inheritdoc/>
        public ConnectionType ConnectionType
        {
            get { return ConnectionType.Modbus; }
        }

        ///<inheritdoc/>
        public string IpAddress { get; set; }

        ///<inheritdoc/>
        public ushort[] AllData { get; private set; }
        #endregion

        #region ================ public & internal methods =================
        
        /// <summary>
        /// Connect a Modbus/TCP device
        /// </summary>
        public void Connect(int timeoutMs = 20000)
        {
            try
            {
                _client = new TcpClient(IpAddress, MODBUS_TCP_PORT);
                var factory = new ModbusFactory();
                _master = factory.CreateMaster(_client);
                IsConnected = true;
                CommunicationLog?.Invoke(this, new LogEventArgs("Connection successful"));
            }
            catch (Exception)
            {
                IsConnected = false;
                CommunicationLog?.Invoke(this, new LogEventArgs("Connection failed"));
            }
        }
     
        /// <summary>
        /// Closes the Modbus/TCP connection
        /// </summary>
        public void Disconnect()
        {
            if (_client!=null)
                _client.Close();
            IsConnected = false;
            CommunicationLog?.Invoke(this, new LogEventArgs("Disconnected"));
        }

        /// <summary>
        /// Synchronizes the Modbus data 
        /// </summary>
        /// <returns>Synchronized Modbus registers</returns>
        public ushort[] SyncData()
        {
            ushort[] _data = ReadModbusRegisters();
            this.UpdateData?.Invoke(this, new EventArgs());
            return _data;
        }

        /// <summary>
        /// Reads a single Modbus/TCP register 
        /// </summary>
        /// <param name="command">Modbus/TCP register index for holding register</param>
        /// <returns>Register content</returns>
        public string Read(object command)
        {
            int _value = 0;
            ModbusCommand _command = (ModbusCommand)command;
            ushort[]_data = ReadModbusRegisters();
            _value = _command.ToValue(_data);
            return _value.ToString();
        }

        /// <summary>
        /// Reads a single Modbus/TCP register 
        /// </summary>
        /// <param name="command">Modbus/TCP register index for holding register</param>
        /// <returns>Register content</returns>
        public int ReadInteger(object command)
        {
            int _value = 0;
            ModbusCommand _command = (ModbusCommand)command;
            ushort[] _data = ReadModbusRegisters();
            _value = _command.ToValue(_data);
            return _value;
        }

        ///<inheritdoc/>
        public async Task<string> ReadAsync(object command)
        {
            int _value = 0;
            ModbusCommand _command = (ModbusCommand)command;
            ushort[] _data = await ReadModbusRegistersAsync();
            _value = _command.ToValue(_data);
            this.UpdateData?.Invoke(this, new EventArgs());
            return _value.ToString();
        }

        public bool Write(object command, string value)
        {
            return this.WriteInteger(command, Convert.ToInt32(value));
        }

        ///<inheritdoc/>
        public bool WriteInteger(object command, int value)
        {
            bool result = true;

            ModbusCommand _command = (ModbusCommand)command;
            ushort[] _dataToWrite = new ushort[2];

            switch (_command.DataType)
            {
                case DataType.U32:
                case DataType.S32:
                    _dataToWrite[0] = (ushort)((value & 0xffff0000) >> 16);
                    _dataToWrite[1] = (ushort)(value & 0x0000ffff);
                    _master.WriteMultipleRegisters(WTX_SLAVE_ADDRESS, _command.Register, _dataToWrite);
                    break;

                case DataType.BIT:
                    _master.WriteSingleRegister(WTX_SLAVE_ADDRESS, _command.Register, (ushort)(value << _command.BitIndex));
                    break;
                case DataType.U08:
                case DataType.S16:
                case DataType.U16:
                default:
                    _master.WriteSingleRegister(WTX_SLAVE_ADDRESS, _command.Register, (ushort)value);
                    break;
            }

            if (_command.Register == WTX_REGISTER_EXECUTION_COMMANDS)
            {
                if (DoHandshake())
                {
                    CommunicationLog?.Invoke(this, new LogEventArgs("Write register " + _command.Register + " to " + value.ToString() + "successful"));
                }
                else
                {
                    CommunicationLog?.Invoke(this, new LogEventArgs("Write register " + _command.Register + " to " + value.ToString() + "error"));
                    result = false;
                }
            }
            else
                CommunicationLog?.Invoke(this, new LogEventArgs("Write register " + _command.Register + " to " + value.ToString()));

            return result;
        }

        ///<inheritdoc/>
        public async Task<int> WriteAsync(object command, int value)
        {
            ModbusCommand _command = (ModbusCommand)command;

            ushort registerAddress = (ushort)Convert.ToInt16(_command.Register);

            await _master.WriteSingleRegisterAsync(0, registerAddress, (ushort)(value << _command.BitIndex));

            CommunicationLog?.Invoke(this, new LogEventArgs("Write register " + _command.Register + " to " + value.ToString()));

            return value;
        }
        #endregion

        #region =============== protected & private methods ================
        /// This method writes a data word to the WTX120 device synchronously. 
        private bool DoHandshake()
        {
            while (ReadInteger(ModbusCommands.PLCComHandshake) == 0)
            {
                Thread.Sleep(50);
            }
           _master.WriteSingleRegister(WTX_SLAVE_ADDRESS, WTX_REGISTER_EXECUTION_COMMANDS, 0x00);

            while (ReadInteger(ModbusCommands.PLCComHandshake) == 1) 
            {
                Thread.Sleep(50);
            }
            return (ReadInteger(ModbusCommands.PLCComStatus) == 1);
        }
                
        public string ReadFromBuffer(object command)
        {
            ModbusCommand modBusCommand = (ModbusCommand)command;
            return (modBusCommand.ToValue(AllData)).ToString();
        }

        public string ReadFromDevice(object command)
        {
            return "";
        }

        ///<inheritdoc/>
        public int ReadIntegerFromBuffer(object command)
        {
            ModbusCommand modbuscommand = (ModbusCommand)command;
            return modbuscommand.ToValue(AllData);
        }

        private void CreateDictionary()
        {
            AllData = new ushort[WTX_REGISTER_DATAWORD_COUNT];
        }
        
        private async Task<ushort[]> ReadModbusRegistersAsync()
        {
            AllData = await _master.ReadHoldingRegistersAsync(WTX_SLAVE_ADDRESS, WTX_REGISTER_START_ADDRESS, WTX_REGISTER_DATAWORD_COUNT);
            CommunicationLog?.Invoke(this, new LogEventArgs("Read all: " + string.Join(",", AllData.Select(x => x.ToString("X")).ToArray())));
            return AllData;
        }

        private ushort[] ReadModbusRegisters()
        {
            AllData = _master.ReadHoldingRegisters(WTX_SLAVE_ADDRESS, WTX_REGISTER_START_ADDRESS, WTX_REGISTER_DATAWORD_COUNT);
            CommunicationLog?.Invoke(this, new LogEventArgs("Read all: " + string.Join(",", AllData.Select(x => x.ToString("X")).ToArray())));
            return AllData;
        }
        #endregion
    }
}

   