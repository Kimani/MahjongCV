﻿// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MahjongCVCamera
{
    public delegate void SourceInfoChangedDelegate(ISourceInfo prev, ISourceInfo next);
    public delegate void FrameRenderDelegate();

    public struct StreamThreadArgs
    {
        public ISourceInfo Info;
        public HostVisual  Host;
        public uint        InitialWidth;
        public uint        InitialHeight;

        public StreamThreadArgs(ISourceInfo info, HostVisual host, uint initialWidth, uint initialHeight)
        {
            Info = info;
            Host = host;
            InitialWidth = initialWidth;
            InitialHeight = initialHeight;
        }
    }

    public class VideoStream : FrameworkElement
    {
        public static readonly DependencyProperty SourceInfoProperty =
            DependencyProperty.Register(
                "SourceInfo",
                typeof(ISourceInfo),
                typeof(VideoStream),
                new FrameworkPropertyMetadata(
                    null,
                    new PropertyChangedCallback(OnSourceInfoPropertyChanged)));
 
        public ISourceInfo SourceInfo
        {
            get { return GetValue(SourceInfoProperty) as ISourceInfo; }

            set
            {
                if (value != _LocalInfo)
                {
                    ISourceInfo previous = _LocalInfo;
                    _LocalInfo = value;
                    SetValue(SourceInfoProperty, value);

                    _Dispatcher.BeginInvoke(
                        new SourceInfoChangedDelegate(SourceInfoChanged),
                        DispatcherPriority.Normal,
                        new object[] { previous, value });
                }
            }
        }

        protected override int    VisualChildrenCount       { get { return _Collection.Count; } }
        protected override Visual GetVisualChild(int index) { return _Collection[index]; }

        // https://blogs.msdn.microsoft.com/dwayneneed/2007/04/26/multithreaded-ui-hostvisual/
        private static AutoResetEvent _HostVisualSetup = new AutoResetEvent(false);

        private VisualCollection _Collection;
        private DrawingVisual    _StreamRenderTarget;
        private ISourceInfo      _LocalInfo = null;
        private ISourceStream    _CurrentStream = null;
        private bool             _Loaded = false;
        private Dispatcher       _Dispatcher;
        private Thread           _AnimatedStreamThread = null;
        private Dispatcher       _AnimatedStreamDispatcher = null;
        private uint             _NextFrameToDraw = 0;
        private uint?            _LastDrawnFrame = null;
        private double           _LastWidth;
        private double           _LastHeight;

        public VideoStream()
        {
            _Collection = new VisualCollection(this);
            _Dispatcher = Dispatcher;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
        }

        private static void OnSourceInfoPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var videoStream = (sender as VideoStream);
            if (videoStream != null)
            {
                videoStream._LocalInfo = e.NewValue as ISourceInfo;
                videoStream.SourceInfoChanged((e.OldValue as ISourceInfo), (e.NewValue as ISourceInfo));
            }
        }

        private void SourceInfoChanged(ISourceInfo prevInfo, ISourceInfo nextInfo)
        {
            Dispatcher.VerifyAccess();
            if (_Loaded)
            {
                if (prevInfo != null) { UnloadSourceInfo(prevInfo); }
                if (nextInfo != null) { LoadSourceInfo(nextInfo); } // NEED TO DO SOMETHING TO MAKE IT REQUERY VisualChildrenCount HERE
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // If we have a source, connect to the source at this time.
            _Loaded = true;
            if (_LocalInfo != null) { LoadSourceInfo(_LocalInfo); }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _Loaded = false;
            if (_LocalInfo != null) { UnloadSourceInfo(_LocalInfo); }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((_CurrentStream != null) && ((e.NewSize.Width != _LastWidth) || (e.NewSize.Height != _LastHeight)))
            {
                _LastWidth = e.NewSize.Width;
                _LastHeight = e.NewSize.Height;
                _CurrentStream.SetOutputSize((uint)_LastWidth, (uint)_LastHeight);
            }
        }

        private void UnloadSourceInfo(ISourceInfo info)
        {
            Dispatcher.VerifyAccess();

            if (info.Static)
            {
                _CurrentStream.RepaintRequested -= StreamRepaintRequested;
                _CurrentStream.Disconnected -= StreamDisconnected;
                _CurrentStream.Disconnect();

                _CurrentStream = null;
                _StreamRenderTarget = null;
            }
            else
            {
                if (_AnimatedStreamThread != null)
                {
                    _AnimatedStreamDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                    _AnimatedStreamThread.Join(1000);
                    _AnimatedStreamThread = null;
                }
            }

            _Collection.Clear();
            _NextFrameToDraw = 0;
            _LastDrawnFrame = null;
        }

        private void LoadSourceInfo(ISourceInfo info)
        {
            Dispatcher.VerifyAccess();
            _LastWidth = ActualWidth;
            _LastHeight = ActualHeight;

            if (info.Static)
            {
                // Paint the stream on the main UI thread. The overhead of a background thread is not needed.
                _StreamRenderTarget = new DrawingVisual();
                _Collection.Add(_StreamRenderTarget);

                _CurrentStream = info.Open((uint)_LastWidth, (uint)_LastHeight);
                _CurrentStream.RepaintRequested += StreamRepaintRequested;
                _CurrentStream.Disconnected += StreamDisconnected;
                _CurrentStream.Connect();
            }
            else
            {
                if (_AnimatedStreamThread != null) { throw new Exception("Stale thread shouldn't exist when loading a new source!"); }

                // Create the host visual that will enable us to paint from a background thread.
                HostVisual hostVisual = new HostVisual();

                // Create a background thread to load the stream and then perform the STA message loop.
                StreamThreadArgs args = new StreamThreadArgs(info, hostVisual, (uint)_LastWidth, (uint)_LastHeight);

                _AnimatedStreamThread = new Thread(new ParameterizedThreadStart(StreamWorkerThread));
                _AnimatedStreamThread.SetApartmentState(ApartmentState.STA);
                _AnimatedStreamThread.IsBackground = true;
                _AnimatedStreamThread.Start(new StreamThreadArgs?(args));

                _HostVisualSetup.WaitOne();

                // Set the host visual as our child.
                _Collection.Add(hostVisual);
            }
        }

        private void StreamWorkerThread(object arg)
        {
            if (_CurrentStream != null) { throw new Exception("Stale stream shouldn't exist when starting a new stream!"); }

            // Setup stream resources, as well as the HostVisual. Then signal the UI thread to continue.
            _AnimatedStreamDispatcher = Dispatcher.CurrentDispatcher;

            var argsWrapper = arg as StreamThreadArgs?;
            StreamThreadArgs args = argsWrapper.Value;
            VisualTargetPresentationSource visualTarget = new VisualTargetPresentationSource(args.Host);

            _HostVisualSetup.Set();

            // Grab the stream, setup to eventing, and then Connect to it.
            _StreamRenderTarget = new DrawingVisual();
            visualTarget.RootVisual = _StreamRenderTarget;

            _CurrentStream = args.Info.Open(args.InitialWidth, args.InitialHeight);
            _CurrentStream.RepaintRequested += StreamRepaintRequested;
            _CurrentStream.Disconnected += StreamDisconnected;
            _CurrentStream.Connect();

            // Run the message loop.
            Dispatcher.Run();

            // Clean up _CurrentStream.
            _CurrentStream.RepaintRequested -= StreamRepaintRequested;
            _CurrentStream.Disconnected -= StreamDisconnected;
            _CurrentStream.Disconnect();
            _CurrentStream = null;
            _AnimatedStreamDispatcher = null;
            _StreamRenderTarget = null;
        }

        private void StreamRepaintRequested(uint frame)
        {
            Dispatcher dispatcherToUse = _LocalInfo.Static ? _Dispatcher : _AnimatedStreamDispatcher;

            if (dispatcherToUse.CheckAccess())
            {
                _NextFrameToDraw = frame;
                StreamPaint(frame);
                _LastDrawnFrame = frame;
            }
            else
            {
                // We're not on the correct thread. Queue the repaint onto the correct thread.
                // Many repaints may queue before we get a chance to render. Take the most recent
                // frame and draw that. Skip all the stale events that may happen in the interim,
                // although a stale event may become live if a frame is queued again.
                _NextFrameToDraw = frame;
                dispatcherToUse.BeginInvoke(new FrameRenderDelegate(() =>
                    {
                        uint frameToRender = _NextFrameToDraw;
                        if ((_LastDrawnFrame == null) || (_NextFrameToDraw > _LastDrawnFrame))
                        {
                            StreamPaint(frameToRender);
                        }
                        _LastDrawnFrame = frameToRender;
                    }),
                    DispatcherPriority.Render,
                    null);
            }
        }

        private void StreamDisconnected(object sender, EventArgs args)
        {
            // TODO
        }

        private void StreamPaint(uint frame)
        {
            using (DrawingContext dc = _StreamRenderTarget.RenderOpen())
            {
                _CurrentStream.Render(dc, frame);
            }
        }
    }
}
