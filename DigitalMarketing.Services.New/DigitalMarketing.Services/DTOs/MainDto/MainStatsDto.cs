using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.DTOs.MainDto
{
    public class MainStatsDto
    {
        public int ArticlesCount { get; set; }
        public int ProductsCount { get; set; }

        public int DraftArticlesCount { get; set; }
        public int DraftProductsCount { get; set; }

        public int UnreadMessagesCount { get; set; }


        // درصد رشد واقعی نسبت به 30 روز قبل 
        public double ArticlesGrowthPercent { get; set; }
        public double ProductsGrowthPercent { get; set; }


        public List<ArticleDto> LatestArticles { get; set; } = new();
        public List<ProductDto> LatestProducts { get; set; } = new();
    }
}
