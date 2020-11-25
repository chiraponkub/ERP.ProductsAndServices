using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace erp_project.Libraries.Infrastructure
{
    /// <summary>
    /// Injection ข้อมูลให้กับ Library, DbContext และ Service
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// สำหรับ Inject ส่วนของ Library, DbContext และ Service
        /// </summary>
        public static void RegisterLibraries(this IServiceCollection service)
        {
            service.AddDbContext<DBConnect>();
            service.AddTransient<IProductAndService, EFProductAndService>();
            service.AddTransient<IProductAndServiceDasgboard, EFProductAndServiceDasgboard>();
        }
    }
}
