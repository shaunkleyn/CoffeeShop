using Microsoft.EntityFrameworkCore;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoffeeShop.Models.Entities
{
    public partial class CoffeeShopContext : DbContext
    {
        public CoffeeShopContext()
        {
        }

        public CoffeeShopContext(DbContextOptions<CoffeeShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DatabaseConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>(entity =>
            {
                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Sales>(entity =>
            {
                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Sales_Clients");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_Products");
            });

            modelBuilder.Entity<Products>().HasData(
               new Products { Id = 1, Code = "LATTE", Name = "Latte", Price = 29.95M },
               new Products { Id = 2, Code = "ICELAT", Name = "Iced Latte", Price = 32.95M },
               new Products { Id = 3, Code = "ESPRESS", Name = "Espresso", Price = 28.50M },
               new Products { Id = 4, Code = "VALECOF", Name = "Vale Coffee", Price = 22.50M },
               new Products { Id = 5, Code = "CAPPUC", Name = "Cappuccino", Price = 22.50M },
               new Products { Id = 6, Code = "ICECAPPUC", Name = "Iced Cappuccino", Price = 22.50M },
               new Products { Id = 7, Code = "AFRCOFF", Name = "African Coffee", Price = 22.50M }

           );

            modelBuilder.Entity<Clients>().HasData(
                new Clients { Id = 1, Name = "Manya", Surname = "Bertome", EmailAddress = "mbertome0@boston.com" },
                new Clients { Id = 2, Name = "Crosby", Surname = "Nesfield", EmailAddress = "cnesfield1@shinystat.com" },
                new Clients { Id = 3, Name = "Dredi", Surname = "Giacomuzzi", EmailAddress = "dgiacomuzzi2@exblog.jp" },
                new Clients { Id = 4, Name = "Thom", Surname = "Lucy", EmailAddress = "tlucy3@clickbank.net" },
                new Clients { Id = 5, Name = "Terrie", Surname = "Yarn", EmailAddress = "tyarn4@yahoo.com" },
                new Clients { Id = 6, Name = "Dinnie", Surname = "Conboy", EmailAddress = "dconboy5@paginegialle.it" }
            );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
