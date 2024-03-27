using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using webapi.Entities;

namespace webapi.Entities
{
    public class ContacttracingContext : IdentityDbContext<
            Persoon,
            Rol,
            Guid,
            IdentityUserClaim<Guid>,
            PersoonRol,
            IdentityUserLogin<Guid>,
            IdentityRoleClaim<Guid>,
            IdentityUserToken<Guid>>
    {

        //--Entitys--
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Aanwezigheid> Aanwezigheden { get; set; }

        //--Identity entitys--
        public DbSet<Persoon> Personen { get; set; }
        public DbSet<MedeWerker> MedeWerkers { get; set; }
        public DbSet<Bezoeker> Bezoekers { get; set; }

        //--JWT entitys--
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ContacttracingContext(DbContextOptions<ContacttracingContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Contacttracing;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //<<<--- identity entity --->>>
            base.OnModelCreating(modelBuilder);

            //<<<--- Restaurant --->>>
            modelBuilder.Entity<Restaurant>(r =>
            {
                r.HasKey(r => r.IdRestaurant);
                r.Property(r => r.IdRestaurant).ValueGeneratedOnAdd();

                r.HasMany(r => r.Aanwezigheden)
                    .WithOne(a => a.Restaurant)
                    .HasForeignKey(a => a.IdRestaurant)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //<<<--- Persoon ---->>>
            modelBuilder.Entity<Persoon>(p =>
            {
                p.HasMany(p => p.Aanwezigheden)
                    .WithOne(a => a.Persoon)
                    .HasForeignKey(a => a.IdPersoon)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                p.HasMany(p => p.PersoonRolen)
                    .WithOne(pr => pr.Persoon)
                    .HasForeignKey(pr => pr.UserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //<<<--- MedeWerker:Persoon ---->>>
            modelBuilder.Entity<MedeWerker>(m =>
            {
                m.HasOne<Persoon>()
                    .WithMany()
                    .HasForeignKey(mw => mw.Id)
                    .OnDelete(DeleteBehavior.Cascade);

                m.HasOne(mw => mw.Restaurant)
                    .WithMany(r => r.MedeWerkers)
                    .HasForeignKey(mw => mw.IdRestaurant)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            //<<<--- Bezoeker:Persoon ---->>>
            modelBuilder.Entity<Bezoeker>(b =>
            {
                b.HasOne<Persoon>()
                    .WithMany()
                    .HasForeignKey(bz => bz.Id)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            //<<<--- Aanwezigheid ---->>>
            modelBuilder.Entity<Aanwezigheid>(a =>
            {
                a.HasKey(aanw => aanw.IdAanwezigheid);
            });

            //<<<--- MemberRole ---->>>
            modelBuilder.Entity<PersoonRol>(pr =>
            {
                pr.HasKey(pr => new { pr.UserId, pr.RoleId });
            });

            //<<<--- Rol ---->>>
            modelBuilder.Entity<Rol>(r =>
            {
                r.HasMany(r => r.PersoonRolen)
                    .WithOne(mr => mr.Role)
                    .HasForeignKey(mr => mr.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                ;
            });

            //<<< ---JWR--->>>
            modelBuilder.Entity<RefreshToken>(r =>
            {
                r.HasOne(r => r.Persoon)
                    .WithMany(m => m.RefreshTokens)
                    .HasForeignKey(r => r.IdPersoon)
                    .IsRequired();
            });
        }
    }
    
}
