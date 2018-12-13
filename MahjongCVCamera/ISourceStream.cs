// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Windows.Media;

namespace MahjongCVCamera
{
    public delegate void RepaintEventHandler(uint frame);

    public interface ISourceInfo
    {
        string Name      { get; }
        string Path      { get; }
        bool   Available { get; }
        bool   Static    { get; }

        ISourceStream Open(uint parentInitialWidth, uint parentInitialHeight);
    }

    public interface ISourceStream
    {
        bool Connected    { get; }
        uint OutputWidth  { get; set; }
        uint OutputHeight { get; set; }

        event RepaintEventHandler RepaintRequested;
        event EventHandler        Disconnected;

        ISourceInfo TakeSnapshot();
        void        Connect();
        void        Disconnect();
        void        Render(DrawingContext dc, uint frame);
    }
}
