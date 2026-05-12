using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SQLDataAccess
{
    public sealed class RepositoryQuery<T> where T : class
    {
        private readonly List<Expression<Func<T, object>>> _includeProperties;

        private readonly Repository<T> _repository;
        private List<Expression<Func<T, bool>>> _filters;
        private Func<IQueryable<T>, IOrderedQueryable<T>> _orderByQuerable;
        private int? _page;
        private int? _pageSize;
        private bool _asNoTracking = false;

        public RepositoryQuery(Repository<T> repository)
        {
            this._repository = repository;
            _includeProperties = new List<Expression<Func<T, object>>>();
        }

        public RepositoryQuery<T> Filter(Expression<Func<T, bool>> filter)
        {
            if (_filters == null)
                _filters = new List<Expression<Func<T, bool>>>();

            _filters.Add(filter);
            return this;
        }

        public RepositoryQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        public RepositoryQuery<T> Include(Expression<Func<T, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }

        /// <summary>
        /// Like as no tracking of entity framework
        /// </summary>
        /// <returns></returns>
        public RepositoryQuery<T> AsNoTracking()
        {
            _asNoTracking = true;
            return this;
        }

        public IEnumerable<T> GetPage(int page,
            int pageSize, out int totalCount)
        {
            _page = page;
            _pageSize = pageSize;
            totalCount = _repository.Get(_filters).Count();

            return _repository.Get(_filters, _orderByQuerable, _includeProperties,
                _asNoTracking, _page, _pageSize);
        }

        public IEnumerable<T> Get()
        {
            return _repository.Get(_filters,
                _orderByQuerable, _includeProperties, _asNoTracking,
                _page, _pageSize);
        }

        public T FirstOrDefault()
        {
            return this.Get().FirstOrDefault();
        }
    }
}
