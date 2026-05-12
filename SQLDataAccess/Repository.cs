using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SQLDataAccess.Interface;

namespace SQLDataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal IDbContext Context;
        internal IDbSet<T> DbSet;

        public Repository(IDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        #region Basic CRUD
        public virtual T FindById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void InsertGraph(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void InsertOrUpdate(T entity)
        {
            DbSet.Attach(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = DbSet.Find(id);
            var objectState = entity as BaseData;
            if (objectState != null)
                objectState.State = EDataState.Deleted;
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
        }

        public virtual RepositoryQuery<T> Query()
        {
            var repositoryGetFluentHelper = new RepositoryQuery<T>(this);
            return repositoryGetFluentHelper;
        }

        public virtual IQueryable<T> Local()
        {
            return DbSet.Local.AsQueryable();
        }
        #endregion

        internal void Update(T entity, List<Expression<Func<T, object>>> includeProperties = null)
        {

        }

        internal IQueryable<T> Get(List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            bool asNoTracking = false,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = DbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filters != null && filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
