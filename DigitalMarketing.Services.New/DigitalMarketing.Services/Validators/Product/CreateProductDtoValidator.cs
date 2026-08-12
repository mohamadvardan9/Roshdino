using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Validators.Product
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان محصول الزامی است.")
                .MaximumLength(200);

            RuleFor(x => x.ShortDescription)
                .NotEmpty().WithMessage("توضیح کوتاه الزامی است.")
                .MaximumLength(500);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("توضیحات کامل الزامی است.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).When(x => x.Price.HasValue)
                .WithMessage("قیمت نمی‌تواند منفی باشد.");

            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0).WithMessage("انتخاب دسته‌بندی الزامی است.");
        }
    }
}
