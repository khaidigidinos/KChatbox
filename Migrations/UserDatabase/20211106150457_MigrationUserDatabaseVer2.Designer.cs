﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalRApi.DatabaseContexts;

namespace SignalRApi.Migrations.UserDatabase
{
    [DbContext(typeof(UserDatabaseContext))]
    [Migration("20211106150457_MigrationUserDatabaseVer2")]
    partial class MigrationUserDatabaseVer2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("SignalRApi.Entities.RoleEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<long>("UpdatedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("UserEntityId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserEntityId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SignalRApi.Entities.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("UpdatedAt")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SignalRApi.Entities.RoleEntity", b =>
                {
                    b.HasOne("SignalRApi.Entities.UserEntity", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserEntityId");
                });

            modelBuilder.Entity("SignalRApi.Entities.UserEntity", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}