// <copyright file="Enums.cs" company="Hottinger Baldwin Messtechnik GmbH">
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
    /// <summary>
    /// Identify a connection
    /// </summary>
    public enum ConnectionType
    {
        Modbus = 0,
        Jetbus = 1,
        DSEJet = 2
    }

    /// <summary>
    /// IO type
    /// </summary>
    public enum IOType
    {
        Input = 0,
        Output = 1
    }

    /// <summary>
    /// Data types for the device data
    /// </summary>
    public enum DataType
    {
        NIL,
        BIT,
        S08,
        U08,
        S16,
        U16,
        S32,
        U32,
        ASCII
    }

    /// <summary>
    /// Digital input functions
    /// </summary>
    public enum InputFunction
    {
        Off,
        Tare,
        Trigger,        // Only ApplicationMode.Checkweigher
        BreakFilling,   // Only ApplicationMode.Filler
        RunFilling,     // Only ApplicationMode.Filler
        Redosing,       // Only ApplicationMode.Filler
        RecordWeight,
        Zero,
        Sum             // Only ApplicationMode.Count
    }

    /// <summary>
    /// Digital output functions
    /// </summary>
    public enum OutputFunction
    {
        Off,
        Manually,     
        LimitSwitch1,               // Only ApplicationMode.Standard
        LimitSwitch2,               // Only ApplicationMode.Standard
        LimitSwitch3,               // Only ApplicationMode.Standard
        LimitSwitch4,               // Only ApplicationMode.Standard
        StopMaterial,               // Only ApplicationMode.Filler
        CoarseFlow,                 // Only ApplicationMode.Filler
        FineFlow,                   // Only ApplicationMode.Filler
        Ready,                      // Only ApplicationMode.Filler
        ToleranceExceeded,          // Only ApplicationMode.Filler
        ToleranceUnderrun,          // Only ApplicationMode.Filler
        ToleranceExceededUnderrun,  // Only ApplicationMode.Filler
        Alert,                      // Only ApplicationMode.Filler
        DL1DL2,                     // Only ApplicationMode.Filler
        Empty,
        DeviceStatus
    }

    /// <summary>
    /// Application mode
    /// </summary>
    public enum ApplicationMode
    {
        Standard = 0,
        Checkweigher = 1,
        Filler = 2
    }

    /// <summary>
    /// Modes for limit switches
    /// </summary>
    public enum LimitSwitchMode
    {
        AboveLevel = 0,
        BelowLevel = 1,
        InsideBand = 2,
        OutsideBand = 3
    }

    /// <summary>
    /// Sources for limit switches
    /// </summary>
    public enum LimitSwitchSource
    {
        Net = 0,
        Gross = 1
    }

    /// <summary>
    /// Tare mode
    /// </summary>
    public enum TareMode
    {
        None,
        Tare,
        PresetTare
    }

    /// <summary>
    /// Multi range/interval scale type
    /// </summary>
    public enum ScaleRangeMode
    {
        None,
        MultiRange,
        MultiInterval
    }

    public enum LowPassFilter
    {
        No_Filter,
        IIR_Filter,
        FIR_Filter,     
        Standard,
        Critically_damped,
        Bessel,
        Butterworth
    }

    public enum FilterTypes
    {
        No_Filter = 0,
        FIR_Comb_Filter = 13089,
        FIR_Moving_Average = 13105
    }

}