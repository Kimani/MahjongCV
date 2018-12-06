// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MahjongCVCamera
{
    public class VideoStream : FrameworkElement
    {
        private VisualCollection _Collection;
        private DispatcherTimer  _Timer;
        private DrawingVisual    _BackgroundDrawingVisual;
        private int              _GreyscaleValue = 0;

        protected override int    VisualChildrenCount       { get { return _Collection.Count; } }
        protected override Visual GetVisualChild(int index) { return _Collection[index]; }

        public VideoStream()// ISourceStream info)
        {
            _Collection = new VisualCollection(this);
            _Timer = new DispatcherTimer();

            //if (!info.Active) { throw new Exception("Stream unavailable."); }

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            double actualHeight = this.ActualHeight;
            double actualWidth = this.ActualWidth;

            _BackgroundDrawingVisual = new DrawingVisual();
            using (DrawingContext dc = _BackgroundDrawingVisual.RenderOpen())
            {
                dc.DrawRectangle(
                    Brushes.Blue,
                    new Pen(Brushes.Black, 2),
                    new Rect(
                        new Point(0, 0),
                        new Size(actualWidth, actualHeight)));
                _Collection.Add(_BackgroundDrawingVisual);
            }

            int x = 0;
            for (int i = 0; i <= 1; i++)
            {
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext dc = visual.RenderOpen())
                {
                    dc.DrawRectangle(
                        Brushes.Red,
                        new Pen(Brushes.Black, 2),
                        new Rect(
                            new Point(0 + x, 0),
                            new Size(40, 40)));
                }

                using (DrawingContext dc = visual.RenderOpen())
                {
                    dc.DrawRectangle(
                        Brushes.Green,
                        new Pen(Brushes.Black, 2),
                        new Rect(
                            new Point(10 + x, 10),
                            new Size(20, 20)));
                }
                _Collection.Add(visual);
                x += 60;
            }


            _Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            _Timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            _Timer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            using (DrawingContext dc = _BackgroundDrawingVisual.RenderOpen())
            {
                double actualHeight = this.ActualHeight;
                double actualWidth = this.ActualWidth;
                Color c = new Color();
                c.R = (byte)_GreyscaleValue;
                c.G = (byte)_GreyscaleValue;
                c.B = (byte)_GreyscaleValue;
                c.A = 255;

                dc.DrawRectangle(
                    new SolidColorBrush(c),
                    new Pen(Brushes.Black, 2),
                    new Rect(
                        new Point(0, 0),
                        new Size(actualWidth, actualHeight)));
            }

            _GreyscaleValue = (_GreyscaleValue + 1) % 255;
        }
    }
}
