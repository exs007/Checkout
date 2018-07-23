using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;

namespace CheckoutKataAPI.DAL
{
    public interface IRepository<T> where T: BaseEntity
    {
        T Select(int id);

        ICollection<T> Select(Func<T, bool> condition);

        ICollection<T> SelectAll();

        T Add(T item);

        bool Update(T item);

        bool Delete(int id);

        void DeleteAll();
    }
}
