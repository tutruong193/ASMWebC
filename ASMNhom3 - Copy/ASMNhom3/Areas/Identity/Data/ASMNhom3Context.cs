using ASMNhom3.Areas.Identity.Data;
using ASMNhom3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASMNhom3.Areas.Identity.Data;

public class ASMNhom3Context : IdentityDbContext<ManageUser>
{
    public ASMNhom3Context(DbContextOptions<ASMNhom3Context> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categorys { get; set; }
    public DbSet<Cart> Carts { get; set; }
}
public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ManageUser>
{
    public void Configure(EntityTypeBuilder<ManageUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(255);
    }
}
