using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductCategoryDtos;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Validators.ProductCategory
{
    public class UpdateProductCategoryDtoValidator : AbstractValidator<UpdateProductCategoryDto>
    {
        public UpdateProductCategoryDtoValidator()
        { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام دسته‌بندی الزامی است.")
                .MaximumLength(150).WithMessage("نام دسته‌بندی نباید بیشتر از ۱۵۰ کاراکتر باشد.");
        }
    }
}
