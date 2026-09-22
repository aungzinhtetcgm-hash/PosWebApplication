using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PosWebApplication.Entity;
using PosWebApplication.Constraints;
using PosWebApplication.DAO.CommentDAO;
using PosWebApplication.DAO.PostDAO;
using PosWebApplication.DAO.UserDAO;
using PosWebApplication.DTOs.Comment;
using PosWebApplication.DTOs.Post;
using PosWebApplication.FluentValidators.Comment;
using PosWebApplication.FluentValidators.Post;
using PosWebApplication.Helper;
using PosWebApplication.Middlewares;
using PosWebApplication.PosStartUp;
using PosWebApplication.Services;
using PosWebApplication.Services.Admin;
using PosWebApplication.Services.Comment;
using PosWebApplication.Services.Post;
using PosWebApplication.Services.User;
using POS_Retails.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// MariaDB Connection
var connectionString =
    builder.Configuration.GetConnectionString("MariaDb");

builder.Services.AddDbContext<pos_saas_entities>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));


// HttpContext
builder.Services.AddHttpContextAccessor();


// Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Services
builder.Services.AddScoped<SessionService>();

builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IPostDAO, PostDAO>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<ICommentDAO, CommentDAO>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<FilePathHelper>();


// FluentValidation
builder.Services.AddScoped<
    IValidator<CreatePostDTO>,
    CreatePostValidator>();

builder.Services.AddScoped<
    IValidator<UpdatePostDTO>,
    UpdatePostValidator>();

builder.Services.AddScoped<
    IValidator<CreateCommentDTO>,
    CreateCommentValidator>();


var app = builder.Build();


// ==============================
// Admin Seed
// ==============================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<pos_saas_entities>();

    await AdminSeedData.InitializeAsync(context);
}


// ==============================
// HTTP Request Pipeline
// ==============================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();