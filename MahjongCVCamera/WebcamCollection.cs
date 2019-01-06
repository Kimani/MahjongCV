// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2019

using MahjongCVCamera.SourceInfo;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MahjongCVCamera
{
    public class WebcamCollection
    {
        //public event EventHandler WebcamCollectionChanged;

        public static void CameraEnumerationCallback(string path, string name)
        {

        }

        public IEnumerable<WebcamInfo> EnumerateCameras()
        {
            var callback = new NativeMethods.EnumVideoInputDevicesCallback(WebcamCollection.CameraEnumerationCallback);
            NativeMethods.EnumVideoInputDevices(Marshal.GetFunctionPointerForDelegate(callback));

            return null;
        }
    }
}
