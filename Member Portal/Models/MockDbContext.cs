using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberPortal.Models
{
    public class MockDbContext:DbContext
    {
       public MockDbContext(DbContextOptions<MockDbContext> options) : base(options) { }

       public DbSet<MockDatabase> MockDatabases { get; set; }
       
    }
}
