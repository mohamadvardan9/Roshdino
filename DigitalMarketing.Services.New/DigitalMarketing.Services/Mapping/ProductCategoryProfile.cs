using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductCategoryDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Mapping
{
    public class ProductCategoryProfile : Profile
    {
        public ProductCategoryProfile()
        {
            CreateMap<ProductCategory, ProductCategoryDto>();

            CreateMap<CreateProductCategoryDto, ProductCategory>()
                .ForMember(dest => dest.Slug, opt => opt.Ignore()); // Slug تو Service ساخته می‌شه، نه از ورودی خام
        }
    }
}
