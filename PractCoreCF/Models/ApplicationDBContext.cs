using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<CountryMaster> CountryMaster { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }
        public DbSet<TokenMaster> TokenMaster { get; set; }
        public DbSet<UserMaster> UserMaster { get; set; }
        public DbSet<UserImages> UserImages { get; set; }
    }
}
