using System.ComponentModel;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    ///     Promise that returns a <see cref="IPromise{TResult}.Result"/> of type <see cref="bool"/> indicating
    ///     whether all of the <see cref="IPromise{TResult}.Work"/> event handlers completed successfully
    ///     (that is, without being canceled or throwing an exception) or not.
    /// </summary>
    public class SimplePromise : Promise<bool>
    {
        /// <summary>
        ///     Constructs a new <see cref="SimplePromise"/> instance that invokes callback event handlers on
        ///     the <b>current thread</b>.
        /// </summary>
        public SimplePromise()
        {
        }

        /// <summary>
        ///     Constructs a new <see cref="SimplePromise"/> instance that invokes callback event handlers on
        ///     the given <paramref name="synchronizingObject"/>'s owner thread.
        /// </summary>
        /// <param name="synchronizingObject">
        ///     Object whose owner thread will be used to invoke UI callbacks.
        /// </param>
        public SimplePromise(ISynchronizeInvoke synchronizingObject)
            : base(synchronizingObject)
        {
        }

        protected override void BeforeDispatchCompletionEvents()
        {
            Result = !IsCancellationRequested && LastException == null;
        }
    }
}