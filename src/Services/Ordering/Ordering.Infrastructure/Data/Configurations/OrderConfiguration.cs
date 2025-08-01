using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasConversion(
            oid => oid.Value,
            dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(o => o.OrderName,
            nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.ShippingAddress, addBuilder =>
            {
                addBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                addBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                addBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);

                addBuilder.Property(a => a.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();

                addBuilder.Property(a => a.Country)
                    .HasMaxLength(50);

                addBuilder.Property(a => a.State)
                    .HasMaxLength(50);

                addBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.BillingAddress, addBuilder =>
            {
                addBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                addBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                addBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);

                addBuilder.Property(a => a.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();

                addBuilder.Property(a => a.Country)
                    .HasMaxLength(50);

                addBuilder.Property(a => a.State)
                    .HasMaxLength(50);

                addBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

        builder.ComplexProperty(o => o.Payment, payBuild =>
        {
            payBuild.Property(p => p.CardName)
                .HasMaxLength(50);

            payBuild.Property(p => p.CardNumber)
                .HasMaxLength(24)
                .IsRequired();

            payBuild.Property(p => p.Expiration)
                .HasMaxLength(10);

            payBuild.Property(p => p.CVV)
                .HasMaxLength(3);

            payBuild.Property(p => p.PaymentMethod);
        });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(o => o.TotalPrice);
    }
}