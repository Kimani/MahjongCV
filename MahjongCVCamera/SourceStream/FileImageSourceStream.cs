// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using MahjongCVCamera.SourceInfo;

namespace MahjongCVCamera
{
    internal class FileImageSourceStream : ISourceStream
    {
        // ISourceStream
        public bool Connected    { get { return (_ImageCache != null) && _Info.Available; } }
        public uint OutputWidth  { get; private set; }
        public uint OutputHeight { get; private set; }

        public event RepaintEventHandler RepaintRequested;
        public event EventHandler        Disconnected;

        public ISourceInfo TakeSnapshot() { throw new NotImplementedException(); }
        public void        Connect()      { LoadImageData(); }
        public void        Disconnect()   { Disconnected?.Invoke(this, null); }

        public void Render(DrawingContext dc, uint frame)
        {
            if ((_ImageCache != null) && (frame > _LastPaintedFrame))
            {
                GetImageSize(out uint imageWidth, out uint imageHeight);

                dc.DrawImage(
                    _ImageCache,
                    new System.Windows.Rect(
                        ((OutputWidth - imageWidth) / 2),
                        ((OutputHeight - imageHeight) / 2),
                        imageWidth,
                        imageHeight));
                _LastPaintedFrame = _FrameCount;
            }
        }

        public void SetOutputSize(uint width, uint height)
        {
            OutputWidth = width;
            OutputHeight = height;

            if (Connected)
            {
                RepaintRequested?.Invoke(++_FrameCount);
            }
        }

        // FileImageSourceStream
        private FileImageSourceInfo _Info;
        private ImageSource         _ImageCache;
        private uint                _FrameCount = 0;
        private uint                _LastPaintedFrame = 0;
        private uint                _InputWidth = 0;
        private uint                _InputHeight = 0;

        internal FileImageSourceStream(FileImageSourceInfo info, uint parentInitialWidth, uint parentInitialHeight)
        {
            _Info = info;
            OutputWidth = parentInitialWidth;
            OutputHeight = parentInitialHeight;
        }

        private bool LoadImageData()
        {
            try
            {
                Bitmap imageBitmap = new Bitmap(_Info.Path);
                ImageSource source = BitmapToImageSource(imageBitmap);

                // Successfully loaded the bitmap. Otherwise just keep whatever bitmap we already have loaded.
                _ImageCache = source;
                _InputWidth = (uint)imageBitmap.Width;
                _InputHeight = (uint)imageBitmap.Height;

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

        private void GetImageSize(out uint width, out uint height)
        {
            // https://stackoverflow.com/questions/6565703/math-algorithm-fit-image-to-screen-retain-aspect-ratio
            double imageRatio = ((double)_InputWidth) / ((double)_InputHeight);
            double screenRatio = ((double)OutputWidth) / ((double)OutputHeight);

            if (screenRatio > imageRatio)
            {
                width = (uint)(((double)_InputWidth) * (((double)OutputHeight) / ((double)_InputHeight)));
                height = OutputHeight;
            }
            else
            {
                width = OutputWidth;
                height = (uint)(((double)_InputHeight) * (((double)OutputWidth) / ((double)_InputWidth)));
            }
        }
    }
}
