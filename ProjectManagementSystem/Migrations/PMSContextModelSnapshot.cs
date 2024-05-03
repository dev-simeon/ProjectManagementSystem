﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectManagementSystem.Data;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    [DbContext(typeof(PMSContext))]
    partial class PMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectManagementSystem.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AssignedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AssigneeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("FeatureId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 5, 3, 9, 4, 24, 437, DateTimeKind.Utc).AddTicks(5348),
                            Email = "omom@gmail.com",
                            FirstName = "symeon",
                            LastName = "omomowo",
                            Mobile = "08099626426",
                            Password = "$2a$11$x1N4aa6pNY8pwy4OTPIC1eyqZZ9NXuq3yRzyvZexmS/zWyplFwF6i"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 5, 3, 9, 4, 24, 629, DateTimeKind.Utc).AddTicks(3036),
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Mobile = "08099629428",
                            Password = "$2a$11$TnZpqxCNycs8I9X9P2/ntOA.JTdy6MIh9gSNQLf8ODB.gGsHkF2lC"
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2024, 5, 3, 9, 4, 24, 842, DateTimeKind.Utc).AddTicks(4086),
                            Email = "jane.smith@example.com",
                            FirstName = "Jane",
                            LastName = "Smith",
                            Mobile = "08089626426",
                            Password = "$2a$11$9KdUH1dvGtiujCYPs.N5eeK81ZPFaGOTZSUqJYalh.oO/FcVDj3Ri"
                        });
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.UserProject", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProjects");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Feature", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.Project", "Project")
                        .WithMany("Features")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Task", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.User", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ProjectManagementSystem.Models.Feature", "Feature")
                        .WithMany("Tasks")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.UserProject", b =>
                {
                    b.HasOne("ProjectManagementSystem.Models.Project", null)
                        .WithMany("Collaborators")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ProjectManagementSystem.Models.User", null)
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Feature", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.Project", b =>
                {
                    b.Navigation("Collaborators");

                    b.Navigation("Features");
                });

            modelBuilder.Entity("ProjectManagementSystem.Models.User", b =>
                {
                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}