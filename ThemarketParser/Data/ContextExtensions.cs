using Microsoft.EntityFrameworkCore;

namespace ThemarketParser.Data
{
    public static class ContextExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
        public static void AddOrUpdate(this DbContext ctx, object entity)
        {
            var entry = ctx.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    ctx.Add(entity);
                    break;
                case EntityState.Modified:
                    ctx.Update(entity);
                    break;
                case EntityState.Added:
                    ctx.Add(entity);
                    break;
                case EntityState.Unchanged:
                    //item already in db no need to do anything  
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static void AddOrUpdateRange(this DbContext ctx, IEnumerable<object> entities)
        {
            foreach (var entity in entities)
            {
                //ctx.Attach(entity);
                ctx.ChangeTracker.DetectChanges();
                var entry = ctx.Entry(entity);
                switch (entry.State)
                {
                    case EntityState.Detached:
                        Console.WriteLine("Detached");
                        ctx.Add(entity);
                        break;
                    case EntityState.Modified:
                        Console.WriteLine("Modified");
                        ctx.Update(entity);
                        break;
                    case EntityState.Added:
                        Console.WriteLine("Added");
                        ctx.Add(entity);
                        break;
                    case EntityState.Unchanged:
                        Console.WriteLine("Unchanged");
                        //item already in db no need to do anything  
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
