// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2019

using System;
using System.Runtime.InteropServices;

namespace MahjongCVCamera
{
    public static class NativeMethods
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void EnumVideoInputDevicesCallback([MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string name);

        [DllImport("MahjongCVCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint EnumVideoInputDevices(IntPtr callback);

        [DllImport("MahjongCVCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint InitializeEVR();

        [DllImport("MahjongCVCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint UninitializeEVR();
    }
}
