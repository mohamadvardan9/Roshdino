using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Validators.Product
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);


            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0);
        }
    }
}
