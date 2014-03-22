// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

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
