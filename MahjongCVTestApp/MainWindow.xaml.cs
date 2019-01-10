// [Ready Design Corps] - [Mahjong CV Test App] - Copyright 2018

using System.Collections.ObjectModel;
using System.Windows;
using MahjongCVCamera;

namespace MahjongCVTestApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ISourceInfo> SourceInfoCollection
        {
            get
            {
                var collection = new ObservableCollection<ISourceInfo>
                {
                    FindResource("GradientInfo") as ISourceInfo,
                    FindResource("ImageInfo") as ISourceInfo
                };

                foreach (ISourceInfo info in _Webcams.Webcams)
                {
                    collection.Add(info);
                }
                return collection;
            }
        }

        private WebcamCollection _Webcams;

        public MainWindow()
        {
            _Webcams = WebcamCollection.GetInstance();
            _Webcams.WebcamCollectionChanged += WebcamCollectionChanged;
            _Webcams.EnsureConnected();

            DataContext = this;
            InitializeComponent();
        }

        private void WebcamCollectionChanged(object sender, WebcamCollectionChangedEventArgs e)
        {
            //throw new System.NotImplementedException();
        }
    }
}
