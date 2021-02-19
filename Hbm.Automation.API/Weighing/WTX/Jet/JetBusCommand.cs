// <copyright file="JetBusCommand.cs" company="Hottinger Baldwin Messtechnik GmbH">
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

namespace Hbm.Automation.Api.Weighing.WTX.Jet
{
    using System;

    /// <summary>
    /// Class to define the frame specifing data type, path, bit offset(for bit addressing) of the data to be read or write via the jet ethernet interface.
    /// A frame consists of a datatype, path, bit index, bit length.
    /// </summary>
    public class JetBusCommand
    {
        #region =============== constructors & destructors =================
        /// <summary>
        /// Initializes a new instance of the <see cref="JetBusCommand" /> class
        /// </summary>
        /// <param name="dataType">Provide the data type</param>
        /// <param name="path">Register number of the modbus register</param>
        /// <param name="bitIndex">Bit index for a flag</param>
        /// <param name="bitLength">Bit length for a multi-bit flag</param>
        /// public ModbusCommand(DataType dataTy
        public JetBusCommand(DataType dataType, string path, int bitIndex, int bitLength)
        {
            this.DataType  = dataType;
            this.Path = path;
            this.BitIndex  = bitIndex;
            this.BitLength = bitLength;
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
        #endregion

        #region ================ public & internal methods =================
        /// <summary>
        /// Picks the command from all registers 
        /// </summary>
        /// <param name="input">All available holding register starting from index 0</param>
        /// <returns>Value as integer</returns>
        public string ToString(string input)
        {
            string _value;

            try
            {               
                switch (DataType)
                {
                    case DataType.BIT:
                    {
                            _value = ExtractBit(Convert.ToInt32(input)).ToString();
                            break;
                    }

                    default:
                    {
                        _value = input;
                        break;
                    }
                }
            }
            catch
            {
                _value = "0";
            }

            return _value;
        }

        /// <summary>
        /// Picks the command from all registers 
        /// </summary>
        /// <param name="input">All available holding register starting from index 0</param>
        /// <returns>Value as integer</returns>
        public int ToSValue(string input)
        {
            int _value;

            try
            {
                switch (DataType)
                {
                    case DataType.BIT:
                        {
                            _value = ExtractBit(Convert.ToInt32(input));
                            break;
                        }
                    default:
                        {
                            _value = Convert.ToInt32(input);
                            break;
                        }
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
        /// <summary>
        /// Masks and shifts the integer value to get a specific bit according to bit length and bit index
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
