using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using System.Data;

namespace DigitalMarketing.DigitalMarketing.Services.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductCategoryNAme, opt => opt.MapFrom(src => src.ProductCategory.Name));

            CreateMap<ProductImage, ProductImageDto>();

            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Slug, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());


            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Slug, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<ProductDto, UpdateProductDto>();

        }
    }
}
