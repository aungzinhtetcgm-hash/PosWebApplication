using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PosWebApplication.DAO.CommentDAO;
using PosWebApplication.DAO.PostDAO;
using PosWebApplication.DAO.UserDAO;
using PosWebApplication.DTOs.Comment;
using PosWebApplication.DTOs.Post;
using PosWebApplication.FluentValidators.Comment;
using PosWebApplication.FluentValidators.Post;
using PosWebApplication.Helper;
using PosWebApplication.Services.Admin;
using PosWebApplication.Services.Comment;
using PosWebApplication.Services.Post;
using PosWebApplication.Services.User;

namespace PosWebApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPosWebApplicationServices(
            this IServiceCollection services)
        {
            // User
            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IUserService, UserService>();

            // Admin
            services.AddScoped<IAdminService, AdminService>();

            // Post
            services.AddScoped<IPostDAO, PostDAO>();
            services.AddScoped<IPostService, PostService>();

            // Comment
            services.AddScoped<ICommentDAO, CommentDAO>();
            services.AddScoped<ICommentService, CommentService>();

            // Helper
            services.AddScoped<FilePathHelper>();

            // FluentValidation
            services.AddScoped<
                IValidator<CreatePostDTO>,
                CreatePostValidator>();

            services.AddScoped<
                IValidator<UpdatePostDTO>,
                UpdatePostValidator>();

            services.AddScoped<
                IValidator<CreateCommentDTO>,
                CreateCommentValidator>();

            return services;
        }
    }
}