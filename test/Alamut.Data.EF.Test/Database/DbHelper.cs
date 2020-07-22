using System;
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
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context =  new AppDbContext(options);

            return context;
        }

        public static Story SeedSingleStory(AppDbContext context)
        {
            var entity = new Story
            {
                Title = "the first story"
            };

            context.Stories.Add(entity);

            context.SaveChanges();

            return entity;
        }

        public static Blog SeedSingleBlog(AppDbContext context)
        {
            Blog entity = new Blog
            {
                Url = "https://github.com/SorenZ/Alamut.AspNet",
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
                    Url = "https://github.com/SorenZ/Alamut.Data",
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