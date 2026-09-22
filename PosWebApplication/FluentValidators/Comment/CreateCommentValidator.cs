using FluentValidation;
using PosWebApplication.DTOs.Comment;

namespace PosWebApplication.FluentValidators.Comment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDTO>
    {
        public CreateCommentValidator() 
        {
            RuleFor(x => x.p_id)
                .GreaterThan(0)
                .WithMessage("Invalid Post");

            RuleFor(x => x.comments)
                .NotEmpty()
                .WithMessage("Comment is Required.")
                .MaximumLength(200)
                .WithMessage("Comment access 200 characters");
        }
    }
}
