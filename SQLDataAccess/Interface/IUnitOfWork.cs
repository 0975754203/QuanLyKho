using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLDataAccess.Interface
{
    public interface IUnitOfWork
    {
        void Dispose();
        void Save();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : class;
    }
}
