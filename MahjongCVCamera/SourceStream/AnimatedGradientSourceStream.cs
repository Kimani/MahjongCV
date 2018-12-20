// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace MahjongCVCamera.SourceStream
{
    internal class AnimatedGradientSourceStream : ISourceStream
    {
        // ISourceStream
        public bool Connected    { get { return true; } }
        public uint OutputWidth  { get; private set; }
        public uint OutputHeight { get; private set; }

        public event RepaintEventHandler RepaintRequested;
        public event EventHandler        Disconnected;

        public ISourceInfo TakeSnapshot() { return null; }

        public void Connect()
        {
            // Paint the first frame.
            _FirstPaintTick = Environment.TickCount;
            _LastFramePaint = 0;
            RepaintRequested?.Invoke(0);

            // Start a thread that will loop and request a repaint at 60 fps.
            _RepaintThread = new Thread(new ParameterizedThreadStart(RepaintThread));
            _RepaintThread.IsBackground = true;
            _RepaintThread.Start(++_Session);
        }

        public void Disconnect()
        {
            ++_Session;
            _RepaintThread = null;
            Disconnected?.Invoke(this, null);
        }

        public void Render(DrawingContext dc, uint frame)
        {
            float perc = ((float)(frame % 60)) / 59.0f;
            byte grey = (byte)(255.0f * perc);

            Color c = new Color();
            c.R = grey;
            c.G = grey;
            c.B = grey;
            c.A = 255;

            Point renderPoint = new Point(0, 0);
            dc.DrawRectangle(
                new SolidColorBrush(c),
                new Pen(Brushes.Black, 2),
                new Rect(
                    renderPoint,
                    new Size(OutputWidth, OutputHeight)));

            if ((frame - _LastFramePaint) > 1)
            {
                dc.DrawText(new FormattedText(
                                (frame - _LastFramePaint).ToString(),
                                CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                new Typeface("Consolas"),
                                15,
                                new SolidColorBrush(Colors.Black)),
                            renderPoint);
            }
            _LastFramePaint = frame;
        }

        public void SetOutputSize(uint width, uint height)
        {
            OutputWidth = width;
            OutputHeight = height;
        }

        // AnimatedGradientSourceStream
        private Thread _RepaintThread;
        private int    _Session = 0;
        private int    _FirstPaintTick;
        private uint   _LastFramePaint;

        internal AnimatedGradientSourceStream(uint w, uint h) { SetOutputSize(w, h); }

        private void RepaintThread(object arg)
        {
            int lastPaintTick = _FirstPaintTick;

            for (uint nextFrame = 1; ((int)arg) == _Session; ++nextFrame)
            {
                int nextSleepTime = 16 - Math.Min(16, (Environment.TickCount - lastPaintTick));
                Thread.Sleep(nextSleepTime);

                lastPaintTick = Environment.TickCount;
                RepaintRequested?.Invoke(nextFrame);
            }
        }
    }
}
