// <copyright file="Program.cs" company="Hottinger Baldwin Messtechnik GmbH">
//
// Example application for Hbm.Automation.Api, a library to communicate with HBM weighing technology devices  
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

namespace WTXModbus
{
    using System;
    using System.Threading;
    using System.Globalization;
    using Hbm.Automation.Api;
    using Hbm.Automation.Api.Data;
    using Hbm.Automation.Api.Weighing.WTX;
    using Hbm.Automation.Api.Weighing.WTX.Jet;
    using Hbm.Automation.Api.Weighing.WTX.Modbus;
    using Hbm.Automation.Api.Weighing;

    /// <summary>
    /// This class implements a console application. An Object of the class 'ModbusTcpConnection' or 'JetBusConnection' and 'BaseWTDevice'('WTXJet' or WTXModbus') or 'ExtendedBaseWtDevice'-
    /// are initialized as a publisher and subscriber. Afterwards a connection to the device is established and the timer/sending interval is set. 
    /// A timer with for example 200ms is created. After the timer interval an event is triggered, which executes the method "Update" reading the register of the device
    /// by an asynchronous call. 
    /// The data is printed on the console in the callback method "Update". 
    /// You can select commands to the device. For example taring, change from gross to net value, stop dosing, zeroing and so on. 
    /// 
    /// This is overall just a simple application as an example, but for starters it is recommended. 
    /// </summary>

    static class Program
    {

        #region ==================== constants & fields ==================== 
        private const string DEFAULT_IP_ADDRESS = "192.168.100.88";

        private static ConnectionType connectiontype;
        private const string MESSAGE_CONNECTION_FAILED = "Connection failed!";
        private const string MESSAGE_CONNECTING = "Connecting...";

        private const int WAIT_DISCONNECT = 2000; 
        
        private static BaseWTDevice _wtxDevice;
        
        private static string _ipAddress = DEFAULT_IP_ADDRESS ;     // IP-adress, set as the first argument in the VS project properties menu as an argument or in the console application as an input(which is commented out in the following) 
        private static int _timerInterval;                          // timer interval, set as the second argument in the VS project properties menu as an argument or in the console application as an input(which is commented out in the following) 
         
        private static ConsoleKeyInfo _valueOutputwords;
        private static ConsoleKeyInfo _valueExitapplication;

        private static string _calibrationWeight = "0";
        
        private static double _preload, _capacity;
        private static IFormatProvider _provider;

        private const double MULTIPLIER_MV2_D = 500000;      //   2 / 1000000; // 2mV/V correspond 1 million digits (d)
        private static string _strCommaDot ="";

        #endregion

        #region ================ public & internal methods =================
        static void Main(string[] args)
        {
            // Input for the ip adress, the timer interval and the input mode: 

            _timerInterval = 200;

            DefineInputs(args);

            // Initialize:

            _provider = CultureInfo.InvariantCulture;

            Console.WriteLine("\nTCPModbus Interface for weighting terminals of HBM\nEnter e to exit the application");

            do // do-while loop for the connection establishment. If the connection is established successfully, the do-while loop is left/exit. 
            {
                InitializeConnection();

            } while (_wtxDevice.Connection.IsConnected==false);


            MenuCases();

            //Get a single value from the WTX device : 
            double x = _wtxDevice.ProcessData.Weight.Net; 

        } // end main  

        private static void DefineInputs(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "Jetbus" || args[0] == "JetBus" || args[0] == "jetbus" || args[0] == "jetBus" || args[0] == "Jet" || args[0] == "jet")
                    connectiontype = ConnectionType.Jetbus;

                if (args[0] == "Modbus" || args[0] == "ModBus" || args[0] == "modbus" || args[0] == "modBus")
                    connectiontype = ConnectionType.Modbus;
            }
            else
            {
                connectiontype = ConnectionType.Jetbus;
            }

            if (args.Length > 1)
                _ipAddress = args[1];
            else
            {
                Console.WriteLine(" WTX - For connection establishment : No IP adress is given. \n please enter the IP adress to the WTX device, format : aaa.bbb.ccc.dd");
                _ipAddress = Console.ReadLine();
            }
            if (args.Length > 2)
                _timerInterval = Convert.ToInt32(args[2]);
            else
                _timerInterval = 200;

        }

        // This method connects to the given IP address
        private static void InitializeConnection()
        {
            if (connectiontype == ConnectionType.Modbus)    // If 'Modbus/Tcp' is selected: 
            {
                // Creating objects of ModbusTcpConnection and WTXModbus: 
                ModbusTCPConnection _modbusConnection = new ModbusTCPConnection(_ipAddress);

                _wtxDevice = new WTXModbus(_modbusConnection, _timerInterval, Update);
            }
            else
            {
                if (connectiontype == ConnectionType.Jetbus)  // If 'JetBus' is selected: 
                {
                    // Creating objects of JetBusConnection and WTXJet: 
                    JetBusConnection _jetConnection = new JetBusConnection(_ipAddress, "Administrator", "wtx");

                    _wtxDevice = new WTXJet(_jetConnection, _timerInterval, Update);
                }
            }

            // Connection establishment via Modbus or Jetbus :  
            try
            {
                _wtxDevice.Connect(5000);
            }
            catch (Exception)
            {
                Console.WriteLine(MESSAGE_CONNECTION_FAILED);
            }

            if (_wtxDevice.Connection.IsConnected == true)
            {
                //Update(null, null);  // call of callback method to print values on console, even though the measured values did not change. 

                Console.WriteLine("\nThe connection has been established successfully.\nThe values of the WTX device are printed on the console ... :");

                CommandLine.Properties.Settings.Default.IPaddress = _ipAddress;
                CommandLine.Properties.Settings.Default.Save();
            }
            else
            {
                Console.WriteLine(MESSAGE_CONNECTION_FAILED);
                Console.WriteLine("\nFailure : The connection has not been established successfully.\nPlease enter a correct IP Adress for the connection establishment...");               
                _ipAddress = Console.ReadLine();
            }

        } // End method Connect() 

        private static void MenuCases()
        {
            // This while loop is repeated till the user enters 'e' (=e meaning exit). After the timer interval the register of the device is read out. 
            // In the while-loop the user can select commands, which are send immediately to the device. 
            while (_valueExitapplication.KeyChar != 'e')
            {
                //_isCalibrating = false;
                _valueOutputwords = Console.ReadKey();
                int valueOutput = Convert.ToInt32(_valueOutputwords.KeyChar);

                switch (_valueOutputwords.KeyChar)
                {
                    case '0': _wtxDevice.Tare(); break;                      // Taring 
                    case '1': _wtxDevice.SetGross(); break;                  // Gross/Net
                    case '2': _wtxDevice.Zero(); break;                      // Zeroing
                    case '3': _wtxDevice.AdjustZeroSignal(); break;          // Adjust zero 
                    case '4': _wtxDevice.AdjustNominalSignal(); break;       // Adjust nominal
                    case '5': _wtxDevice.RecordWeight(); break;              // Record Weight

                    // 'c' for writing on multiple registers, which is necessary for the calibration. 
                    case 'c':       // Calculate Calibration
                        CalculateCalibration();
                        break;
                    case 'w':       // Calculation with weight 
                        CalibrationWithWeight();
                        break;
                    // Change connection from Jetbus to Modbus or from Modbus to Jetbus: 
                    case 'j':             
                        if (_wtxDevice != null)    // Necessary to check if the object of BaseWtDevice have been created and a connection exists. 
                        {
                            _wtxDevice.Connection.Disconnect();
                            _wtxDevice = null;
                        }

                        Thread.Sleep(WAIT_DISCONNECT);     // Wait for 2 seconds till the disconnection request is finished. 

                        if (connectiontype == ConnectionType.Modbus)
                            connectiontype = ConnectionType.Jetbus;
                        else
                             if (connectiontype == ConnectionType.Jetbus)
                                connectiontype = ConnectionType.Modbus;

                        InitializeConnection();
                        break;

                    default: break;

                }   // end switch-case

                _valueExitapplication = Console.ReadKey();
                if (_valueExitapplication.KeyChar == 'e')
                    break;

            }// end while

        } // end method MenuCases() 

        /*
         * This method calcutes the values for a dead load and a nominal load(span) in a ratio in mV/V and write in into the WTX registers. 
         */
      
        private static void CalculateCalibration()
        {
            _wtxDevice.Stop();

            zero_load_nominal_load_input();
           
            _wtxDevice.CalculateAdjustment(_preload,_capacity);          
        }

        /*
         * This method does a calibration with an individual weight to the WTX.  
         * First you tip the value for the calibration weight, then you set the value for the dead load (method ‚MeasureZero‘), 
         * finally you set the value for the nominal weight in the WTX (method ‚Calibrate(calibrationValue)‘).
         */
        private static void CalibrationWithWeight()
        {
            _wtxDevice.Stop();

            Console.Clear();
            Console.WriteLine("\nPlease tip the value for the calibration weight and tip enter to confirm : ");
            _calibrationWeight = Console.ReadLine();

            Console.WriteLine("\nTo start : Set zero load and press any key for measuring zero and wait.");
            string another = Console.ReadLine();

            _wtxDevice.AdjustZeroSignal();
            Console.WriteLine("\nZero load measured.Put weight on scale, press any key and wait.");

            string another2 = Console.ReadLine();

            _wtxDevice.AdjustNominalSignalWithCalibrationWeight(CalibrationWeight());

            _wtxDevice.Restart();
        }  

        /*
         * This method potentiate the number of the values decimals and multiply it with the calibration weight(input) to get
         * an integer which is in written into the WTX registers by the method Calibrate(potencyCalibrationWeight()). 
         */
        private static double CalibrationWeight()
        {
            _strCommaDot = _calibrationWeight.Replace(".", ","); // Transformation into a floating-point number.Thereby commas and dots can be used as input for the calibration weight.                         
            return double.Parse(_strCommaDot);                                                                                                                
        }

        // This method prints the values of the device (as a integer and the interpreted string) as well as the description of each bit. 
        private static void Update(object sender, ProcessDataReceivedEventArgs e)
        {
            // The description and the value of the WTX are only printed on the console if the Interface, containing all auto-properties of the values is 
            // not null (respectively empty) and if no calibration is done at that moment.

            if (_wtxDevice != null && e != null)
            {
                Console.Clear();               

                if (connectiontype == ConnectionType.Modbus) 
                {
                    Console.WriteLine("\n0-Taring | 1-Gross/net   | 2-Zeroing  | 3- Adjust zero    | 4-Adjust nominal |\n5-Record weight|m-Manual redosing | j-Switch connection to Jetbus | \nc-Calculate Calibration | w-Calibration with weight  | e-Exit the application\n");
                }
                else
                {
                    if (connectiontype == ConnectionType.Jetbus)
                        Console.WriteLine("\n0-Taring | 1-Gross/net   | 2-Zeroing  | 3- Adjust zero    | 4-Adjust nominal |\n5-Record weight|m-Manual redosing | j-Switch connection to Modbus | \nc-Calculate Calibration | w-Calibration with weight  | e-Exit the application\n");

                }
                if (_wtxDevice.ApplicationMode == ApplicationMode.Standard)   // If the device is in the standard mode (standard=0 or standard=1; filler=1 or filler=2) 
                {
                    Console.WriteLine("Net value:                     " + _wtxDevice.Weight.Net +  "\t  As an int/bool:  " + e.ProcessData.Weight.Net);
                    Console.WriteLine("Gross value:                   " + _wtxDevice.Weight.Gross + "\t  As an int/bool:  " + e.ProcessData.Weight.Gross);
                    Console.WriteLine("General scale error:          " + e.ProcessData.GeneralScaleError.ToString() +                  "\t  As an int/bool:  " + e.ProcessData.GeneralScaleError);
                    Console.WriteLine("Scale alarm triggered:         " + e.ProcessData.ScaleAlarm.ToString() +                         "\t  As an int/bool:  " + e.ProcessData.ScaleAlarm);
                    Console.WriteLine("Scale seal is open:            " + e.ProcessData.LegalForTrade.ToString() +                     "\t  As an int/bool:  " + e.ProcessData.LegalForTrade);
                    Console.WriteLine("Weight type:                   " + e.ProcessData.TareMode + "\t  As an int/bool:  " + e.ProcessData.TareMode);
                    Console.WriteLine("Scale range:                   " + e.ProcessData.ScaleRange + "\t  As an int/bool:  " + e.ProcessData.ScaleRange);
                    Console.WriteLine("Zero required/True zero:       " + e.ProcessData.ZeroRequired.ToString() +      "\t  As an int/bool:  " + e.ProcessData.ZeroRequired);
                    Console.WriteLine("Weight within center of zero:  " + e.ProcessData.CenterOfZero.ToString() + "\t  As an int/bool:  " + e.ProcessData.CenterOfZero);
                    Console.WriteLine("Weight in zero range:          " + e.ProcessData.InsideZero.ToString() + "\t  As an int/bool:  " + e.ProcessData.InsideZero);
                    Console.WriteLine("Application mode:              " + _wtxDevice.ApplicationMode.ToString() + "\t  As an int/bool:  " + _wtxDevice.ApplicationMode.ToString());
                    Console.WriteLine("Decimal places:                " + e.ProcessData.Decimals.ToString() +          "\t  As an int/bool:  " + e.ProcessData.Decimals);
                    Console.WriteLine("Unit:                          " + _wtxDevice.Unit +                       "\t  As an int/bool:  " + e.ProcessData.Unit);                   
                    Console.WriteLine("Limit status:                  " + limitCommentMethod() + "       As an int/bool:  " + e.ProcessData.Overload);
                    Console.WriteLine("Weight moving:                 " + e.ProcessData.WeightStable.ToString() + "         As an int/bool: " + e.ProcessData.WeightStable);

                }
                else // In Filler application : 
                {
                    Console.WriteLine("Net value:                     " + _wtxDevice.PrintableWeight.Net +   "\t  As an int/bool:  " + e.ProcessData.Weight.Net);
                    Console.WriteLine("Gross value:                   " + _wtxDevice.PrintableWeight.Gross + "\t  As an int/bool:  " + e.ProcessData.Weight.Gross);
                    Console.WriteLine("General scale error:          " + e.ProcessData.GeneralScaleError.ToString() +                   "\t  As an int/bool:  " + e.ProcessData.GeneralScaleError);
                    Console.WriteLine("Scale alarm triggered:         " + e.ProcessData.ScaleAlarm.ToString() +     "\t  As an int/bool:  " + e.ProcessData.ScaleAlarm);
                    Console.WriteLine("Scale seal is open:            " + e.ProcessData.LegalForTrade.ToString() + "\t  As an int/bool:  " + e.ProcessData.LegalForTrade);
                    Console.WriteLine("Weight type:                   " + e.ProcessData.TareMode +               "\t  As an int/bool:  " + e.ProcessData.TareMode);
                    Console.WriteLine("Scale range:                   " + e.ProcessData.ScaleRange +               "\t  As an int/bool:  " + e.ProcessData.ScaleRange);
                    Console.WriteLine("Zero required/True zero:       " + e.ProcessData.ZeroRequired.ToString() +    "\t  As an int/bool:  " + e.ProcessData.ZeroRequired);
                    Console.WriteLine("Weight within center of zero:  " + e.ProcessData.CenterOfZero.ToString() + "\t  As an int/bool:  " + e.ProcessData.CenterOfZero);
                    Console.WriteLine("Weight in zero range:          " + e.ProcessData.InsideZero.ToString() +           "\t  As an int/bool:  " + e.ProcessData.InsideZero);
                    Console.WriteLine("Decimal places:                " + e.ProcessData.Decimals.ToString() +         "\t  As an int/bool:  " + e.ProcessData.Decimals);
                    Console.WriteLine("Unit:                          " + _wtxDevice.Unit +                      "\t  As an int/bool:  " + e.ProcessData.Unit);
                    Console.WriteLine("Limit status:                  " + limitCommentMethod() +  "     As an int/bool:  " + e.ProcessData.Overload);
                    Console.WriteLine("Weight moving:                 " + e.ProcessData.WeightStable.ToString() + "          As an int/bool:  " + e.ProcessData.WeightStable);                
                }
            }
        }

        private static string limitCommentMethod()
        {
            if (_wtxDevice.ProcessData.Overload)
                return "Higher than maximum capacity";
            else if (_wtxDevice.ProcessData.Underload)
                return "Lower than minimum";
            else if (_wtxDevice.ProcessData.HigherSafeLoadLimit)
                return "Higher than safe load limit";
            else
                return "Weight within limits";
        }

        private static void zero_load_nominal_load_input()
        {
            Console.Clear();

            Console.WriteLine("\nPlease tip the value for the zero load/dead load and tip enter to confirm : ");

            string _preloadStr = Console.ReadLine();
            _strCommaDot = _preloadStr.Replace(".", ",");           // For converting into a floating-point number.
            _preload = double.Parse(_strCommaDot);                  // By using the method 'double.Parse(..)' you can use dots and commas.


            Console.WriteLine("\nPlease tip the value for the span/nominal load and tip enter to confirm : ");

            string _capacityStr = Console.ReadLine();
            _strCommaDot = _capacityStr.Replace(".", ",");           // For converting into a floating-point number.
            _capacity = double.Parse(_strCommaDot);                  // By using the method 'double.Parse(..)' you can use dots and commas.

        }
        #endregion

    }
}
