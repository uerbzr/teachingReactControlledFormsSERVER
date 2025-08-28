using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using workshop.wwwapi.Data;
using workshop.wwwapi.Endpoints;
using workshop.wwwapi.Models;
using workshop.wwwapi.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "C# Complaints API",
        Version = "v1",
        Description = "A .Net Minimal API"
    });

    // Explicitly set OpenAPI version
    s.SupportNonNullableReferenceTypes(); // Optional, but good practice
});
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseInMemoryDatabase("Complaints");
});
builder.Services.AddCors();
builder.Services.AddScoped<IRepository<ComplaintDetails>, Repository<ComplaintDetails>>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "C# Complaints API");
    });

    app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) 
                  .AllowCredentials()); 
}
app.UseHttpsRedirection();
app.ConfigureComplaintsEndpoint();
app.Run();

