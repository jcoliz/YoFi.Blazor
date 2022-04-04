using Common.DotNet;
using Microsoft.EntityFrameworkCore;
using YoFi.Core;
using YoFi.Core.Reports;
using YoFi.Core.Repositories;
using YoFi.WireApi.Host.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("DataSource=bin\\sqlite.db"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument( o => { o.UseRouteNameAsOperationId = true; o.GenerateAbstractSchemas = false; });

builder.Services.AddSingleton<IClock>(new SystemClock());
builder.Services.AddScoped<IDataContext, ApplicationDbContext>();
builder.Services.AddScoped<ITransactionRepository,TransactionRepository>();
builder.Services.AddScoped<IReportEngine, ReportBuilder>();

var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c =>
    {
        c.Path = "/swagger/{documentName}/yofi.wireapi.swagger.json";
    });

    app.UseSwaggerUi3(options =>
    {
        options.DocumentPath = "/swagger/{documentName}/yofi.wireapi.swagger.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }