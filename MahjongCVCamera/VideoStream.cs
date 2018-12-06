// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System.Windows;
using System.Windows.Media;

namespace MahjongCVCamera
{
    public class VideoStream : FrameworkElement
    {
        private VisualCollection _Collection;

        protected override int    VisualChildrenCount       { get { return _Collection.Count; } }
        protected override Visual GetVisualChild(int index) { return _Collection[index]; }

        public VideoStream()// ISourceStream info)
        {
            _Collection = new VisualCollection(this);

            //if (!info.Active) { throw new Exception("Stream unavailable."); }

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            double actualHeight = this.ActualHeight;
            double actualWidth = this.ActualWidth;

            DrawingVisual v = new DrawingVisual();
            using (DrawingContext dc = v.RenderOpen())
            {
                dc.DrawRectangle(
                    Brushes.Blue,
                    new Pen(Brushes.Black, 2),
                    new Rect(
                        new Point(0, 0),
                        new Size(actualWidth, actualHeight)));
                _Collection.Add(v);
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
        }
    }
}
