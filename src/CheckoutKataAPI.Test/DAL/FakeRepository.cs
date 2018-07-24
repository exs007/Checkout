using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Test.DAL
{
    internal class FakeRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDictionary<int, T> _storage = new Dictionary<int, T>();
        private int _seed;

        public T Select(int id)
        {
            T toReturn = null;
            _storage.TryGetValue(id, out toReturn);

            return toReturn;
        }

        public ICollection<T> Select(Func<T, bool> condition)
        {
            return _storage.Select(p => p.Value).Where(p => condition(p)).ToList();
        }

        public ICollection<T> SelectAll()
        {
            var toReturn = _storage.Select(p=>p.Value).ToList();

            return toReturn;
        }

        public T Add(T item)
        {
            _seed++;
            item.Id = _seed;
            _storage.TryAdd(item.Id, item);

            return item;
        }

        public bool Update(T item)
        {
            T storeItem = null;
            _storage.TryGetValue(item.Id, out storeItem);

            if (storeItem!=null)
            {
                _storage[item.Id] = item;
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            return _storage.Remove(id);
        }

        public void DeleteAll()
        {
            _storage.Clear();
        }
    }
}
