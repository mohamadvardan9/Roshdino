using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.Tests
{
    public class ProductCategoryRepositoryTests : IDisposable
    {
        private readonly TestDbContextFactory _factory;
        private readonly ProductCategoryRepository _sut;
        public ProductCategoryRepositoryTests()
        {
            _factory = new TestDbContextFactory();
            _sut = new ProductCategoryRepository(_factory.Context);
        }




        [Fact]
        public async Task GetAllAsync_DoesNotReturnSoftDeletedCategories()
        {
            // Arrange
            _factory.Context.ProductCategories.Add(new ProductCategory { Name = "حذف شذه", Slug = "حذف-شده", IsDeleted = true });
            _factory.Context.ProductCategories.Add(new ProductCategory { Name = "فعال", Slug = "فعال"});
            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Single().Name.Should().Be("فعال");
        }





        [Fact]
        public async Task GetByIdAsync_WhenSoftDeleted_ReturnsNull()
        {
            // Arrange
            var category = new ProductCategory { Name = "حذفی", Slug = "حذقی", IsDeleted = true };
            _factory.Context.ProductCategories.Add(category);
            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.GetByIdAsync(category.Id);

            // Assert
            result.Should().BeNull(); // Query Filter حتی رو GetById هم اعمال می‌شه
        }

















        public void Dispose() => _factory.Dispose();
    }
}
