using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNetUtils.TaskUtils
{
    public delegate void TaskStartedEventHandler();

    public delegate void TaskWorkHandler(IThreadInvoker threadInvoker, CancellationToken cancellationToken);

    public delegate void TaskSucceededEventHandler();

    /// <summary>
    /// Invoked when a task completes, regardless of whether the task succeeded or failed.
    /// Invoked <em>after</em> the <see cref="TaskSucceededEventHandler"/> and <see cref="ExceptionEventHandler"/> events.
    /// </summary>
    public delegate void TaskCompletedEventHandler();

    public delegate void ThreadAction(CancellationToken cancellationToken);
}
