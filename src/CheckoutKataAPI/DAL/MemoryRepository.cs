using CheckoutKataAPI.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CheckoutKataAPI.DAL
{
    public class MemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        public T Add(T item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public T Select(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Select(Func<T, bool> condition)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> SelectAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}
