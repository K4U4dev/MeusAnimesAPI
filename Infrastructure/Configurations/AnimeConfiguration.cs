using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AnimeConfiguration : IEntityTypeConfiguration<Anime>
    {
        public void Configure(EntityTypeBuilder<Anime> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Anime__3213E83FC77C470D");

            builder.ToTable("Anime");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Diretor)
                .HasMaxLength(255)
                .HasColumnName("diretor");
            builder.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            builder.Property(e => e.Resumo).HasColumnName("resumo");
        }
    }
}
