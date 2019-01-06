// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;

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
        internal WebcamInfo(string name, string path)
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
