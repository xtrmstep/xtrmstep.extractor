using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Xtrmstep.Extractor.Core.Model;

namespace Xtrmstep.Extractor.Core
{
    public class ExtractorDbContext : DbContext, IDbContext, IUnitOfWork
    {
        public IDbSet<WebPage> Pages
        {
            get;
            set;
        }

        public void Commit()
        {
            SaveChanges();
        }

        public void Rollback()
        {
            ObjectContext context = ((IObjectContextAdapter) this).ObjectContext;
            foreach (DbEntityEntry change in ChangeTracker.Entries())
            {
                if (change.State == EntityState.Modified)
                {
                    context.Refresh(RefreshMode.StoreWins, change.Entity);
                }
                if (change.State == EntityState.Added)
                {
                    context.Detach(change.Entity);
                }
            }
        }

        #region ctors

        public ExtractorDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public ExtractorDbContext()
            : base("ExtractorDbContext")
        {
        }

        #endregion
    }
}