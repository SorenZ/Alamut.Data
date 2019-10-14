using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Alamut.Data.EF.Test.Database
{
    public static class DbHelper
    {
        public static AppDbContext GetInMemoryInstance()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("test")
                .Options;

            var context =  new AppDbContext(options);

            return context;
        }

        public static Blog Seed_SingleBlog(AppDbContext context)
        {
            Blog entity = new Blog
            {
                Url = "https://github.com/SorenZ/Alamut.DotNet",
                Rating = 5
            };

            context.Blogs.Add(entity);

            context.SaveChanges();

            return entity;
        }

        public static List<Blog> SeedBulkBlogs(AppDbContext context)
        {
            for (int i = 0; i < 15; i++)
            {
                var entity = new Blog
                {
                    Url = "https://github.com/SorenZ/Alamut.DotNet",
                    Rating = 5
                };

                context.Blogs.Add(entity);
            }
            
            context.SaveChanges();

            return context.Blogs.ToList();
        }

        public static void CleanBlog(AppDbContext context)
        {
            context.Blogs.RemoveRange(context.Blogs);
            context.SaveChanges();
        }
    }

}