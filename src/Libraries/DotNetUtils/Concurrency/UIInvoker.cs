using System;
using System.ComponentModel;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    ///     Implementation of <see cref="IInvoker"/> that invokes actions on a Windows Forms control's owner thread.
    /// </summary>
    public class UIInvoker : IInvoker
    {
        private readonly ISynchronizeInvoke _uiContext;

        /// <summary>
        ///     Constructs a new <see cref="UIInvoker"/> instance that invokes actions on the given
        ///     <paramref name="uiContext"/>'s owner thread.
        /// </summary>
        /// <param name="uiContext"></param>
        public UIInvoker(ISynchronizeInvoke uiContext)
        {
            _uiContext = uiContext;
        }

        public void InvokeSync(Action action)
        {
            _uiContext.Invoke(action, new object[0]);
        }

        public void InvokeAsync(Action action)
        {
            _uiContext.BeginInvoke(action, new object[0]);
        }
    }
}