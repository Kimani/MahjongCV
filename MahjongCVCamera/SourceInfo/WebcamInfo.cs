// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Collections.Generic;

namespace MahjongCVCamera.SourceInfo
{
    public class WebcamInfo : ISourceInfo
    {
        // ISourceInfo
        public string Name      { get; private set; }
        public string Path      { get; private set; }
        public bool   Available { get { return AvailableImpl(); } }
        public bool   Static    { get { return false; } }

        public ISourceStream Open(uint parentInitialWidth, uint parentInitialHeight)
        {
            throw new NotImplementedException();
        }

        // WebcamInfo
        public static IEnumerable<WebcamInfo> EnumerateCameras()
        {
            return null;
        }

        private WebcamInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }

        private bool AvailableImpl()
        {
            return false;
        }
    }
}
