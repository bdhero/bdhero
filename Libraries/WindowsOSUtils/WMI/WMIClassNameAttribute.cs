using System;

namespace WindowsOSUtils.WMI
{
    /// <summary>
    /// Declares the Win32 WMI class name of the .NET class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class WMIClassNameAttribute : Attribute
    {
        /// <summary>
        /// Win32 WMI class name of the .NET class.
        /// </summary>
        public string WMIClassName { get; private set; }

        /// <summary>
        /// Constructs a new WMIClassNameAttribute object with the specified WMI class name.
        /// </summary>
        /// <param name="wmiClassName">Win32 WMI class name of the .NET class</param>
        public WMIClassNameAttribute(string wmiClassName)
        {
            WMIClassName = wmiClassName;
        }
    }
}