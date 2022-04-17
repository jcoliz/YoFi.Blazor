using Common.DotNet;
using Microsoft.EntityFrameworkCore;
using YoFi.Core;
using YoFi.Core.Reports;
using YoFi.Core.Repositories;
using YoFi.Data;
using YoFi.Data.SampleData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("DataSource=bin\\sqlite.db"));
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument( o => { o.UseRouteNameAsOperationId = true; o.GenerateAbstractSchemas = false; });
builder.Services.AddSingleton<IClock>(new SystemClock());
builder.Services.AddScoped<IDataProvider, ApplicationDbContext>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IReportEngine, ReportBuilder>();

var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    await SampleDataStore.SeedFullAsync(context);
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
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Can't use https currently, until I figure out how to handle
// it in the clientapp
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
