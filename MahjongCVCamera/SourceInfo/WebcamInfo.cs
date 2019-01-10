// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using MahjongCVCamera.SourceStream;

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
            //if (!Available) { throw new Exception("Webcam unavailable."); }
            return new WebcamStream(parentInitialWidth, parentInitialHeight);
        }

        // WebcamInfo
        public override string ToString() { return Name; }

        internal WebcamInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }

        private bool AvailableImpl()
        {
            // TODO: this
            return false;
        }
    }
}
