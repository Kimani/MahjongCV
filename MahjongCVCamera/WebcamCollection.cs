// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2019

using MahjongCVCamera.SourceInfo;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MahjongCVCamera
{
    public enum WebcamChangedType
    {
        Added,
        Removed
    }

    public class WebcamCollectionChangedEventArgs : EventArgs
    {
        public WebcamInfo        Info { get; private set; }
        public WebcamChangedType Type { get; private set; }

        public WebcamCollectionChangedEventArgs(WebcamInfo info, WebcamChangedType type)
        {
            Info = info;
            Type = type;
        }
    }

    public class WebcamCollection
    {
        // Statics
        private static WebcamCollection _Instance;

        public static WebcamCollection GetInstance()                            { _Instance = _Instance ?? new WebcamCollection(); return _Instance; }
        private static void CameraEnumerationCallback(string path, string name) { _Instance.AddCameraData(path, name); }

        // Members
        public IReadOnlyList<WebcamInfo> Webcams { get { return _WebcamInfoCollection.AsReadOnly(); } }

        public event EventHandler<WebcamCollectionChangedEventArgs> WebcamCollectionChanged;

        private List<WebcamInfo> _WebcamInfoCollection = new List<WebcamInfo>();
        private bool _Connected = false;

        private WebcamCollection() { }

        public void EnsureConnected()
        {
            if (!_Connected)
            {
                EnumerateCameras();
                _Connected = true;
            }
        }

        private void AddCameraData(string path, string name)
        {
            bool found = false;
            foreach (WebcamInfo info in _WebcamInfoCollection)
            {
                if (info.Path.Equals(path))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var newInfo = new WebcamInfo(name, path);
                _WebcamInfoCollection.Add(newInfo);
                WebcamCollectionChanged?.Invoke(this, new WebcamCollectionChangedEventArgs(newInfo, WebcamChangedType.Added));
            }
        }

        private void EnumerateCameras()
        {
            var callback = new NativeMethods.EnumVideoInputDevicesCallback(WebcamCollection.CameraEnumerationCallback);
            NativeMethods.EnumVideoInputDevices(Marshal.GetFunctionPointerForDelegate(callback));
        }
    }
}
