// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using DotNetUtils.Annotations;

namespace DotNetUtils.Forms
{
    /// <summary>
    ///     Delegate that returns the full absolute path to the file selected in the given
    ///     <paramref name="sender"/> control.
    /// </summary>
    /// <param name="sender">Control that the user started dragging.</param>
    public delegate string PathGetter(Control sender);

    /// <summary>
    ///     Enables drag support for Windows Forms controls that contain a list of files.
    ///     Controls that have an <see cref="ExternalDragProvider"/> become drag sources, meaning a user can drag
    ///     the control into other applications and drop the file itself (if supported by the target drop application)
    ///     or a path to the file.
    /// </summary>
    public class ExternalDragProvider
    {
        /// <summary>
        ///     Serializable dummy class that gets attached to drag events initiated by this provider.
        ///     This allows other .NET classes to detect that a drag and drop operation originated
        ///     from this provider, thus enabling them to ignore the event.
        /// </summary>
        [Serializable]
        public class Format
        {
        }

        private readonly Control _dragSource;

        /// <summary>
        ///     Gets or sets a delegate that returns an absolute path to the control's currently selected file.
        /// </summary>
        public PathGetter PathGetter;

        /// <summary>
        ///     Gets or sets the minimum number of pixels the mouse must move in either axis (X or Y)
        ///     before a drag event is triggered.
        /// </summary>
        public uint Threshold = 2;

        private bool _leftMouseDown;
        private bool _isAttached;
        private string _path;
        private Point _startPos;

        /// <summary>
        ///     Constructs a new <see cref="ExternalDragProvider"/> instance that adds file drag functionality to the
        ///     given control.
        /// </summary>
        /// <param name="dragSource">Windows Forms control that contains a list of files.</param>
        public ExternalDragProvider([NotNull] Control dragSource)
        {
            _dragSource = dragSource;
            _dragSource.MouseDown += OnMouseDown;
            _dragSource.MouseMove += OnMouseMove;
            _dragSource.MouseUp += OnMouseUp;
        }

        private bool HasPath
        {
            get { return (_path = PathGetter != null ? PathGetter(_dragSource) : null) != null; }
        }

        /// <summary>
        ///     Uninstalls this drag provider from the host control, disabling drag support.
        /// </summary>
        public void Destroy()
        {
            _dragSource.MouseDown -= OnMouseDown;
            _dragSource.MouseMove -= OnMouseMove;
            _dragSource.MouseUp -= OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseEventArgs args)
        {
            _leftMouseDown = args.Button == MouseButtons.Left;

            if (!_leftMouseDown)
            {
                return;
            }
            if (!HasPath)
            {
                return;
            }

            _startPos = args.Location;
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (!_leftMouseDown)
            {
                return;
            }
            if (_isAttached)
            {
                return;
            }
            if (_path == null)
            {
                return;
            }
            if (Math.Abs(args.X - _startPos.X) < Threshold &&
                Math.Abs(args.Y - _startPos.Y) < Threshold)
            {
                return;
            }

            var paths = new StringCollection { _path };
            var dataObject = new DataObject();

            dataObject.SetFileDropList(paths);
            dataObject.SetText(_path);

            // Allow other classes to check if the DragDrop event was generated by this class
            dataObject.SetData(typeof(Format), new Format());

            _dragSource.DoDragDrop(dataObject, DragDropEffects.Copy);

            _isAttached = true;
        }

        private void OnMouseUp(object sender, MouseEventArgs args)
        {
            if (!_leftMouseDown)
            {
                return;
            }
            _leftMouseDown = false;
            _isAttached = false;
        }
    }
}