// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2019

using System;
using System.Windows.Media;

namespace MahjongCVCamera.SourceStream
{
    internal class WebcamStream : ISourceStream
    {
        public bool Connected    { get; private set; }
        public uint OutputWidth  { get; private set; }
        public uint OutputHeight { get; private set; }

        public event RepaintEventHandler RepaintRequested;
        public event EventHandler        Disconnected;

        public WebcamStream(uint parentInitialWidth, uint parentInitialHeight)
        {
            OutputWidth = parentInitialWidth;
            OutputHeight = parentInitialHeight;
        }

        public void Connect()
        {
            NativeMethods.InitializeEVR();
        }

        public void Disconnect()
        {
            // TODO: this
            Disconnected?.Invoke(this, null);
        }

        public void Render(DrawingContext dc, uint frame)
        {
            throw new NotImplementedException();
        }

        public void SetOutputSize(uint width, uint height)
        {
            throw new NotImplementedException();
        }

        public ISourceInfo TakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
