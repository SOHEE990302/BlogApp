using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BlogDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<BlogDbContext>>()))
            {
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            Username = "a@a.a",
                            Password = "P@$$w0rd",
                            FirstName = "Admin",
                            LastName = "User",
                            Role = "admin"
                        },
                        new User
                        {
                            Username = "c@c.c",
                            Password = "P@$$w0rd",
                            FirstName = "Contributor",
                            LastName = "User",
                            Role = "approved_contributor"
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Articles.Any())
                {
                    context.Articles.AddRange(
                        new Article
                        {
                            Title = "Welcome to BlogApp!",
                            Body = "This is the first article of BlogApp.",
                            ContributorUsername = "a@a.a",
                            CreateDate = DateTime.UtcNow,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddDays(30)
                        },
                        new Article
                        {
                            Title = "Second Post",
                            Body = "This is another example article written by c@c.c.",
                            ContributorUsername = "c@c.c",
                            CreateDate = DateTime.UtcNow,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddDays(30)
                        }
                    );
                    context.SaveChanges();
                    Console.WriteLine("✅ Article seed data inserted successfully.");
                }
                else
                {
                    Console.WriteLine("✅ Article seed data already exists. Skipping insertion.");
                }
            }
        }
    }
}
