using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    /// Invoked on the <strong>UI</strong> thread before the <see cref="TimerPromise"/> begins execution in the background.
    /// </summary>
    /// <param name="promise">Promise that triggered the event.</param>
    public delegate void BeforePromiseHandler(IPromise promise);

    /// <summary>
    /// Invoked on the <strong>background</strong> thread as the main unit of work.
    /// </summary>
    /// <param name="promise">Promise that triggered the event.</param>
    public delegate void DoWorkPromiseHandler(IPromise promise);

    /// <summary>
    /// Invoked on the <strong>UI</strong> thread when the <see cref="TimerPromise"/> has been <em>requested</em> to
    /// cancel execution, but is still running in the background.
    /// </summary>
    /// <param name="promise">Promise that triggered the event.</param>
    public delegate void CancellationRequestedPromiseHandler(IPromise promise);

    /// <summary>
    /// Invoked on the <strong>UI</strong> thread after the <see cref="TimerPromise"/> has been canceled and the
    /// background thread has exited.
    /// </summary>
    /// <param name="promise">Promise that triggered the event.</param>
    public delegate void CanceledPromiseHandler(IPromise promise);

    /// <summary>
    /// Invoked on the <strong>UI</strong> thread after the background thread completes successfully
    /// (that is, without being canceled or throwing an exception).
    /// </summary>
    /// <param name="promise">Promise that triggered the event.</param>
    public delegate void SuccessPromiseHandler(IPromise promise);

    /// <summary>
    /// Invoked on the <strong>UI</strong> thread when the background thread terminates due to an exception being thrown.
    /// </summary>
    /// <param name="promise">Promise that triggered the event.</param>
    public delegate void FailurePromiseHandler(IPromise promise);

    /// <summary>
    /// Invoked on the <strong>UI</strong> thread after the background thread has exited, regardless of the
    /// background thread's state.  Always invoked <em>after</em> <see cref="CanceledPromiseHandler"/>,
    /// <see cref="SuccessPromiseHandler"/>, and <see cref="FailurePromiseHandler"/> handlers.
    /// </summary>
    /// <param name="promise">Promise that triggered the event.</param>
    public delegate void AlwaysPromiseHandler(IPromise promise);

    /// <summary>
    /// Invoked on the <strong>UI</strong> thread whenever the background thread updates its progress.
    /// </summary>
    /// <typeparam name="TState">Datatype of the progress update to listen for.</typeparam>
    /// <param name="promise">Promise that triggered the event.</param>
    /// <param name="state">The current state of the background thread.</param>
    public delegate void ProgressPromiseHandler<TState>(IPromise promise, TState state);
}
