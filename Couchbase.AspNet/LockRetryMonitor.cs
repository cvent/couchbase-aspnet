using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Couchbase.AspNet
{
    public sealed class LockRetryMonitor
    {
        private readonly int _maximumRetries;
        private readonly ConcurrentDictionary<string, int> _sessionIdDictionary;

        public LockRetryMonitor(int maximumRetries)
        {
            // disable the monitor unless there's a valid limit
            if (maximumRetries > 0) {
                _sessionIdDictionary = new ConcurrentDictionary<string, int>();
                _maximumRetries = maximumRetries;                
            }
        }

        public int Increment(
            string id)
        {
            if (null == _sessionIdDictionary) {
                return 0;
            }

            int retryCount;
            if (_sessionIdDictionary.TryGetValue(id, out retryCount)) {
                if (retryCount == _maximumRetries) {
                    throw new Exception("Maximum retries reached while waiting for locked object");
                }
            }

            return _sessionIdDictionary.AddOrUpdate(id,
                1,
                (s,
                    i) => Interlocked.Increment(ref i));
        }

        public bool Remove(
            string id)
        {
            if (null == _sessionIdDictionary)
            {
                return false;
            }

            int retryCount;
            return _sessionIdDictionary.TryRemove(id, out retryCount);
        }
    }
}