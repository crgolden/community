using Microsoft.AspNetCore.Builder;

namespace community.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Community API V1");
                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
