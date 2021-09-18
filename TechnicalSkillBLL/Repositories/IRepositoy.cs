using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSkill.BLL
{
    public interface IRepository<T> where T : class, new()
    {
        IEnumerable<T> Get();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        bool CheckDuplicate(Expression<Func<T, bool>> predicate);
        T Get(object id);
        bool Add(T e);

        bool AddRange(List<T> e);
        bool Update(T entity);
        bool Delete(object id);
        bool Delete(T entity);
    }
}
