using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace SHOPTV.Data
{
    public class SHOPTVContext: DbContext
    {
        public SHOPTVContext(DbContextOptions<SHOPTVContext> options)
            : base(options)
        {
        }

        public DbSet<SHOPTV.Models.Product> Products { get; set; } 

        public DbSet<SHOPTV.Models.Cart> Carts { get; set; }

        public DbSet<SHOPTV.Models.Account> Accounts { get; set; }

        public DbSet<SHOPTV.Models.ProductType> ProductTypes { get; set; }

        public DbSet<SHOPTV.Models.Invoice> Invoices { get; set; }

        public DbSet<SHOPTV.Models.InvoiceDetail> InvoiceDetails { get; set; }
    }
}
