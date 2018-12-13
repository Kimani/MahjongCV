// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Windows;
using System.Windows.Media;

namespace MahjongCVCamera.SourceStream
{
    internal class AnimatedGradientSourceStream : ISourceStream
    {
        // ISourceStream
        public bool Connected    { get { return true; } }
        public uint OutputWidth  { get; set; }
        public uint OutputHeight { get; set; }

        public event RepaintEventHandler RepaintRequested;
        public event EventHandler        Disconnected;

        public void Connect()
        {
            // TEST just paint once.
            RepaintRequested?.Invoke(0);
        }

        public void Disconnect()
        {
            Disconnected?.Invoke(this, null);
        }

        public ISourceInfo TakeSnapshot()
        {
            return null;
        }

        public void Render(DrawingContext dc, uint frame)
        {
            float perc = ((float)(frame % 60)) / 59.0f;
            byte grey = (byte)(255.0f * perc);

            Color c = new Color();
            c.R = 255; //grey;
            c.G = grey;
            c.B = grey;
            c.A = 255;

            dc.DrawRectangle(
                new SolidColorBrush(c),
                new Pen(Brushes.Black, 2),
                new Rect(
                    new Point(0, 0),
                    new Size(OutputWidth, OutputHeight)));
        }

        // AnimatedGradientSourceStream
        internal AnimatedGradientSourceStream(uint w, uint h)
        {
            OutputWidth = w;
            OutputHeight = h;
        }
    }
}
