using TBay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TBay.Data
{
    public class TBayContext : IdentityDbContext<AppUser>
    {
        public TBayContext(DbContextOptions<TBayContext> options) : base(options)
        {
        }

        public DbSet<Item> Item { get; set; }
        public DbSet<Store> Store{ get; set; }
        public DbSet<Designer> Designer { get; set; }
        public DbSet<AppUser> AppUser{ get; set; }
        public DbSet<User> User{ get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable("Item")
              .HasOne(b => b.Designer)
            .WithMany(i => i.Item)
            .HasForeignKey(b => b.Designerid).OnDelete(DeleteBehavior.NoAction);
            
             modelBuilder.Entity<Designer>().ToTable("Designer");
             

            modelBuilder.Entity<Store>().ToTable("Store")
              .HasOne(b => b.Item)
            .WithMany(i => i.Store)
            .HasForeignKey(b => b.ItemId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().ToTable("User")
            .Property(e => e.UserID)
            .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}