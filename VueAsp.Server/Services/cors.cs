namespace VueAsp.Server.Services
{
    public static class CorsExtensions
    {
        public static void AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }

        public static void UseCorsMiddleware(this IApplicationBuilder app)
        {
            app.UseCors("AllowAllOrigins");
        }
    }
}
