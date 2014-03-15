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
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// ReSharper disable MemberCanBePrivate.Global
namespace NativeAPI.Win.User
{
    /// <summary>
    ///     Wraps a <see cref="Message"/> object to provide more useful functionality.
    /// </summary>
    public sealed class WindowMessage
    {
        #region Private fields

        private readonly Message _nativeMessage;

        #endregion

        #region Lazy initialized fields

        private WindowMessageType? _type;
        private WParam? _wParam;
        private LParam? _lParam;

        private Int64? _wParamInt64Value;
        private Int32? _lParamInt32Value;

        #endregion

        #region Managed properties

        public WindowMessageType Type
        {
            get { return _type.HasValue ? _type.Value : (_type = GetMessageType(Id)).Value; }
        }

        public WParam WParam
        {
            get { return _wParam.HasValue ? _wParam.Value : (_wParam = GetWParam(WParamInt64Value)).Value; }
        }

        public LParam LParam
        {
            get { return _lParam.HasValue ? _lParam.Value : (_lParam = GetLParam(LParamInt32Value)).Value; }
        }

        #endregion

        #region Native properties

        public int Id
        {
            get { return _nativeMessage.Msg; }
        }

        public IntPtr WParamPtr
        {
            get { return _nativeMessage.WParam; }
        }

        public IntPtr LParamPtr
        {
            get { return _nativeMessage.LParam; }
        }

        public Int64 WParamInt64Value
        {
            get { return _wParamInt64Value.HasValue ? _wParamInt64Value.Value : (_wParamInt64Value = WParamToInt64(WParamPtr)).Value; }
        }

        public Int32 LParamInt32Value
        {
            get { return _lParamInt32Value.HasValue ? _lParamInt32Value.Value : (_lParamInt32Value = LParamToInt32(LParamPtr)).Value; }
        }

        #endregion

        #region Constructor

        public WindowMessage(Message m)
        {
            _nativeMessage = m;
        }

        #endregion

        #region Marshalling

        public T GetLParamAsStruct<T>() where T : struct
        {
            return PInvokeUtils.MarshalPtrToStruct<T>(LParamPtr);
        }

        public T GetLParamAsObject<T>() where T : class
        {
            return PInvokeUtils.MarshalPtrToObject<T>(LParamPtr);
        }

        #endregion

        #region Equality tests

        public bool Is(WindowMessageType type)
        {
            return Id == type.ToInt32();
        }

        public bool IsOneOf(params WindowMessageType[] types)
        {
            var actual = Id;
            return types.Any(type => type.ToInt32() == actual);
        }

        public bool Is(WParam wParam)
        {
            return WParamInt64Value == wParam.ToInt64();
        }

        public bool IsOneOf(params WParam[] wParams)
        {
            var actual = WParamInt64Value;
            return wParams.Any(wParam => wParam.ToInt64() == actual);
        }

        public bool Is(LParam lParam)
        {
            return LParamInt32Value == lParam.ToInt32();
        }

        public bool IsOneOf(params LParam[] lParams)
        {
            var actual = LParamInt32Value;
            return lParams.Any(lParam => lParam.ToInt32() == actual);
        }

        #endregion

        #region Explicit conversion

        private static Int64 WParamToInt64(IntPtr wParamPtr)
        {
            return wParamPtr.ToInt64();
        }

        private static Int32 LParamToInt32(IntPtr lParamPtr)
        {
            return Marshal.ReadInt32(lParamPtr, 4);
        }

        private static WindowMessageType GetMessageType(int intVal)
        {
            var message = (WindowMessageType) intVal;
            if (Enum.IsDefined(typeof(WindowMessageType), message))
                return message;
            return WindowMessageType.WM_NULL;
        }

        private static WParam GetWParam(Int64 intVal)
        {
            var wParam = (WParam) intVal;
            if (Enum.IsDefined(typeof(WParam), wParam))
                return wParam;
            return WParam.UNKNOWN;
        }

        private static LParam GetLParam(Int32 intVal)
        {
            var lParam = (LParam) intVal;
            if (Enum.IsDefined(typeof(LParam), lParam))
                return lParam;
            return LParam.UNKNOWN;
        }

        #endregion

        #region Implicit conversion

        public static implicit operator WindowMessage(Message m)
        {
            return new WindowMessage(m);
        }

        public static implicit operator Message(WindowMessage windowMessage)
        {
            return windowMessage._nativeMessage;
        }

        #endregion
    }
}
