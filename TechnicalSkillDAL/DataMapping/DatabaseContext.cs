using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSkill.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=ConnectStringDb")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>("ConnectStringDb"));
        }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
