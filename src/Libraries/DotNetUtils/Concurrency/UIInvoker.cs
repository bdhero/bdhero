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

        /// <summary>
        ///     Invokes the given <paramref name="action"/> synchronously on the underlying UI context.
        /// </summary>
        /// <param name="action"></param>
        public void InvokeSync(Action action)
        {
            if (_uiContext.InvokeRequired)
                _uiContext.Invoke(action, new object[0]);
            else
                action();
        }

        /// <summary>
        ///     Invokes the given <paramref name="action"/> asynchronously on the underlying UI context.
        /// </summary>
        /// <param name="action"></param>
        public void InvokeAsync(Action action)
        {
            if (_uiContext.InvokeRequired)
                _uiContext.BeginInvoke(action, new object[0]);
            else
                action();
        }
    }
}