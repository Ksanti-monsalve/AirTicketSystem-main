// src/modules/contactrelationship/Infrastructure/entity/ContactRelationshipEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

public class ContactRelationshipEntityConfig : IEntityTypeConfiguration<ContactRelationshipEntity>
{
    public void Configure(EntityTypeBuilder<ContactRelationshipEntity> builder)
    {
        builder.ToTable("relaciones_contacto");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(c => c.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsRequired();
        builder.HasIndex(c => c.Descripcion).IsUnique();
    }
}