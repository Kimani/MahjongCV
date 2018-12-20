// [Ready Design Corps] - [Mahjong CV Core] - Copyright 2018

using System;
using System.IO;

namespace MahjongCVCamera.SourceInfo
{
    public class FileImageSourceInfo : ISourceInfo
    {
        // ISourceInfo
        public string Name      { get; private set; }
        public string Path      { get { return _Path; } set { SetPathImpl(value); } }
        public bool   Available { get { return AvailableImpl(); } }
        public bool   Static    { get { return true; } }

        public ISourceStream Open(uint parentInitialWidth, uint parentInitialHeight)
        {
            if (!Available) { throw new Exception("Image file unavailable."); }
            return new FileImageSourceStream(this, parentInitialWidth, parentInitialHeight);
        }

        // FileImageSourceInfo
        private string _Path;

        private void SetPathImpl(string path)
        {
            if (path != null)
            {
                if (!IsPathValid(path)) { throw new Exception("Path is not valid"); }
                _Path = path;
                Name = System.IO.Path.GetFileName(path);
            }
            else
            {
                _Path = null;
                Name = null;
            }
        }

        private bool AvailableImpl()
        {
            if (_Path == null)
            {
                return false;
            }

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
            catch (Exception) { return false; }
        }
    }
}
