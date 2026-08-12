using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Validators.Article
{
    public class UpdateArticleDtoValidator : AbstractValidator<UpdateArticleDto>
    {
        public UpdateArticleDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان مقاله الزامی است.")
                .MaximumLength(200);

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("خلاصه مقاله الزامی است.")
                .MaximumLength(500);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("متن مقاله الزامی است.");

            RuleFor(x => x.ArticleCategoryId)
                .GreaterThan(0).WithMessage("انتخاب دسته‌بندی الزامی است.");
        }
    }
}
