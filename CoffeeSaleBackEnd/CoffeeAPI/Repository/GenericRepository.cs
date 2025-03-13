using CoffeeAPI.Datas;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoffeeAPI.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        // Lấy danh sách entity với lọc, sắp xếp, bao gồm properties, và phân trang
        IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);

        // Lấy tất cả các entity dưới dạng danh sách
        Task<IEnumerable<T>> GetAllAsync();

        // Lấy entity theo ID (hỗ trợ generic type cho khóa chính)
        Task<T> GetByIdAsync<TKey>(TKey id);

        // Thêm mới một entity
        Task AddAsync(T entity);

        // Cập nhật một entity
        void Update(T entity);

        // Lấy entity đầu tiên thỏa mãn điều kiện
        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        // Xóa một entity (hỗ trợ soft delete nếu entity implements ISoftDelete)
        void Delete(T entity);

        // Tìm các entity thỏa mãn điều kiện
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Thêm mới một entity (phiên bản đồng bộ)
        void Insert(T entity);

        // Lấy entity theo ID (phiên bản đồng bộ)
        T GetByID(object id);

        // Kiểm tra xem entity có tồn tại không
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Đếm số lượng entity thỏa mãn điều kiện
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        // Lưu các thay đổi vào cơ sở dữ liệu
        Task<int> SaveChangesAsync();
    }

    // Interface để hỗ trợ soft delete
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CoffeeSalesContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CoffeeSalesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync<TKey>(TKey id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            return entity;
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Update(entity);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Add(entity);
        }

        public virtual T GetByID(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _dbSet.Find(id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}