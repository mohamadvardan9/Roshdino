using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.DigitalMarketing.Services.Validators.Product;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IProductCategoryRepository> _categoryRepoMock;
        private readonly Mock<IFileUploadHelper> _fileUploadHelperMock;
        private readonly IMapper _mapper;
        private readonly ProductService _sut;

        public ProductServiceTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _categoryRepoMock = new Mock<IProductCategoryRepository>();
            _fileUploadHelperMock = new Mock<IFileUploadHelper>();

            using var loggerFactory = LoggerFactory.Create(builder => { });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
            },
            loggerFactory
            );

            _mapper = config.CreateMapper();



            var createValidator = new CreateProductDtoValidator();
            var updateValidator = new UpdateProductDtoValidator();


            _sut = new ProductService(_productRepoMock.Object, _categoryRepoMock.Object, _mapper, createValidator, updateValidator, _fileUploadHelperMock.Object);
        }








        // Helper
        // برای ساخت یه 
        // IFormFile
        // فیک
        private static IFormFile CreateFakeFormFile(string fileName = "test.jpg", int length = 1000)
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(f => f.FileName).Returns(fileName);
            mock.Setup(f => f.Length).Returns(length);
            
            return mock.Object;
        }





        [Fact]
        public async Task CreateAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            var dto = new CreateProductDto
            {
                Title = "لب تاپ ایسر",
                ShortDescription = "این لب تاپ بشدت قدرتمند است",
                Description = "وقتی شما این لب تاپ را بخرید, تا 10 سال آینده نگران لب تاپ جدید نخواهید بود :)",
                Price = 250000000,
                ProductCategoryId = 1,
                Images = new List<IFormFile> { CreateFakeFormFile("img1,jpg") }
            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(dto.ProductCategoryId))
                .ReturnsAsync(new ProductCategory { Id = 1, Name = "لب تاپ", Slug = "لب-تاپ"});

            _productRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            _fileUploadHelperMock.Setup(f => f.SaveImageAsync(It.IsAny<IFormFile>(), "products"))
                .ReturnsAsync((true, "/uploads/products/img1.jpg", (string?)null));


            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            _productRepoMock.Verify(r => r.AddAsync(It.Is<Product>(
                p => p.Images.Count == 1 && p.Images.First().IsMain == true)), Times.Once);
        }









        [Fact]
        public async Task CreateAsync_WithInvalidCategory_ReturnsFailure()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Title = "تستی",
                ShortDescription = "توضحی کوتاه درباره محصول",
                Description = "توضیح کامل درباره محصول",
                ProductCategoryId = 55
            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(dto.ProductCategoryId))
                .ReturnsAsync((ProductCategory?)null);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("معتبر نیست"));
        }






    }
}
