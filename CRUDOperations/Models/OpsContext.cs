using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CRUDOperations.Models
{
    public class OpsContext : DbContext
    {
        public OpsContext(DbContextOptions<OpsContext> options) : base(options) 
        {
            
        }
        public DbSet<Ops> Ops { get; set; }
    }
}
