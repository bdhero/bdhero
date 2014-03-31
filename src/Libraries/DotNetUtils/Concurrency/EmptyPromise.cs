using System.ComponentModel;

namespace DotNetUtils.Concurrency
{
    /// <summary>
    ///     Promise whose <see cref="IPromise{TResult}.Result"/> value is irrelevant.
    /// </summary>
    public class EmptyPromise : Promise<Nil>
    {
        /// <summary>
        ///     Constructs a new <see cref="EmptyPromise"/> instance that invokes callback event handlers on
        ///     the <b>current thread</b>.
        /// </summary>
        public EmptyPromise()
        {
        }

        /// <summary>
        ///     Constructs a new <see cref="EmptyPromise"/> instance that invokes callback event handlers on
        ///     the given <paramref name="synchronizingObject"/>'s owner thread.
        /// </summary>
        /// <param name="synchronizingObject">
        ///     Object whose owner thread will be used to invoke UI callbacks.
        /// </param>
        public EmptyPromise(ISynchronizeInvoke synchronizingObject)
            : base(synchronizingObject)
        {
        }
    }
}