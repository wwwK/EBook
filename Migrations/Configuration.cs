
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace EBook.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EBook.Models.OracleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    } 
}