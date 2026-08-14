using Microsoft.AspNetCore.Builder;
namespace BookStore.ProductService.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static WebApplication UsePresentation(this WebApplication app)
    {
        if(app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.MapControllers();
        return app;
    }
}