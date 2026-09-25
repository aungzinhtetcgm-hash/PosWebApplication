using Microsoft.EntityFrameworkCore;
using POS_Retails.Entity;
using PosWebApplication.Middlewares;
using PosWebApplication.PosStartUp;
using PosWebApplication.Extensions;
using PosWebApplication.Middlewares;
using PosWebApplication.PosStartUp;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// ApiSwagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MariaDB
var connectionString =
    builder.Configuration.GetConnectionString("MariaDb");

builder.Services.AddDbContext<pos_saas_entities>(
    options =>
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)
        ));

// Authentication & Authorization
builder.Services.ConfigurePosWebApplicationAuthentication(builder.Configuration);

// HttpContext
builder.Services.AddHttpContextAccessor();

// Dependency Injection
builder.Services.AddPosWebApplicationServices();

// Build
var app = builder.Build();

// Admin Seed
using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<pos_saas_entities>();

    await AdminSeedData.InitializeAsync(context);
}

// Global Exception
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// HTTP Request Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Authentication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();