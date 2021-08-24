using KybInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains UnitOfWork registration strategies
    /// </summary>
    public static class UnitOfWorkRegister
    {
        /// <summary>
        /// Set UnitOfWork objects lifetime
        /// </summary>
        /// <typeparam name="TDatabaseContext">Database context used by UnitOfWork</typeparam>
        /// <typeparam name="TIUnitOfWork">UnitOfWork interface</typeparam>
        /// <typeparam name="TUnitOfWork">UnitOfWork implementation</typeparam>
        /// <param name="services"></param>
        /// <returns>IServiceCollection itself</returns>
        public static IServiceCollection AddUnitOfWork<TDatabaseContext, TIUnitOfWork, TUnitOfWork>(this IServiceCollection services)
            where TDatabaseContext : class, IDatabaseContext, new()
            where TIUnitOfWork : class, IUnitOfWork
            where TUnitOfWork : class, TIUnitOfWork
        {
            services.AddScoped<TDatabaseContext>();
            services.AddScoped<TIUnitOfWork, TUnitOfWork>();

            return services;
        }

        /// <summary>
        /// Set Entity Framework UnitOfWork objects lifetime
        /// </summary>
        /// <typeparam name="TDatabaseContext">Database context used by Entity Framework UnitOfWork</typeparam>
        /// <typeparam name="TIUnitOfWork">UnitOfWork interface</typeparam>
        /// <typeparam name="TUnitOfWork">UnitOfWork implementation</typeparam>
        /// <param name="services"></param>
        /// <returns>IServiceCollection itself</returns>
        public static IServiceCollection AddEfUnitOfWork<TDatabaseContext, TIUnitOfWork, TUnitOfWork>(this IServiceCollection services)
            where TDatabaseContext : DbContext, IDatabaseContext, new()
            where TIUnitOfWork : class, IUnitOfWork
            where TUnitOfWork : class, TIUnitOfWork
        {
            services.AddDbContext<TDatabaseContext>();
            services.AddScoped<TIUnitOfWork, TUnitOfWork>();

            return services;
        }

    }
}