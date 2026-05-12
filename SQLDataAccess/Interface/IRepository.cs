using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataAccess.Interface
{
    public interface IRepository<T> where T : class
    {
        T FindById(object id);
        void InsertGraph(T entity);
        void Delete(object id);
        void Delete(T entity);
        void InsertOrUpdate(T entity);
        RepositoryQuery<T> Query();
        IQueryable<T> Local();
    }
}
