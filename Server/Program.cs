using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Tailwind;
using CBPresents.Services;
using CBPresents.Data.Data;
using Microsoft.EntityFrameworkCore;
using CBPresents.Server.Models;
using Hangfire;
using Hangfire.SqlServer;
using CBPresents.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDBContext>(options =>
   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!, o =>
   {
       o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
       o.MigrationsAssembly("CBPresents.Server");
   }));

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters.NameClaimType = "name";
    });

builder.Services.AddHangfire(conf => conf
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();

builder.Services.AddServices();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var timeService = scope.ServiceProvider.GetRequiredService<ITimeService>();
var numberOfWinnersService = scope.ServiceProvider.GetRequiredService<INumberOfWinnersService>();
var jobsService = scope.ServiceProvider.GetRequiredService<IJobsService>();

await numberOfWinnersService.SetNumberOfWiinners(50);
// This is UTC Time !!!
await timeService.SetTime("2023-12-22T11:45:00.000");
await jobsService.ScheduleJob();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.RunTailwind("tailwind", "../Client/");
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseHangfireDashboard();

app.UseRouting();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");



app.Run();
