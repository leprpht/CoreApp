using AppCore.Entities;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Context;

public class ContactsDbContext : IdentityDbContext<CrmUser, CrmRole, string>
{
    public DbSet<Person> People { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=crm.db");
    }

    public ContactsDbContext() { }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ── Users & Roles ────────────────────────────────────────────────
        builder.Entity<CrmUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Status).HasConversion<string>();
        });

        builder.Entity<CrmRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
        });

        // ── Seed roles ───────────────────────────────────────────────────
        var roles = Enum.GetValues<UserRole>().Select(r => new CrmRole
        {
            Id = r switch
            {
                UserRole.Administrator  => "11111111-1111-1111-1111-111111111111",
                UserRole.SalesManager   => "22222222-2222-2222-2222-222222222222",
                UserRole.Salesperson    => "33333333-3333-3333-3333-333333333333",
                UserRole.SupportAgent   => "44444444-4444-4444-4444-444444444444",
                UserRole.ReadOnly       => "55555555-5555-5555-5555-555555555555",
                _ => Guid.NewGuid().ToString()
            },
            Name = r.ToString(),
            NormalizedName = r.ToString().ToUpper()
        }).ToList();

        builder.Entity<CrmRole>().HasData(roles);

        // ── Seed users ───────────────────────────────────────────────────
        var hasher = new PasswordHasher<CrmUser>();

        var adminUser = new CrmUser
        {
            Id = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA",
            UserName = "admin@crm.pl",
            NormalizedUserName = "ADMIN@CRM.PL",
            Email = "admin@crm.pl",
            NormalizedEmail = "ADMIN@CRM.PL",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "System",
            FullName = "Admin System",
            Department = "IT",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2024, 1, 1),
            SecurityStamp = "STATIC-STAMP-ADMIN"
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@1234");

        var salesUser = new CrmUser
        {
            Id = "BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB",
            UserName = "sales@crm.pl",
            NormalizedUserName = "SALES@CRM.PL",
            Email = "sales@crm.pl",
            NormalizedEmail = "SALES@CRM.PL",
            EmailConfirmed = true,
            FirstName = "Jan",
            LastName = "Sprzedawca",
            FullName = "Jan Sprzedawca",
            Department = "Sales",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2024, 1, 1),
            SecurityStamp = "STATIC-STAMP-SALES"
        };
        salesUser.PasswordHash = hasher.HashPassword(salesUser, "Sales@1234");

        builder.Entity<CrmUser>().HasData(adminUser, salesUser);

        // ── Assign roles to users ────────────────────────────────────────
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = "11111111-1111-1111-1111-111111111111"
            },
            new IdentityUserRole<string>
            {
                UserId = salesUser.Id,
                RoleId = "33333333-3333-3333-3333-333333333333"
            }
        );

        // ── TPH dla Contact ──────────────────────────────────────────────
        builder.Entity<Contact>()
            .HasDiscriminator<string>("ContactType")
            .HasValue<Person>("Person")
            .HasValue<Company>("Company")
            .HasValue<Organization>("Organization");

        builder.Entity<Contact>(entity =>
        {
            entity.Property(p => p.Email).HasMaxLength(200);
            entity.Property(p => p.Phone).HasMaxLength(20);
            entity.Property(p => p.Status).HasConversion<string>();
        });

        builder.Entity<Person>(entity =>
        {
            entity.Property(p => p.BirthDate).HasColumnType("date");
            entity.Property(p => p.Gender).HasConversion<string>();
            entity.Property(p => p.FirstName).HasMaxLength(100);
            entity.Property(p => p.LastName).HasMaxLength(100);
            entity.Property(p => p.Position).HasMaxLength(100);
        });

        builder.Entity<Person>()
            .HasOne(p => p.Employer)
            .WithMany(e => e.Employees);

        builder.Entity<Organization>()
            .HasMany(o => o.Members)
            .WithOne(p => p.Organization);

        builder.Entity<Company>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(200);
            entity.Property(c => c.Industry).HasMaxLength(100);
            entity.Property(c => c.Website).HasMaxLength(300);
        });

        // ── Seed data ────────────────────────────────────────────────────
        builder.Entity<Company>().HasData(new Company
        {
            Id = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271"),
            Name = "WSEI",
            Industry = "edukacja",
            Phone = "123567123",
            Email = "biuro@wsei.edu.pl",
            Website = "https://wsei.edu.pl",
        });

        builder.Entity<Person>().HasData(
            new
            {
                Id = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                FirstName = "Adam",
                LastName = "Nowak",
                Gender = Gender.Male,
                Status = ContactStatus.Active,
                Email = "adam@wsei.edu.pl",
                Phone = "123456789",
                BirthDate = DateTime.Parse("2001-01-11"),
                Position = "Programista",
                CreatedAt = new DateTime(2024, 1, 1),
                UpdatedAt = (DateTime?)new DateTime(2024, 1, 1)
            },
            new
            {
                Id = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466"),
                FirstName = "Ewa",
                LastName = "Kowalska",
                Gender = Gender.Female,
                Status = ContactStatus.Blocked,
                Email = "ewa@wsei.edu.pl",
                Phone = "123123123",
                BirthDate = DateTime.Parse("2001-01-11"),
                Position = "Tester",
                CreatedAt = new DateTime(2024, 1, 1),
                UpdatedAt = (DateTime?)new DateTime(2024, 1, 1)
            }
        );

        builder.Entity<Contact>()
            .OwnsOne(c => c.Address)
            .HasData(new
            {
                City = "Kraków",
                Country = "Poland",
                PostalCode = "25-009",
                Street = "ul. Św. Filipa 17",
                Type = AddressType.Correspondence,
                ContactId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f")
            });
    }
}