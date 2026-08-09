using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Validators.ArticleCategory
{
    public class UpdateArticleCategoryDtoValidator : AbstractValidator<UpdateArticleCategoryDto>
    {
        public UpdateArticleCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام دسته‌بندی الزامی است.")
                .MaximumLength(150).WithMessage("نام دسته‌بندی نباید بیشتر از ۱۵۰ کاراکتر باشد.");
        }
    }
}
