﻿// [Ready Design Corps] - [Mahjong CV Test App] - Copyright 2018

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
                var collection = new ObservableCollection<ISourceInfo>();
                collection.Add(FindResource("GradientInfo") as ISourceInfo);
                collection.Add(FindResource("ImageInfo") as ISourceInfo);
                return collection;
            }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
