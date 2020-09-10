using Demo.DbModel;
using Microsoft.EntityFrameworkCore;

namespace Demo.SqlServer
{
    public class DemoContext : DbContext
    {
        public DemoContext()
        {
        }

        public DemoContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<PhoneBook> PhoneBooks { get; set; }
    }
}
