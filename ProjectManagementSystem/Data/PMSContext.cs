using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data
{
    public class PMSContext(DbContextOptions<PMSContext> contextOptions) : DbContext(contextOptions)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>((user) => {
                user.HasMany(u => u.Projects) //a user can have many projects.
                     .WithMany(p => p.Users)
                     .UsingEntity<UserProject>(up => {
                         up.Property(x => x.Role)
                            .HasConversion<string>();
                     });

                user.HasIndex(x => x.Email)
                    .IsUnique();

                user.HasData(new List<User>
                {
                    new("symeon", "omomowo", "omom@gmail.com", "08099626426", "userfive5"),
                    new User("John", "Doe", "john.doe@example.com", "08099629428", "password123"),
                    new User("Jane", "Smith", "jane.smith@example.com", "08089626426", "password456"),
                });
            });


            modelBuilder.Entity<Project>(project => { 
                project.HasIndex(x => x.Name)
                    .IsUnique();
            });
                        
                        

            modelBuilder.Entity<Feature>()
                        .HasOne(f => f.Project)
                        .WithMany(p => p.Features)
                        .HasForeignKey(f => f.ProjectId)
                        .IsRequired();

            modelBuilder.Entity<Models.Task>()
                        .HasOne(t => t.Assignee)
                        .WithMany()
                        .HasForeignKey(t => t.AssigneeId);
                 
                        
            // disable cascade deletes on all tables
            foreach (var entity in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                entity.DeleteBehavior = DeleteBehavior.NoAction;
            }

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
    }
}