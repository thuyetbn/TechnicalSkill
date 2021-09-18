using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechnicalSkill.DAL;

namespace TechnicalSkill.BLL
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly DatabaseContext cnn;

        private readonly DbSet<T> tbl;
        public Repository()
        {
            cnn = new DatabaseContext();
            tbl = cnn.Set<T>();
        }
        public bool Add(T e)
        {
            try
            {
                tbl.Add(e);
                cnn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckDuplicate(Expression<Func<T, bool>> predicate)
        {
            return tbl.AsNoTracking().Any(predicate);
        }

        public bool Delete(object id)
        {
            try
            {
                var entity = Get(id);
                tbl.Remove(entity);
                cnn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(T entity)
        {

            try
            {
                tbl.Remove(entity);
                cnn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> Get()
        {
            return tbl.AsEnumerable();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return tbl.Where(predicate).AsEnumerable();
        }

        public T Get(object id)
        {
            return tbl.Find(id);
        }

        public bool Update(T entity)
        {
            try
            {
                cnn.Entry(entity).State = EntityState.Modified;
                cnn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        bool IRepository<T>.AddRange(List<T> e)
        {
            try
            {
                tbl.AddRange(e);
                cnn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
