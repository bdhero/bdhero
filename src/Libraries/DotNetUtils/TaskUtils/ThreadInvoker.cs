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
