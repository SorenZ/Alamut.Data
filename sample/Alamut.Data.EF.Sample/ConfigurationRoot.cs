using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alamut.Data.EF.Test.Database;
using Alamut.Data.EF.Test.Models;
using Alamut.Data.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Alamut.Data.EF.Sample
{
    public static class ConfigurationRoot
    {
        public static void RegisterDatabase(this IServiceCollection services)
        {
            services.AddScoped<DbContext>(provider =>
            {
                var context =  DbHelper.GetInMemoryInstance();
                return context;
            });

        }

        public static void RegisterMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Blog, BlogDto>().ReverseMap();
                //cfg.CreateMap<Bar, BarDto>();
            });
            
            configuration.AssertConfigurationIsValid();

            var mapper = configuration.CreateMapper();

            services.AddSingleton(provider => mapper);
        }

        public static void RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISmartRepository<,>), typeof(SmartRepository<,>));
            services.AddScoped(typeof(Alamut.Data.Repository.ISmartRepository<>), typeof(Alamut.Data.EF.SmartRepository<>));
        }
    }
}
