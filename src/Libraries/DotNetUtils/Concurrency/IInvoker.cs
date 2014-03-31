using System;

namespace DotNetUtils.Concurrency
{
    public interface IInvoker
    {
        void InvokeSync(Action action);
        void InvokeAsync(Action action);
    }
}