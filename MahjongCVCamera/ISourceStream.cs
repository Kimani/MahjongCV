// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Windows.Media;

namespace MahjongCVCamera
{
    public delegate void RepaintEventHandler(int frame);

    public interface ISourceInfo
    {
        string Name      { get; }
        string Path      { get; }
        bool   Available { get; }

        ISourceStream Open();
    }

    public interface ISourceStream
    {
        bool Connected    { get; }
        bool Static       { get; }
        int  OutputWidth  { get; set; }
        int  OutputHeight { get; set; }

        event RepaintEventHandler RepaintRequested;
        event EventHandler        Disconnected;

        ISourceInfo TakeSnapshot();
        bool        Reconnect();
        void        Render(DrawingContext dc, int frame);
    }
}
