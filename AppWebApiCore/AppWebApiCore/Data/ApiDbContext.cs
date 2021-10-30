using AppWebApiCore.Model;
using Microsoft.EntityFrameworkCore;

namespace AppWebApiCore.Data
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext(DbContextOptions opt) : base(opt)
        {

        }

        public DbSet<Fornecedor> Fornecedores { get; set; }

    }
}
