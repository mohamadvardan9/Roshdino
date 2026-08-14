using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Data.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.Tests
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly TestDbContextFactory _factory;
        private readonly ProductRepository _sut;
        public ProductRepositoryTests()
        {
            _factory = new TestDbContextFactory();
            _sut = new ProductRepository(_factory.Context);
        }


        private async Task<ProductCategory> SeedCategoryAsync()
        {
            var category = new ProductCategory{ Name = "کالا", Slug = "کالا"};
            _factory.Context.ProductCategories.Add(category);
            await _sut.SaveChangesAsync();
            return category;
        }



        [Fact]
        public async Task GetAllAsync_DoesNotReturnsSoftDeletedProducts()
        {
            // Arrange
            var category = await SeedCategoryAsync();

            _factory.Context.Products.Add(new Product { Title = "حذف ‌شده", Slug = "حذف-شده", ShortDescription = "...",
                Description ="...", ProductCategoryId = category.Id, IsDeleted = true });
            _factory.Context.Products.Add(new Product { Title = "فعال", Slug = "فعال", ShortDescription = "...",
                Description = "...", ProductCategoryId = category.Id,
            });

            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Single().Title.Should().Be("فعال");
        }






        [Fact]
        public async Task GetPublishedAsync_OnlyRetunrsPublishedProducts()
        {
            // Arrange
            var category = await SeedCategoryAsync();

            _factory.Context.Products.Add(new Product
            {
                Title = "منتشر نشده",
                Slug = "منتشر-نشده",
                ShortDescription = "...",
                Description = "...",
                ProductCategoryId = category.Id,
                IsPublished = false
            });
            _factory.Context.Products.Add(new Product
            {
                Title = "منتشر شده",
                Slug = "منتشر-شده",
                ShortDescription = "...",
                Description = "...",
                ProductCategoryId = category.Id,
                IsPublished= true
            });

            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.GetPublishedAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Single().Title.Should().Be("منتشر شده");
        }





        [Fact]
        public async Task SlugExistsAsync_WithExcludeId_IgnoresItsOwnSlug()
        {
            // Arrange
            var category = await SeedCategoryAsync();
            _factory.Context.Products.Add(new Product
            {
                Id = 1,
                Title = "کالا",
                Slug = "کالا",
                ShortDescription = "...",
                Description = "...",
                ProductCategoryId = category.Id,
            });

            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _sut.SlugExistsAsync("کالا", 1);

            // Assert
            result.Should().BeFalse();

        }







        [Fact]
        public async Task RemoveImage_WhenProductDeleted_ImageIsAlsoRemoved_DueToCascadeDelete()
        {
            // Arrange
            // این تست دقیقاً همون OnDelete(DeleteBehavior.Cascade) که رو ProductImage تنظیم کردیم رو چک می‌کنه
            var category = await SeedCategoryAsync();
            var product = new Product
            {
                Title = "کالا",
                Slug = "کالا",
                ShortDescription = "...",
                Description = "...",
                ProductCategoryId = category.Id
            };
            product.Images.Add(new ProductImage { ImageUrl = "img.jpg", IsMain = true });

            _factory.Context.Products.Add(product);
            await _factory.Context.SaveChangesAsync();

            var imageId = product.Images.First().Id;

            // HardDelete واقعی محصول
            _factory.Context.Products.Remove(product);
            await _factory.Context.SaveChangesAsync();

            // Act
            var result = await _factory.Context.ProductImages.FindAsync(imageId);

            // Assert
            result.Should().BeNull(); // چون Cascade Delete تنظیم شده
        }






        [Fact]
        public async Task GetImageByIdAsync_ReturnsCorrectImage()
        {
            var category = await SeedCategoryAsync();
            var product = new Product
            {
                Title = "کالا",
                Slug = "کالا",
                ShortDescription = "...",
                Description = "...",
                ProductCategoryId = category.Id
            };
            product.Images.Add(new ProductImage { ImageUrl = "img.jpg", IsMain = true });
            _factory.Context.Products.Add(product);
            await _factory.Context.SaveChangesAsync();

            var imageId = product.Images.First().Id;

            // Act
            var result = await _sut.GetImageByIdAsync(imageId);

            // Assert
            result.Should().NotBeNull();
            result.ImageUrl.Should().Be("img.jpg");

        }




        public void Dispose() => _factory.Dispose();
    }
}
