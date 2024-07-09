using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NHOM04.Models;

namespace NHOM04.Data
{
    public class NHOM04Context : DbContext
    {
        public NHOM04Context(DbContextOptions<NHOM04Context> options)
            : base(options)
        {
        }

        public DbSet<NHOM04.Models.Product> Products { get; set; } 

        public DbSet<NHOM04.Models.Cart> Carts { get; set; }

        public DbSet<NHOM04.Models.Account> Accounts { get; set; }

        public DbSet<NHOM04.Models.ProductType> ProductTypes { get; set; }

        public DbSet<NHOM04.Models.Invoice> Invoices { get; set; }

        public DbSet<NHOM04.Models.InvoiceDetail> InvoiceDetails { get; set; }
    }
}
