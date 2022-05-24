using Microsoft.EntityFrameworkCore;
using ThemarketParser.Models;

namespace ThemarketParser.Data
{
    public class ThemarketDBContext : DbContext
    {
        public ThemarketDBContext(DbContextOptions<ThemarketDBContext> options) : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<SexCategory> SexCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ConcreteCategory> ConcreteCategories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Size>  Sizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().Property(e => e.name).IsUnicode(false);
            // modelBuilder.Entity<Category>().Property(e => e.name).IsUnicode(false);
            modelBuilder.Entity<CategoryAbstract>().HasMany(e => e.childCategories).WithOne(e => e.parentCategory).HasForeignKey(e => e.parentCategoryId);
            modelBuilder.Entity<CategoryAbstract>().Property(e => e.name).IsUnicode(false);
            modelBuilder.Entity<SexCategory>().HasMany(e => e.items).WithOne(e => e.sexCategory).HasForeignKey(e => e.sexCategoryId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Category>().HasMany(e => e.items).WithOne(e => e.category).HasForeignKey(e => e.categoryId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ConcreteCategory>().HasMany(e => e.items).WithOne(e => e.concreteCategory).HasForeignKey(e => e.concreteCategoryId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Category>().HasMany(e => e.size).WithOne(e => e.category).HasForeignKey(e => e.categoryId);

            modelBuilder.Entity<City>().Property(e => e.title).IsUnicode(false);
            modelBuilder.Entity<City>().HasMany(e => e.items).WithOne(e => e.city).HasForeignKey(e => e.cityId);
            modelBuilder.Entity<Condition>().Property(e => e.name).IsUnicode(false);
            modelBuilder.Entity<Condition>().HasMany(e => e.items).WithOne(e => e.condition).HasForeignKey(e => e.conditionId);
            modelBuilder.Entity<Size>().HasMany(e => e.items).WithOne(e => e.size).HasForeignKey(e => e.sizeId);
            modelBuilder.Entity<Item>().HasMany(e => e.images).WithOne(e => e.item).HasForeignKey(e => e.itemId);
            modelBuilder.Entity<Item>().Property(e => e.description).IsUnicode(false);
            modelBuilder.Entity<Item>().Property(e => e.isModified).HasDefaultValue(false);
        }
    }
}
