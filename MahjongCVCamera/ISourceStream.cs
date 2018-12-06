// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongCVCamera
{
    public interface ISourceInfo
    {
        string Name      { get; }
        string Path      { get; }
        bool   Available { get; }

        ISourceStream Open();
    }

    public interface ISourceStream
    {
        bool Active       { get; }
        bool IsStatic     { get; }
        int  OutputWidth  { get; set; }
        int  OutputHeight { get; set; }

        ISourceInfo TakeSnapshot();
        bool        Reconnect();
    }
}
