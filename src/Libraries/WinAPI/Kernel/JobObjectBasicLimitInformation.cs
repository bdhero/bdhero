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
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace WinAPI.Kernel
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct JobObjectBasicLimitInformation
    {
        public readonly Int64 PerProcessUserTimeLimit;
        public readonly Int64 PerJobUserTimeLimit;
        public LimitFlags LimitFlags;
        public readonly UIntPtr MinimumWorkingSetSize;
        public readonly UIntPtr MaximumWorkingSetSize;
        public readonly Int16 ActiveProcessLimit;
        public readonly Int64 Affinity;
        public readonly Int16 PriorityClass;
        public readonly Int16 SchedulingClass;
    }
}