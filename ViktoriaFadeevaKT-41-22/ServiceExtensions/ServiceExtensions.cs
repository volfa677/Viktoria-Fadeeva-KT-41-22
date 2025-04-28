using ViktoriaFadeevaKT_41_22.Services.DepartmentServices;
using ViktoriaFadeevaKT_41_22.Services.DisciplineServices;
using ViktoriaFadeevaKT_41_22.Services.TeacherServices;
using ViktoriaFadeevaKT_41_22.Services.LoadServices;



namespace ViktoriaFadeevaKT_41_22.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<TeacherService>();

            services.AddScoped<DisciplineService>();
            services.AddScoped<LoadService>();
            services.AddScoped<DepartmentService>();

            return services;
        }
    }
}