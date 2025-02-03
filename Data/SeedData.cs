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
                // 🔹 데이터베이스 생성 확인
                context.Database.EnsureCreated();

                // 🔹 사용자 데이터가 없으면 추가
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            Username = "a@a.a",
                            Password = "P@$$w0rd", // 실제 앱에서는 비밀번호 해싱 필요
                            FirstName = "Admin",
                            LastName = "User",
                            Role = "admin"
                        },
                        new User
                        {
                            Username = "c@c.c",
                            Password = "P@$$w0rd", // 실제 앱에서는 비밀번호 해싱 필요
                            FirstName = "Contributor",
                            LastName = "User",
                            Role = "approved_contributor"
                        }
                    );
                    context.SaveChanges();
                }

                // 🔹 게시글 데이터가 없으면 추가
                if (!context.Articles.Any())
                {
                    context.Articles.AddRange(
                        new Article
                        {
                            Title = "Welcome to BlogApp!",
                            Body = "This is the first article of BlogApp.",
                            ContributorUsername = "a@a.a", // 관리자(admin)가 작성
                            CreateDate = DateTime.UtcNow,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddDays(30)
                        },
                        new Article
                        {
                            Title = "Second Post",
                            Body = "This is another example article written by c@c.c.",
                            ContributorUsername = "c@c.c", // Contributor(c@c.c)가 작성
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
