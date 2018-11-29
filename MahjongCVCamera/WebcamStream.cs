// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongCVCamera
{
    public class WebcamStream
    {
        public WebcamStream(WebcamInfo info)
        {
            if (!info.Available) { throw new Exception("Webcam unavailable."); }
        }

    }
}
