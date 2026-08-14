using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.Tests
{
    public class ArticleRepositoryTests : IDisposable
    {
        private readonly TestDbContextFactory _factory;
        private readonly ArticleRepository _sut;
        public ArticleRepositoryTests()
        {
            _factory = new TestDbContextFactory();
            _sut = new ArticleRepository(_factory.Context);
        }



        private async Task<ArticleCategory> SeedCategoryAsync()
        {
            var category = new ArticleCategory { Name = "دسته", Slug = "دسته" };
            _factory.Context.ArticleCategories.Add(category);
            await _factory.Context.SaveChangesAsync();
            return category;
        }





        [Fact]
        public async Task GetAllAsync_DoesNotReturnSoftDeletedArticles()
        {
            // Arrange
            var category = await SeedCategoryAsync();

            _factory.Context.Articles.Add(new Article
            {
                Title = "حذف ‌شده",
                Slug = "حذف-شده",
                Summary = "...",
                Content = "...",
                ArticleCategoryId = category.Id,
                IsDeleted = true
            });
            _factory.Context.Articles.Add(new Article
            {
                Title = "فعال",
                Slug = "فعال",
                Summary = "...",
                Content = "...",
                ArticleCategoryId = category.Id
            });
            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Single().Title.Should().Be("فعال");
        }






        [Fact]
        public async Task GetPublishedAsync_OnlyReturnsPublishedArticles()
        {
            // Arrange
            var category = await SeedCategoryAsync();

            _factory.Context.Articles.Add(new Article
            {
                Title = "منتشر شده",
                Slug = "منتشره-شده",
                Summary = "...",
                Content = "...",
                ArticleCategoryId = category.Id,
                IsPublished = true
            });
            _factory.Context.Articles.Add(new Article
            {
                Title = "نشده",
                Slug = "نشده",
                Summary = "...",
                Content = "...",
                ArticleCategoryId = category.Id,
                IsPublished = false
            });
            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.GetPublishedAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(a => a.Title == "منتشر شده");
        }













        public void Dispose() => _factory.Dispose();
    }
}
