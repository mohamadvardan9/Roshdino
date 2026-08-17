using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminSearch;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Implementations
{
    public class AdminSearchService : IAdminSearchService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        public AdminSearchService(IArticleRepository articleRepository, IProductRepository productRepository,
            IContactMessageRepository contactMessageRepository, IArticleCategoryRepository articleCategoryRepository,
            IProductCategoryRepository productCategoryRepository)
        {
            _articleRepository = articleRepository;
            _productRepository = productRepository;
            _contactMessageRepository = contactMessageRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _productCategoryRepository = productCategoryRepository;
        }





        public async Task<IReadOnlyList<AdminSearchResultDto>> SearchAsync(string query, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(query)) return [];

            query = query.Trim();


            var result = new List<AdminSearchResultDto>();


            // =========================
            // Articles
            // =========================

            var articles =
                await _articleRepository.SearchAsync(query, limit);

            result.AddRange(
                articles.Select(a => new AdminSearchResultDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Type = "Article",
                    Icon = "bi-file-earmark-text",
                    Url = $"/Articles/Edit/{a.Id}"
                }));


            // =========================
            // Products
            // =========================

            var products =
                await _productRepository.SearchAsync(query, limit);

            result.AddRange(
                products.Select(p => new AdminSearchResultDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Type = "Product",
                    Icon = "bi-box-seam",
                    Url = $"/Product/Edit/{p.Id}"
                }));


            // =========================
            // Contact Messages
            // =========================

            var contactMessages =
                await _contactMessageRepository.SearchAsync(query, limit);

            result.AddRange(
                contactMessages.Select(m => new AdminSearchResultDto
                {
                    Id = m.Id,
                    Title = m.FullName,
                    Type = "Message",
                    Icon = "bi-envelope",
                    Url = $"/ContactMessages/Details/{m.Id}"
                }));


            // =========================
            // Article Categories
            // =========================

            var articleCategories =
                await _articleCategoryRepository.SearchAsync(query, limit);

            result.AddRange(
                articleCategories.Select(ac => new AdminSearchResultDto
                {
                    Id = ac.Id,
                    Title = ac.Name,
                    Type = "Article Category",
                    Icon = "bi-folder",
                    Url = $"/ArticleCategories/Edit/{ac.Id}"
                }));


            // =========================
            // Product Categories
            // =========================

            var productCategories =
                await _productCategoryRepository.SearchAsync(query, limit);

            result.AddRange(
                productCategories.Select(pc => new AdminSearchResultDto
                {
                    Id = pc.Id,
                    Title = pc.Name,
                    Type = "Product Category",
                    Icon = "bi-tags",
                    Url = $"/ProductCategories/Edit/{pc.Id}"
                }));


            return result
                .Take(limit)
                .ToList();

        }
    }
}
