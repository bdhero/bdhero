// Copyright 2014 Andrew C. Dvorak
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
using System.Diagnostics;
using WinAPI.User;

namespace WindowsOSUtils.Windows
{
    public static class Window
    {
        /// <summary>
        ///     Minimizes the main window of the specified <paramref name="process"/>.
        /// </summary>
        /// <param name="process">
        ///     A process that may or may not have been started by the current process and that may or may not have
        ///     a main window associated with it.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> has a main window and was successfully minimized;
        ///     otherwise <c>false</c>.
        /// </returns>
        public static bool Minimize(Process process)
        {
            return Minimize(process.MainWindowHandle);
        }

        /// <summary>
        ///     Minimizes the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A pointer to the window to minimize.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the specified window was successfully minimized; otherwise <c>false</c>.
        /// </returns>
        public static bool Minimize(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            if (WindowAPI.ShowWindowAsync(hWnd, ShowWindowCommand.SW_SHOWMINIMIZED))
                return true;

            return false;
        }
    }
}
