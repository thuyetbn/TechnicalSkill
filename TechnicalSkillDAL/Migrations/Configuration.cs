
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace TechnicalSkill.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TechnicalSkill.DAL.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    } 
}