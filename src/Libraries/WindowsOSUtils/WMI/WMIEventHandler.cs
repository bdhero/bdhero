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

namespace WindowsOSUtils.WMI
{
    /// <summary>
    /// Asynchronously handles WMI events related to the WMI class specified by <typeparamref name="T"/>.
    /// </summary>
    /// <param name="wmiObjectInstance">Instance of a .NET object that represents the Win32 WMI class object returned by the event</param>
    /// <typeparam name="T">.NET type that represents the Win32 WMI class</typeparam>
    public delegate void WMIEventHandler<T>(T wmiObjectInstance);
}