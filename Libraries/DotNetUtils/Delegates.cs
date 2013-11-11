using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetUtils.Annotations;

namespace DotNetUtils
{
    public delegate void ExceptionEventHandler(ExceptionEventArgs args);

    public class ExceptionEventArgs
    {
        [CanBeNull]
        public readonly Exception Exception;

        public ExceptionEventArgs([CanBeNull] Exception exception = null)
        {
            Exception = exception;
        }
    }
}
