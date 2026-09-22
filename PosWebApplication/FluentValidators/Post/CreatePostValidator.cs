using FluentValidation;
using PosWebApplication.DTOs.Post;
using PosWebApplication.DTOs.Post;

namespace PosWebApplication.FluentValidators.Post
{
    public class CreatePostValidator : AbstractValidator<CreatePostDTO>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(255)
                .WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(x => x.public_flag)
                .NotEmpty()
                .WithMessage("Please select Public or Private.")
                .Must(x => x == "public" || x == "private")
                .WithMessage("Visibility must be either Public or Private.");
        }
    }
}