// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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

using System.Windows.Forms;

namespace DotNetUtils.Forms
{
    /// <summary>
    ///     Interface for a Windows Forms control that notifies observers whenever its <see cref="Control.WndProc" />
    ///     method is called.
    /// </summary>
    public interface IWndProcObservable
    {
        /// <summary>
        ///     Invoked whenever the implementing class receives a Windows <see cref="Message" />.
        /// </summary>
        event WndProcEventHandler WndProcMessage;
    }

    /// <summary>
    ///     Represents a method that will handle a Windows <see cref="Message" />.
    /// </summary>
    /// <param name="m">Windows Message received from the operating system.</param>
    public delegate void WndProcEventHandler(ref Message m);
}
