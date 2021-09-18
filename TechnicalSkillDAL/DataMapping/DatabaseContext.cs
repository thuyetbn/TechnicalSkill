using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSkill.DAL.Migrations;

namespace TechnicalSkill.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=ConnectStringDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>());
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post>()
        //        .HasIndex(b => b.SourceID)
        //        .IsUnique();
        //}
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        
    }
}
