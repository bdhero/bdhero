namespace WindowsOSUtils.WMI
{
    /// <summary>
    /// Asynchronously handles WMI events related to the WMI class specified by <typeparamref name="T"/>.
    /// </summary>
    /// <param name="wmiObjectInstance">Instance of a .NET object that represents the Win32 WMI class object returned by the event</param>
    /// <typeparam name="T">.NET type that represents the Win32 WMI class</typeparam>
    public delegate void WMIEventHandler<T>(T wmiObjectInstance);
}