using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YoFi.Core;
using YoFi.Core.Models;

namespace YoFi.Vue.Data
{
    public class ApplicationDbContext : DbContext, IDataContext
    {
        #region Entity Sets

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<Split> Splits { get; set; }
        public DbSet<BudgetTx> BudgetTxs { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        #endregion

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Split>().ToTable("Split");

            builder.Entity<Transaction>().HasIndex(p => new { p.Timestamp, p.Hidden, p.Category });

            // https://stackoverflow.com/questions/60503553/ef-core-linq-to-sqlite-could-not-be-translated-works-on-sql-server
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                    foreach (var property in properties)
                    {
                        builder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }            
        }


        #region CRUD Entity Accessors

        IQueryable<T> IDataContext.Get<T>() where T : class
        {
            return Set<T>();
        }

        IQueryable<TEntity> IDataContext.GetIncluding<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
            => base.Set<TEntity>().Include(navigationPropertyPath);

        void IDataContext.Add(object item)
        {
            base.Add(item);
        }

        void IDataContext.Update(object item)
        {
            base.Update(item);
        }

        void IDataContext.Remove(object item)
        {
            base.Remove(item);
        }

        Task IDataContext.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        #endregion

        #region Async Queries

        Task<List<T>> IDataContext.ToListNoTrackingAsync<T>(IQueryable<T> query)
        {
            return query.AsNoTracking().ToListAsync();
        }

        Task<int> IDataContext.CountAsync<T>(IQueryable<T> query)
        {
            return query.CountAsync();
        }

        Task<bool> IDataContext.AnyAsync<T>(IQueryable<T> query)
        {
            return query.AnyAsync();
        }

        #endregion

        #region Bulk Operations

        async Task<int> IDataContext.ClearAsync<T>() where T : class
        {
            var result = await Set<T>().CountAsync();
            base.Remove(Set<T>());
            await base.SaveChangesAsync();

            return result;
        }

        async Task IDataContext.BulkInsertAsync<T>(IList<T> items)
        {
            base.Set<T>().AddRange(items);
            await base.SaveChangesAsync();
        }

        async Task IDataContext.BulkDeleteAsync<T>(IQueryable<T> items)
        {
            base.Set<T>().RemoveRange(items);
            await base.SaveChangesAsync();
        }

        async Task IDataContext.BulkUpdateAsync<T>(IQueryable<T> items, T newvalues, List<string> columns)
        {
            // We support ONLY a very limited range of possibilities, which is where this
            // method is actually called.
            if (typeof(T) != typeof(Transaction))
                throw new NotImplementedException("Bulk Update on in-memory DB is only implemented for transactions");

            var txvalues = newvalues as Transaction;
            var txitems = items as IQueryable<Transaction>;
            var txlist = await txitems.ToListAsync();
            foreach (var item in txlist)
            {
                if (columns.Contains("Imported"))
                    item.Imported = txvalues.Imported;
                if (columns.Contains("Hidden"))
                    item.Hidden = txvalues.Hidden;
                if (columns.Contains("Selected"))
                    item.Selected = txvalues.Selected;
            }
            UpdateRange(txlist);

            await SaveChangesAsync();
        }

        #endregion

    }

}
