using BLL.Businesses;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DIExtensions
    {
        public static void ConfigureDI(this IServiceCollection services)
        {
            #region Business

            services.AddScoped<IBusiness<UserTable>, UserTableBusiness>();
            services.AddScoped<IBusiness<CustomerTable>, CustomerTableBusiness>();

            #endregion

            #region Repository

            services.AddScoped<IRepository<UserTable>, UserTableRepository>();
            services.AddScoped<IRepository<CustomerTable>, CustomerTableRepository>();

            #endregion
        }
    }
}
