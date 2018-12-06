// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.IO;

namespace MahjongCVCamera
{
    public class FileImageSourceInfo : ISourceInfo
    {
        // ISourceInfo
        public string Name      { get; private set; }
        public string Path      { get; private set; }
        public bool   Available { get { return AvailableImpl(); } }

        public ISourceStream Open()
        {
            if (!Available) { throw new Exception("Image file unavailable."); }
            return new FileImageSourceStream(this);
        }

        // FileImageSourceInfo
        public FileImageSourceInfo(string path)
        {
            if (!IsPathValid(path)) { throw new Exception("Path is not valid"); }

            Path = path;
            Name = System.IO.Path.GetFileName(path);
        }

        private bool AvailableImpl()
        {
            try
            {
                string fullPath = System.IO.Path.GetFullPath(Path);
                return File.Exists(fullPath);
            }
            catch (Exception) { return false; }
        }

        private static bool IsPathValid(string path)
        {
            try
            {
                string fullPath = System.IO.Path.GetFullPath(path);
                return System.IO.Path.IsPathRooted(fullPath);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
