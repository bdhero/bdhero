using System.ComponentModel;

namespace DotNetUtils.Concurrency
{
    public class SimplePromise : Promise<bool>
    {
        public SimplePromise()
        {
        }

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