// <copyright file="ModbusCommand.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// <summary>
    /// Class to define the frame specifing data type, register, bit offset(for bit addressing) of the data to be read or write via the Modbus/TCP interface. 
    /// A command consists of a datatype, register, input or output type, application mode type bit index, bit length.
    /// </summary>
    public class ModbusCommand
    {
        #region =============== constructors & destructors =================
        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusCommand" /> class
        /// </summary>
        /// <param name="dataType">Provide the data type</param>
        /// <param name="register">Register number of the modbus register</param>
        /// <param name="io">IO type</param>
        /// <param name="appMode">Application mode</param>
        /// <param name="bitIndex">Bit index for a flag</param>
        /// <param name="bitLength">Bit length for a multi-bit flag</param>
        public ModbusCommand(DataType dataType, ushort register, IOType io, ApplicationMode appMode, int bitIndex, int bitLength)
        {
            this.DataType  = dataType;
            this.Register  = register;
            this.IO  = io;
            this.App = appMode;
            this.BitIndex  = bitIndex;
            this.BitLength = bitLength;

            this.Path = register.ToString() + dataType + appMode + io + bitIndex + bitLength;
        }
        #endregion
        
        #region ======================== properties ========================
        /// <summary>
        /// Gets the data type 
        /// </summary>
        public DataType DataType { get; private set; }

        /// <summary>
        /// Gets the bit index for flags
        /// </summary>
        public int BitIndex { get; private set; }

        /// <summary>
        /// Gets the bit length for multi-bit flags 
        /// </summary>
        public int BitLength { get; private set; }

        /// <summary>
        /// Gets the overall path for unique command identification 
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the number of the Modbus register
        /// </summary>
        public ushort Register { get; private set; }

        /// <summary>
        /// Gets a Input or output register
        /// </summary>
        public IOType IO { get; private set; }

        /// <summary>
        /// Gets the application mode
        /// </summary>
        public ApplicationMode App { get; private set; }
        #endregion

        #region ================ public & internal methods =================
        /// <summary>
        /// Picks the command from all registers 
        /// </summary>
        /// <param name="allRegisters">All available holding register starting from index 0</param>
        /// <returns>Value as integer</returns>
        public int ToValue(ushort[] allRegisters)
        {
            ushort _bitMask = 0;
            ushort _mask = 0;
            int _value;

            try
            {
                switch (DataType)
                {
                    case DataType.BIT:
                        {
                            switch (BitLength)
                            {
                                case 0:
                                    _bitMask = 0xFFFF;
                                    break;
                                case 1:
                                    _bitMask = 1;
                                    break;
                                case 2:
                                    _bitMask = 3;
                                    break;
                                case 3:
                                    _bitMask = 7;
                                    break;
                                default:
                                    _bitMask = 1;
                                    break;
                            }

                            _mask = (ushort)(_bitMask << BitIndex);
                            _value = (allRegisters[Register] & _mask) >> BitIndex;
                            break;
                        }

                    case DataType.U32:
                    case DataType.S32:
                        _value = (allRegisters[Register] >> 16) + allRegisters[Register + 1];
                        break;

                    case DataType.S16:
                    case DataType.U16:
                    case DataType.U08:
                    default:
                        _value = allRegisters[Register];
                        break;
                }
            }
            catch
            {
                _value = 0;
            }

            return _value;
        }
        #endregion

        #region =============== protected & private methods ================
        private int ExtractBit(int input)
        {
            int _bitMask = 0;
            int _mask = 0;

            switch (BitLength)
            {
                case 0:
                    _bitMask = 0xFFFF;
                    break;
                case 1:
                    _bitMask = 1;
                    break;
                case 2:
                    _bitMask = 3;
                    break;
                case 3:
                    _bitMask = 7;
                    break;
                default:
                    _bitMask = 1;
                    break;
            }
            _mask = _bitMask << BitIndex;
            return (input & _mask) >> BitIndex;
        }
        #endregion
    }
}
