using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils.Annotations;

// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace DotNetUtils
{
    public static class DragUtils
    {
        public static bool HasFile(DragEventArgs e)
        {
            return GetFirstFilePath(e) != null;
        }

        public static bool HasDirectory(DragEventArgs e)
        {
            return GetFirstDirectoryPath(e) != null;
        }

        /// <summary>
        /// Returns true if at least one (>= 1) of the dragged files has an extension that matches
        /// at least one (>= 1) of the given file extensions.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="extension">File extension of the form ".ext" or "ext" (case insensitive)</param>
        /// <returns></returns>
        public static bool HasFileExtension(DragEventArgs e, string extension)
        {
            return HasFileExtension(e, new[] { extension });
        }

        /// <summary>
        /// Returns true if at least one (>= 1) of the dragged files has an extension that matches
        /// at least one (>= 1) of the given file extensions.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="extensions">Collection of file extensions of the form ".ext" or "ext" (case insensitive)</param>
        /// <returns></returns>
        public static bool HasFileExtension(DragEventArgs e, IEnumerable<string> extensions)
        {
            return GetFilePaths(e).Any(path => FileUtils.FileHasExtension(path, extensions));
        }

        public static IList<string> GetFilesWithExtension(DragEventArgs e, string extension)
        {
            return GetFilesWithExtension(e, new[] { extension });
        }

        public static IList<string> GetFilesWithExtension(DragEventArgs e, IEnumerable<string> extensions)
        {
            return GetFilePaths(e).Where(path => FileUtils.FileHasExtension(path, extensions)).ToList();
        }

        public static string GetFirstFileWithExtension(DragEventArgs e, string extension)
        {
            return GetFirstFileWithExtension(e, new[] { extension });
        }

        public static string GetFirstFileWithExtension(DragEventArgs e, ICollection<string> extensions)
        {
            var files = GetFilesWithExtension(e, extensions);
            return files.Count > 0 ? files[0] : null;
        }

        public static ICollection<string> GetPaths(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return (string[])e.Data.GetData(DataFormats.FileDrop, false);
            }
            return new string[0];
        }

        public static string GetFirstPath(DragEventArgs e)
        {
            var paths = GetPaths(e);
            var enumerable = paths as List<string> ?? paths.ToList();
            return enumerable.Any() ? enumerable[0] : null;
        }

        public static ICollection<string> GetFilePaths(DragEventArgs e)
        {
            return GetPaths(e).Where(FileUtils.IsFile).ToArray();
        }

        public static string GetFirstFilePath(DragEventArgs e)
        {
            return GetPaths(e).FirstOrDefault(FileUtils.IsFile);
        }

        public static ICollection<string> GetDirectoryPaths(DragEventArgs e)
        {
            return GetPaths(e).Where(FileUtils.IsDirectory).ToArray();
        }

        public static string GetFirstDirectoryPath(DragEventArgs e)
        {
            var paths = GetPaths(e);
            return paths.FirstOrDefault(FileUtils.IsDirectory);
        }

        public static string GetFirstFileNameWithoutExtension(DragEventArgs e)
        {
            var path = GetFirstFilePath(e);
            return path == null ? null : Path.GetFileNameWithoutExtension(path);
        }

        public static string GetUnicodeText(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                return (string)e.Data.GetData(DataFormats.UnicodeText, false);
            }
            return null;
        }
    }

    public class ExternalDragProvider
    {
        private bool _leftMouseDown;
        private Point _startPos;
        private string _path;

        private readonly Control _dragSource;

        /// <summary>
        /// Gets or sets a delegate that returns an absolute path to the control's currently selected file.
        /// </summary>
        public PathGetter PathGetter;

        /// <summary>
        /// Gets or sets the minimum number of pixels the mouse must move in either axis (X or Y) before a drag event is triggered.
        /// </summary>
        public uint Threshold = 2;

        public ExternalDragProvider([NotNull] Control dragSource)
        {
            _dragSource = dragSource;
            _dragSource.MouseDown += OnMouseDown;
            _dragSource.MouseMove += OnMouseMove;
            _dragSource.MouseUp += OnMouseUp;
        }

        public void Destroy()
        {
            _dragSource.MouseDown -= OnMouseDown;
            _dragSource.MouseMove -= OnMouseMove;
            _dragSource.MouseUp -= OnMouseUp;
        }

        private bool HasPath
        {
            get { return (_path = PathGetter != null ? PathGetter(_dragSource) : null) != null; }
        }

        private void OnMouseDown(object sender, MouseEventArgs args)
        {
            _leftMouseDown = args.Button == MouseButtons.Left;

            if (!_leftMouseDown) return;
            if (!HasPath) return;

            _startPos = args.Location;
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (!_leftMouseDown) return;
            if (_path == null) return;
            if (Math.Abs(args.X - _startPos.X) < Threshold &&
                Math.Abs(args.Y - _startPos.Y) < Threshold) return;

            var paths = new StringCollection {_path};
            var dataObject = new DataObject();

            dataObject.SetFileDropList(paths);
            dataObject.SetText(_path);

            // Allow other classes to check if the DragDrop event was generated by this class
            dataObject.SetData(typeof(ExternalDragProvider), this);

            _dragSource.DoDragDrop(dataObject, DragDropEffects.Copy);
        }

        private void OnMouseUp(object sender, MouseEventArgs args)
        {
            if (!_leftMouseDown) return;
            _leftMouseDown = false;
        }
    }

    public delegate string PathGetter(Control sender);
}
