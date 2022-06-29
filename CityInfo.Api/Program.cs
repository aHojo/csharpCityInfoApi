using CityInfo.Api;
using CityInfo.Api.DBContext;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Set up Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders(); // get rid of all the loggers
// builder.Logging.AddConsole();
builder.Host.UseSerilog(); // use serilog instead.

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true; // if client asks for a format we don't support return 406 - eg application/xml
})
    // Adds support for the jsonpatch we are using for the put controller
    .AddNewtonsoftJson()
    // this adds support for xml
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injects FileExtensionContentTypeProvider
// So we can set the content type of a file based on extension
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

// This is our own service for mailing
// This is registering a concrete type eg not an interface
// builder.Services.AddTransient<LocalMailService>();

//Here we use an overload to use the interface and tell it what to inject
#if DEBUG
builder.Services.AddTransient<IMailService,LocalMailService>();
#else
builder.Services.AddTransient<IMailService,CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();

// register or dbcontext
builder.Services.AddDbContext<CityInfoContext>(dbContextOptions =>
{
    dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]);
});

// Register our repository
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
    
    //below is a shortcut in .NET6
// app.MapControllers();

// app.Run(async (context) =>
// {
//     await context.Response.WriteAsync("Hello World");
// });

app.Run();