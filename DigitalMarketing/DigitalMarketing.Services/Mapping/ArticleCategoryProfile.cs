using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Mapping
{
    public class ArticleCategoryProfile : Profile
    {
        public ArticleCategoryProfile()
        {
            CreateMap<ArticleCategory, ArticleCategoryDto>();

            CreateMap<CreateArticleCategoryDto, ArticleCategory>()
                .ForMember(dest => dest.Slug, opt => opt.Ignore()) // چون همونجا در لایه سروسیس تولید میشه :)
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
