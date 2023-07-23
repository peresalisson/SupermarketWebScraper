using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
//using Microsoft.OpenApi.Models;
using Swashbuckle.Swagger;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddMvc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    public void ConfigurationService(IServiceCollection service)
    {

        service.AddApiVersioning(
            options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });


        //service.AddSwaggerGen(options =>
        //{
        //    options.SwaggerDoc("v1",
        //        new Info
        //        {
        //            title = "WebScraper API",
        //            version = "v1",
        //            description = "A API that retrieves information from supermarkets and compare prices.",
        //        });

        //    string filePath = Path.Combine(PlatformsSettings.Default.Application.ApplicationBasePath, pathToDoc);
        //    options.IncludeXmlComments(filePath);
        //    options.DescribeAllEnumsAsStrings();
        //});

    }

    public void Configuration(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
    {
        RewriteOptions rewriteOptions = new RewriteOptions();
        if (!env.IsDevelopment())
        {
            rewriteOptions.AddRedirectToHttps();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseRewriter(rewriteOptions);

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });

        //app.UseSwagger(c =>
        //{
        //    c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
        //});
        app.UseSwaggerUI(
            c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs"); });
    }
}

