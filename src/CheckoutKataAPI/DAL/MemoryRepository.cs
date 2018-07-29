using CheckoutKataAPI.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CheckoutKataAPI.DAL
{
    /// <summary>
    /// Storing is based on a dictionary with locks for providing thread-safe access
    /// </summary>
    /// <typeparam name="T">BaseEntity</typeparam>
    public class MemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        //TODO: return a copy of an object
        private readonly IDictionary<int, T> _storage = new Dictionary<int, T>();
        private int _seed;
        private readonly IDictionary<int, object> _recordLocks = new Dictionary<int, object>();
        private readonly object _storageLock = new object();

        public T Add(T item)
        {
            lock (_storageLock)
            {
                _seed++;
                item.Id = _seed;
                item.StatusCode = RecordStatusCode.Active;
                _storage.Add(item.Id, item);
            }

            return item;
        }

        public T Select(int id)
        {
            lock (_storageLock)
            {
                T toReturn = null;
                _storage.TryGetValue(id, out toReturn);

                return IsActive(toReturn) ? toReturn : null;
            }
        }

        public ICollection<T> Select(Func<T, bool> condition)
        {
            lock (_storageLock)
            {
                var toReturn = _storage.Select(p => p.Value).Where(p => IsActive(p) && condition(p)).ToList();

                return toReturn;
            }
        }

        public ICollection<T> SelectAll()
        {
            lock (_storageLock)
            {
                var toReturn = _storage.Select(p => p.Value).Where(p => IsActive(p)).ToList();

                return toReturn;
            }
        }

        public bool Update(T item)
        {
            if (item.Id == 0)
                return false;
            
            var recordLock = GetRecordLock(item.Id);
            lock (recordLock)
            {
                T storeItem = null;
                lock (_storageLock)
                {
                    _storage.TryGetValue(item.Id, out storeItem);

                    if (IsActive(storeItem))
                    {
                        _storage[item.Id] = item;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Delete(int id)
        {
            if (id == 0)
                return false;

            var recordLock = GetRecordLock(id);
            lock (recordLock)
            {
                T item = null;
                lock (_storageLock)
                {
                    _storage.TryGetValue(id, out item);
                }

                if (IsActive(item))
                {
                    item.StatusCode = RecordStatusCode.Deleted;
                    return true;
                }
            }

            return false;
        }

        public void DeleteAll()
        {
            lock (_storageLock)
            {
                foreach (var item in _storage.Select(p => p.Value).Where(p => IsActive(p)))
                {
                    item.StatusCode = RecordStatusCode.Deleted;
                }
            }
        }

        private bool IsActive(T item)
        {
            return item!=null && item.StatusCode != RecordStatusCode.Deleted;
        }

        private object GetRecordLock(int id)
        {
            object toReturn = null;
            lock (_storageLock)
            {
                if (!_recordLocks.TryGetValue(id, out toReturn))
                {
                    toReturn = new object();
                    _recordLocks.Add(id, toReturn);
                }
            }

            return toReturn;
        }
    }
}
