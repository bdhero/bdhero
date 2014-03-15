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

        #region Private fields (lazy-initialized)

        private WindowMessageType? _type;
        private WParam? _wParam;
        private LParam? _lParam;

        private Int64? _wParamInt64Value;
        private Int32? _lParamInt32Value;

        #endregion

        #region Managed (parsed/interpreted) properties

        /// <summary>
        ///     Gets the specific message type.
        /// </summary>
        public WindowMessageType Type
        {
            get { return _type.HasValue ? _type.Value : (_type = GetMessageType(Id)).Value; }
        }

        /// <summary>
        ///     Gets the parsed WPARAM value.
        /// </summary>
        public WParam WParam
        {
            get { return _wParam.HasValue ? _wParam.Value : (_wParam = GetWParam(WParamInt64Value)).Value; }
        }

        /// <summary>
        ///     Gets the parsed LPARAM value.
        /// </summary>
        public LParam LParam
        {
            get { return _lParam.HasValue ? _lParam.Value : (_lParam = GetLParam(LParamInt32Value)).Value; }
        }

        /// <summary>
        ///     Gets the value of the native WPARAM field as a 64-bit signed integer.
        /// </summary>
        public Int64 WParamInt64Value
        {
            get { return _wParamInt64Value.HasValue ? _wParamInt64Value.Value : (_wParamInt64Value = WParamToInt64(WParamPtr)).Value; }
        }

        /// <summary>
        ///     Gets the value of the native LPARAM field as a 32-bit signed integer.
        /// </summary>
        public Int32 LParamInt32Value
        {
            get { return _lParamInt32Value.HasValue ? _lParamInt32Value.Value : (_lParamInt32Value = LParamToInt32(LParamPtr)).Value; }
        }

        #endregion

        #region Native properties (proxy getters)

        /// <summary>
        ///     Gets the message identifier (ID number). Applications can only use the low word; the high word is reserved by the system.
        /// </summary>
        public int Id
        {
            get { return _nativeMessage.Msg; }
        }

        /// <summary>
        ///     Gets a pointer to the native WPARAM field.
        /// </summary>
        public IntPtr WParamPtr
        {
            get { return _nativeMessage.WParam; }
        }

        /// <summary>
        ///     Gets a pointer to the native LPARAM field.
        /// </summary>
        public IntPtr LParamPtr
        {
            get { return _nativeMessage.LParam; }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructs a new <see cref="WindowMessage"/> instance that wraps the given <paramref name="nativeMessage"/>.
        /// </summary>
        /// <param name="nativeMessage">
        ///     Native window message.
        /// </param>
        public WindowMessage(Message nativeMessage)
        {
            _nativeMessage = nativeMessage;
        }

        #endregion

        #region Marshalling

        /// <summary>
        ///     Marshals the unmanaged structure pointed to by the native LPARAM field to a managed .NET CLR struct.
        /// </summary>
        /// <typeparam name="T">
        ///     Expected type of the LPARAM structure.
        /// </typeparam>
        /// <returns>
        ///     A structure of type <typeparamref name="T"/> populated with the values contained in the native LPARAM field.
        /// </returns>
        public T GetLParamAsStruct<T>() where T : struct
        {
            return PInvokeUtils.MarshalPtrToStruct<T>(LParamPtr);
        }

        /// <summary>
        ///     Marshals the unmanaged object pointed to by the native LPARAM field to a managed .NET CLR object.
        /// </summary>
        /// <typeparam name="T">
        ///     Expected type of the LPARAM object.
        /// </typeparam>
        /// <returns>
        ///     An instance of an object of type <typeparamref name="T"/> populated with the values contained in the native LPARAM field.
        /// </returns>
        public T GetLParamAsObject<T>() where T : class
        {
            return PInvokeUtils.MarshalPtrToObject<T>(LParamPtr);
        }

        #endregion

        #region Equality tests

        /// <summary>
        ///     Determines whether this window message matches the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        ///     Window message type to check for equality.
        /// </param>
        /// <returns>
        ///     <c>true</c> if this window message matches the specified <paramref name="type"/>; otherwise <c>false</c>.
        /// </returns>
        public bool Is(WindowMessageType type)
        {
            return Id == type.ToInt32();
        }

        /// <summary>
        ///     Determines whether this window message matches at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="types">
        ///     Array of window message types.
        /// </param>
        /// <returns>
        ///     <c>true</c> if this window message matches at least one of the specified <paramref name="types"/>; otherwise <c>false</c>.
        /// </returns>
        public bool IsOneOf(params WindowMessageType[] types)
        {
            var actual = Id;
            return types.Any(type => type.ToInt32() == actual);
        }

        /// <summary>
        ///     Determines whether the <see cref="WParam"/> field of this window message matches the specified <paramref name="wParam"/>.
        /// </summary>
        /// <param name="wParam">
        ///     <see cref="User.WParam"/> value to check for equality.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the <see cref="WParam"/> field of this window message matches the specified <paramref name="wParam"/>; otherwise <c>false</c>.
        /// </returns>
        public bool Is(WParam wParam)
        {
            return WParamInt64Value == wParam.ToInt64();
        }

        /// <summary>
        ///     Determines whether the <see cref="WParam"/> field of this window message matches at least one of the specified <paramref name="wParams"/>.
        /// </summary>
        /// <param name="wParams">
        ///     <see cref="User.WParam"/> values to check for equality.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the <see cref="WParam"/> field of this window message matches at least one of the specified <paramref name="wParams"/>; otherwise <c>false</c>.
        /// </returns>
        public bool IsOneOf(params WParam[] wParams)
        {
            var actual = WParamInt64Value;
            return wParams.Any(wParam => wParam.ToInt64() == actual);
        }

        /// <summary>
        ///     Determines whether the <see cref="LParam"/> field of this window message matches the specified <paramref name="lParam"/>.
        /// </summary>
        /// <param name="lParam">
        ///     <see cref="User.LParam"/> value to check for equality.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the <see cref="LParam"/> field of this window message matches the specified <paramref name="lParam"/>; otherwise <c>false</c>.
        /// </returns>
        public bool Is(LParam lParam)
        {
            return LParamInt32Value == lParam.ToInt32();
        }

        /// <summary>
        ///     Determines whether the <see cref="LParam"/> field of this window message matches at least one of the specified <paramref name="lParams"/>.
        /// </summary>
        /// <param name="lParams">
        ///     <see cref="User.LParam"/> values to check for equality.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the <see cref="LParam"/> field of this window message matches at least one of the specified <paramref name="lParams"/>; otherwise <c>false</c>.
        /// </returns>
        public bool IsOneOf(params LParam[] lParams)
        {
            var actual = LParamInt32Value;
            return lParams.Any(lParam => lParam.ToInt32() == actual);
        }

        #endregion

        #region Explicit conversion utility methods (private)

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

        #region Implicit conversion operators (public)

        /// <summary>
        ///     Implicitly converts a native <see cref="Message"/> structure to a <see cref="WindowMessage"/> wrapper object.
        /// </summary>
        /// <param name="message">
        ///     Native window message structure.
        /// </param>
        /// <returns>
        ///     A <see cref="WindowMessage"/> object that wraps the given <paramref name="message"/>.
        /// </returns>
        public static implicit operator WindowMessage(Message message)
        {
            return new WindowMessage(message);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="WindowMessage"/> wrapper object to a native <see cref="Message"/> structure.
        /// </summary>
        /// <param name="windowMessage">
        ///     Window message wrapper object.
        /// </param>
        /// <returns>
        ///     The native <see cref="Message"/> structure wrapped by <paramref name="windowMessage"/>.
        /// </returns>
        public static implicit operator Message(WindowMessage windowMessage)
        {
            return windowMessage._nativeMessage;
        }

        #endregion
    }
}
