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

using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DotNetUtils.Extensions
{
    internal static class CommonFileDialogResultExtensions
    {
        public static DialogResult ToDialogResult(this CommonFileDialogResult cfdr)
        {
            if (cfdr == CommonFileDialogResult.Ok)
                return DialogResult.OK;

            if (cfdr == CommonFileDialogResult.Cancel)
                return DialogResult.Cancel;

            return DialogResult.None;
        }
    }
}