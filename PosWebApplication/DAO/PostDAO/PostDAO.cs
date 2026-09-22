using Microsoft.EntityFrameworkCore;
using POS_Retails.Entity;
using PosWebApplication.Entity;
using PosWebApplication.DAO.PostDAO;
using PosWebApplication.Entity;

namespace PosWebApplication.DAO.PostDAO
{
    public class PostDAO : IPostDAO
    {
        private readonly pos_saas_entities _context;
        public PostDAO(pos_saas_entities context)
        {
            _context = context;
        }

        public List<post> GetAll()
        {
            return _context.posts
                .Include(x => x.user)
                .ToList();
        }

        public List<post> GetByu_id(int u_id)
        {
            return _context.posts
                .Where(x => x.created_by == u_id)
                .ToList();
        }

        public post? GetById(int id)
        {
            return _context.posts
                .Include(x => x.user)
                .FirstOrDefault(x => x.p_id == id);
        }

        public void Create(post post)
        {
            _context.posts.Add(post);

            _context.SaveChanges();
        }

        public void Update(post post)
        {
            _context.posts.Update(post);

            _context.SaveChanges();
        }

        public void Delete(post post)
        {
            _context.posts.Remove(post);

            _context.SaveChanges();
        }
    }
}