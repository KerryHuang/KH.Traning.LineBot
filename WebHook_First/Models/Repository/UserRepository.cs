using System;
using System.Data.Entity;
using System.Linq;

namespace WebHook_First.Models.Repository
{
    public class UserRepository : IDisposable
    {
        private HangfireEntities entities = new HangfireEntities();

        #region Implement
        public void Create(User entity)
        {
            ArgumentNullException(entity);
            entities.Set<User>().Add(entity);
        }

        public void Delete(User entity)
        {
            ArgumentNullException(entity);
            entities.Set<User>().Remove(entity);
        }

        public void Dispose()
        {
            if (entities != null)
            {
                entities.Dispose();
                entities = null;
            }
        }

        public User Get(int id)
        {
            return entities.Set<User>().Find(id);
        }

        public IQueryable<User> GetAll()
        {
            return entities.Set<User>().AsQueryable();
        }

        public void Update(User entity)
        {
            ArgumentNullException(entity);
            entities.Entry(entity).State = EntityState.Modified;
        }

        public int SaveChanges()
        {
            return entities.SaveChanges();
        }
        #endregion

        #region functions
        private void ArgumentNullException(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity), "entity 不可為空值");
        }
        #endregion
    }
}