using System.ComponentModel;

namespace DotNetUtils.Concurrency
{
    public class EmptyPromise : Promise<Nil>
    {
        public EmptyPromise()
        {
        }

        public EmptyPromise(ISynchronizeInvoke synchronizingObject)
            : base(synchronizingObject)
        {
        }
    }
}