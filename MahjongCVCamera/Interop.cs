// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2019

using System;
using System.Runtime.InteropServices;

namespace MahjongCVCamera
{
    public static class NativeMethods
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EnumVideoInputDevicesCallback([MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string name);

        [DllImport("MahjongCVCore.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint EnumVideoInputDevices(IntPtr callback);
    }
}
