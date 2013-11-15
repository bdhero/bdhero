using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetUtils.TaskUtils
{
    /// <summary>
    /// Concrete implementation of <c>IThreadInvoker</c>.
    /// </summary>
    public class ThreadInvoker : IThreadInvoker
    {
        private readonly TaskScheduler _callbackThread;

        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Constructs a thread invoker that executes actions in the context of the specified task scheduler, passing them 
        /// </summary>
        /// <param name="callbackThread">Thread context in which actions will run</param>
        /// <param name="cancellationToken"></param>
        public ThreadInvoker(TaskScheduler callbackThread, CancellationToken cancellationToken)
        {
            _callbackThread = callbackThread;
            _cancellationToken = cancellationToken;
        }

        public void InvokeOnUIThreadSync(ThreadAction action)
        {
            StartTask(action, _cancellationToken).Wait();
        }

        public void InvokeOnUIThreadAsync(ThreadAction action)
        {
            StartTask(action, _cancellationToken);
        }

        private Task StartTask(ThreadAction action, CancellationToken cancellationToken)
        {
            var token = !cancellationToken.IsCancellationRequested ? cancellationToken : CancellationToken.None;
            return Task.Factory.StartNew(() => action(token), token, TaskCreationOptions.None, _callbackThread);
        }
    }
}
