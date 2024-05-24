namespace WebAPI.TaskManager.Configuration
{
    public static class GeneralConfiguration
    {
        public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            #region authentication
            services.AddJwtAuthentication(configuration);
            services.AddHttpContextAccessor();
            #endregion

            #region CORS
            services.AddCors(options => {
                options.AddPolicy(
                    "AllowAll", 
                    builder => {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            #endregion

            #region controller policies
            services.AddControllers()
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.PropertyNamingPolicy = null;
                   });
            #endregion
        }
    }
}
