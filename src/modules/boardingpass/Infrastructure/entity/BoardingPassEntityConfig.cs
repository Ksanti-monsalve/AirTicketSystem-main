// src/modules/boardingpass/Infrastructure/entity/BoardingPassEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.boardingpass.Infrastructure.entity;

public class BoardingPassEntityConfig : IEntityTypeConfiguration<BoardingPassEntity>
{
    public void Configure(EntityTypeBuilder<BoardingPassEntity> builder)
    {
        builder.ToTable("pases_abordar");

        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(bp => bp.CheckinId).HasColumnName("checkin_id").IsRequired();
        builder.Property(bp => bp.CodigoPase).HasColumnName("codigo_pase").HasMaxLength(30).IsRequired();
        builder.Property(bp => bp.CodigoQr).HasColumnName("codigo_qr").HasMaxLength(500);
        builder.Property(bp => bp.PuertaEmbarqueId).HasColumnName("puerta_embarque_id");
        builder.Property(bp => bp.HoraEmbarque).HasColumnName("hora_embarque");
        builder.Property(bp => bp.FechaEmision).HasColumnName("fecha_emision").IsRequired();

        builder.HasIndex(bp => bp.CheckinId).IsUnique();
        builder.HasIndex(bp => bp.CodigoPase).IsUnique();

        builder.HasOne(bp => bp.Checkin)
            .WithOne(c => c.PaseAbordar)
            .HasForeignKey<BoardingPassEntity>(bp => bp.CheckinId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.PuertaEmbarque)
            .WithMany(g => g.PasesAbordar)
            .HasForeignKey(bp => bp.PuertaEmbarqueId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}