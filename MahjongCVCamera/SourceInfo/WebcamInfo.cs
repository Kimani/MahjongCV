// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongCVCamera
{
    public class WebcamInfo
    {
        public static IEnumerable<WebcamInfo> EnumerateCameras()
        {
            return null;
        }

        public string FriendlyName { get; private set; }
        public string DevicePath   { get; private set; }
        public bool   Available    { get { return AvailableImpl(); } }

        private WebcamInfo(string name, string path)
        {
            FriendlyName = name;
            DevicePath = path;
        }

        private bool AvailableImpl()
        {
            return false;
        }
    }
}
