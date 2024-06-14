using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Locks
{
    public class LockAsync
    {
        private static string _lastClearDay;
        private static readonly Dictionary<String, AsyncLock> _lockDictionary;
        private static readonly object _lockDictionaryObj;
        static LockAsync()
        {
            _lockDictionary = new Dictionary<string, AsyncLock>();
            _lockDictionaryObj = new AsyncLock();
        }
        public static AsyncLock LockOnValue(string lockKey)
        {
            lock (_lockDictionaryObj)
            {
                AsyncLock lockObj = null;
                if (!_lockDictionary.TryGetValue(lockKey, out lockObj))
                {
                    clearLockDictionary();
                    lockObj = new AsyncLock();
                    _lockDictionary.Add(lockKey, lockObj);
                }

                return lockObj;
            }
        }

        private static void clearLockDictionary()
        {
            bool isWednesday1Oclock = _lastClearDay != DateTime.Today.ToShortDateString() && (int)DateTime.Now.DayOfWeek == 2 && DateTime.Now.Hour == 1;

            if (isWednesday1Oclock)
            {
                _lastClearDay = DateTime.Today.ToShortDateString();
                _lockDictionary.Clear();
            }
        }
    }
}
