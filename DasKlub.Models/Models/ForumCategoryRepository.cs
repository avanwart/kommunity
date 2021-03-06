using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DasKlub.Models.Forum;
using DasKlub.Web.Models.Models;

namespace DasKlub.Models.Models
{
    public class ForumCategoryRepository : IForumCategoryRepository
    {
        private readonly DasKlubModelsContext _context = new DasKlubModelsContext();

        public IQueryable<ForumCategory> All
        {
            get { return _context.ForumCategory; }
        }

        public IQueryable<ForumCategory> AllIncluding(params Expression<Func<ForumCategory, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<ForumCategory, object>>,
                IQueryable<ForumCategory>>(_context.ForumCategory,
                    (current, includeProperty) => current.Include(includeProperty));
        }

        public ForumCategory Find(string id)
        {
            return _context.ForumCategory.First(x => x.Key == id);
        }

        public void InsertOrUpdate(ForumCategory forumcategory)
        {
            if (forumcategory.ForumCategoryID == default(int))
            {
                // New entity
                _context.ForumCategory.Add(forumcategory);
            }
            else
            {
                // Existing entity
                _context.Entry(forumcategory).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            ForumCategory forumcategory = _context.ForumCategory.Find(id);
            _context.ForumCategory.Remove(forumcategory);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public interface IForumCategoryRepository : IDisposable
    {
        IQueryable<ForumCategory> All { get; }
        IQueryable<ForumCategory> AllIncluding(params Expression<Func<ForumCategory, object>>[] includeProperties);
        ForumCategory Find(string id);
        void InsertOrUpdate(ForumCategory forumcategory);
        void Delete(int id);
        void Save();
    }
}