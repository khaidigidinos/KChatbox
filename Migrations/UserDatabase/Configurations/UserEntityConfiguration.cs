using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalRApi.Entities.User;
using SignalRApi.Enums;

namespace SignalRApi.Migrations.UserDatabase.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder
                .Property("Status")
                .HasConversion(new EnumToNumberConverter<Status, int>());
        }
    }
}
