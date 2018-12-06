// [Ready Design Corps] - [Mahjong CV Test App] - Copyright 2018

using System.Windows;
using MahjongCVCamera;

namespace MahjongCVTestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var x = new VideoStream();
        }
    }
}
