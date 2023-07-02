using Mati_ApiVersioning;
using Mati_ApiVersioning.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: Microsoft.AspNetCore.Mvc.ApiController]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    // reporting api versions will return the headers
    // "api-supported-versions" and "api-deprecated-versions"  options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"))
    ;
    //options.ApiVersionReader = ApiVersionReader.Combine(
    //   new QueryStringApiVersionReader("api-version"),
    //   new HeaderApiVersionReader("X-Version"),
    //   new MediaTypeApiVersionReader("ver"));

    options.Conventions.Controller<HelloWorldController>()
    //.HasDeprecatedApiVersion(new ApiVersion(1,0))
    //.Action(c => c.Get()).MapToApiVersion(new ApiVersion(2, 0))
    //.HasApiVersion(new ApiVersion(2, 0))
        .IsApiVersionNeutral()
    ;

    options.Conventions.Add(new VersionByNamespaceConvention());

    options.Conventions.Add(new MyCustomConvention());
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// removing the default api version policy        
//builder.Services.Remove(builder.Services.Single(s => s.ImplementationType == typeof(DefaultApiVersionRoutePolicy)));
//builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, DatedApiVersionMatcherPolicy>());
//builder.Services.AddSingleton<IDatedApiVersionResolver, DatedApiVersionResolver>();

//builder.Services.AddControllers(opts =>
//{
//    opts.Filters.Add<DatedApiVersionFilter>();
//});
//builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, DatedApiVersionMatcherPolicy>());
//builder.Services.AddSingleton<IDatedApiVersionResolver, DatedApiVersionResolver>();

builder.Services.TryAddEnumerable(
    ServiceDescriptor.Transient<IApiControllerSpecification, NonUIControllerSpecification>());

builder.Services.TryAddEnumerable(
    ServiceDescriptor.Transient<IApiControllerFilter, MyApiControllerFilter>());


var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        //foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        //{
        //    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
        //        description.GroupName.ToUpperInvariant());
        //}

        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();