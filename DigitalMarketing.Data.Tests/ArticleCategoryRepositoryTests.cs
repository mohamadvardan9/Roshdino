using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.Tests
{
    public class ArticleCategoryRepositoryTests : IDisposable
    {
        private readonly TestDbContextFactory _factory;
        private readonly ArticleCategoryRepository _sut;
        public ArticleCategoryRepositoryTests()
        {
            _factory = new TestDbContextFactory();
            _sut = new ArticleCategoryRepository(_factory.Context);
        }




        [Fact]
        public async Task GetAllAsync_DoesNotReturnSoftDeletedCategories()
        {
            // Arrange
            _factory.Context.ArticleCategories.Add(new ArticleCategory { Name = "حذف ‌شده", Slug = "حذف-شده", IsDeleted = true });
            _factory.Context.ArticleCategories.Add(new ArticleCategory { Name = "فعال", Slug = "فعال" });
            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Single().Name.Should().Be("فعال");
        }







        [Fact]
        public async Task SlugExistsAsync_WhenSlugAlreadyExists_ReturnsTrue()
        {
            // Arrange
            _factory.Context.ArticleCategories.Add(new ArticleCategory { Name = "کالا", Slug = "کالا" });
            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.SlugExistsAsync("کالا");

            // Arrange
            result.Should().BeTrue();
        }



















        public void Dispose() => _factory.Dispose();
    }
}
