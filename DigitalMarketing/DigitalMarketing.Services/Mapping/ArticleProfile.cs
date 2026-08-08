using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Mapping
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.ArticleCategoryName, opt => opt.MapFrom(src => src.ArticleCategory.Name));

            CreateMap<CreateArticleDto, Article>()
                .ForMember(dest => dest.Slug, opt => opt.Ignore())
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImagePath));

            CreateMap<UpdateArticleDto, Article>()
                .ForMember(dest => dest.Slug, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore());

        }
    }
}
