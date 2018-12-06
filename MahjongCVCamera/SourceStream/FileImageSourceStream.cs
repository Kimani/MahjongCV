// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;

namespace MahjongCVCamera
{
    internal class FileImageSourceStream : ISourceStream
    {
        // ISourceStream
        public bool Connected    { get { return (_ImageCache != null) && _Info.Available; } }
        public bool Static       { get { return true; } }
        public int  OutputWidth  { get { return _OutputWidth; }  set { _OutputWidth = value;  _OutputWidthSet = true; } }
        public int  OutputHeight { get { return _OutputHeight; } set { _OutputHeight = value; _OutputHeightSet = true; } }

        public event RepaintEventHandler RepaintRequested;
        public event EventHandler        Disconnected;

        public ISourceInfo TakeSnapshot()
        {
            return null;
        }

        public bool Reconnect()
        {
            return LoadImageData();
        }

        public void Render(DrawingContext dc, int frame)
        {
            if ((_ImageCache != null) && (frame > _LastPaintedFrame))
            {
                double outputWidth;
                double outputHeight;
                GetOutputSize(out outputWidth, out outputHeight);
                dc.DrawImage(_ImageCache, new System.Windows.Rect(0, 0, _OutputWidth, _OutputHeight));

                _LastPaintedFrame = _FrameCount;
            }
        }

        // FileImageSourceStream
        private FileImageSourceInfo _Info;
        private ImageSource         _ImageCache;
        private int                 _FrameCount = 0;
        private int                 _LastPaintedFrame = 0;
        private int                 _InputWidth = 0;
        private int                 _InputHeight = 0;
        private int                 _OutputWidth = 0;
        private int                 _OutputHeight = 0;
        private bool                _OutputWidthSet = false;
        private bool                _OutputHeightSet = false;

        internal FileImageSourceStream(FileImageSourceInfo info)
        {
            _Info = info;
            LoadImageData();
        }

        private bool LoadImageData()
        {
            try
            {
                Bitmap imageBitmap = new Bitmap(_Info.Path);
                ImageSource source = BitmapToImageSource(imageBitmap);

                // Successfully loaded the bitmap. Otherwise just keep whatever bitmap we already have loaded.
                _ImageCache = source;
                _InputWidth = imageBitmap.Width;
                _InputHeight = imageBitmap.Height;
                if (!_OutputWidthSet)  { _OutputWidth = _InputWidth; }
                if (!_OutputHeightSet) { _OutputHeight = _InputHeight; }

                RepaintRequested?.Invoke(++_FrameCount);
                return true;
            }
            catch (Exception) { return false; }
        }

        private ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            // https://stackoverflow.com/questions/22499407/how-to-display-a-bitmap-in-a-wpf-image
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        private void GetOutputSize(out double width, out double height)
        {
            width = _OutputWidth;
            height = _OutputHeight;

            if (_OutputWidthSet && !_OutputHeightSet)
            {
                double ratio = ((double)_InputWidth) / ((double)_InputHeight);
                height = Math.Round(width / ratio);
            }
            else if (!_OutputWidthSet && _OutputHeightSet)
            {
                double ratio = ((double)_InputWidth) / ((double)_InputHeight);
                width = Math.Round(height * ratio);
            }
        }
    }
}
