using System;
using System.ComponentModel;

namespace DotNetUtils.Concurrency
{
    public class UIInvoker : IInvoker
    {
        private readonly ISynchronizeInvoke _uiContext;

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