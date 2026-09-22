using Microsoft.EntityFrameworkCore;
using PosWebApplication.Entity;

namespace POS_Retails.Entity
{
    public partial class pos_saas_entities : DbContext
    {
        public pos_saas_entities()
        {
        }

        public pos_saas_entities(
            DbContextOptions<pos_saas_entities> options)
            : base(options)
        {
        }

        public virtual DbSet<user> users { get; set; }

        public virtual DbSet<post> posts { get; set; }

        public virtual DbSet<comment> comments { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.u_id);

                entity.Property(e => e.u_id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.email)
                    .HasMaxLength(255);

                entity.Property(e => e.password)
                    .HasMaxLength(255);

                entity.Property(e => e.name)
                    .HasMaxLength(255);

                entity.Property(e => e.img)
                    .HasMaxLength(500);

                entity.Property(e => e.role)
                    .IsRequired();

                entity.Property(e => e.created_by)
                    .IsRequired();

                entity.Property(e => e.updated_by)
                    .IsRequired();

                entity.Property(e => e.created_at)
                    .IsRequired();

                entity.Property(e => e.updated_at)
                    .IsRequired();
            });

            modelBuilder.Entity<post>(entity =>
            {
                entity.ToTable("posts");

                entity.HasKey(e => e.p_id);

                entity.Property(e => e.p_id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.title)
                    .HasMaxLength(255);

                entity.Property(e => e.description)
                    .HasColumnType("longtext");

                entity.Property(e => e.public_flag)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(e => e.created_by)
                    .IsRequired();

                entity.Property(e => e.updated_by)
                    .IsRequired();

                entity.Property(e => e.created_at)
                    .IsRequired();

                entity.Property(e => e.updated_at)
                    .IsRequired();

                entity.HasOne(e => e.user)
                    .WithMany(e => e.posts)
                    .HasForeignKey(e => e.created_by)
                    .HasPrincipalKey(e => e.u_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<comment>(entity =>
            {
                entity.ToTable("comments");

                entity.HasKey(e => e.c_id);

                entity.Property(e => e.c_id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.u_id)
                    .IsRequired();

                entity.Property(e => e.p_id)
                    .IsRequired();

                entity.Property(e => e.created_at)
                    .IsRequired();

                entity.Property(e => e.updated_at)
                    .IsRequired();

                entity.HasOne(e => e.user)
                    .WithMany(e => e.comments)
                    .HasForeignKey(e => e.u_id)
                    .HasPrincipalKey(e => e.u_id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.post)
                    .WithMany(e => e.comments)
                    .HasForeignKey(e => e.p_id)
                    .HasPrincipalKey(e => e.p_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}