// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using MahjongCVCamera.SourceStream;

namespace MahjongCVCamera.SourceInfo
{
    public class AnimatedGradientInfo : ISourceInfo
    {
        // ISourceInfo
        public string Name      { get { return "Animated Gradient"; } }
        public string Path      { get { return null; } }
        public bool   Available { get { return true; } }
        public bool   Static    { get { return false; } }

        public ISourceStream Open(uint parentInitialWidth, uint parentInitialHeight)
        {
            return new AnimatedGradientSourceStream(parentInitialWidth, parentInitialHeight);
        }
    }
}
