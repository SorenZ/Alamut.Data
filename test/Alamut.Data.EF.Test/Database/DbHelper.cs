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
    }

}