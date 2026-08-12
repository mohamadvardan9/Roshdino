using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.DigitalMarketing.Services.Validators.ProductCategory;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.Tests
{
    public class ProductCategoryServiceTests
    {
        private readonly Mock<IProductCategoryRepository> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly ProductCategoryService _sut;    // System Under Test

        public ProductCategoryServiceTests()
        {
            _repositoryMock = new Mock<IProductCategoryRepository>();


            // اتومپر واقعی رو استفاده میکنیم نه ماک
            // چون خودش منطقی نداره که نیار به ماک داشته باشه
            using var loggerFactory = LoggerFactory.Create(builder => { });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductCategoryProfile>();
            },
            loggerFactory
            );

            _mapper = config.CreateMapper();


            // FluentValidation واقعی هم استفاده می‌کنیم — چون خودِ Validator هم باید تست بشه
            var createValidator = new CreateProductCategoryDtoValidator();
            var updateValidator = new UpdateProductCategoryDtoValidator();


            _sut = new ProductCategoryService(_repositoryMock.Object, _mapper, createValidator, updateValidator);
        }







        [Fact]
        public async Task CreateAsync_WithValidName_ReturnSuccess()
        {
            // Arrange
            var dto = new CreateProductCategoryDto { Name = "لب تاپ" };
            _repositoryMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);  // شبیه سازی برای اینکه اسلاگ از قبل وجود ندارد


            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();  // سرویس باید موفقیت برگرداند
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ProductCategory>()), Times.Once);  // یعنی AddAsync دقیقا باید یکبار اجرا شود
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);  // اینم یکبار
        }






        [Fact]
        public async Task CreateAsync_WithEmptyName_ReturnFailure_AndDoesNotCallRepository()
        {
            // Arrange
            var dto = new CreateProductCategoryDto { Name = "" };

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Contains("نام دسته‌بندی الزامی "));
            result.Errors.Should().NotBeEmpty();

            // نکته‌ی مهم: چون Validation رد شده، نباید اصلاً به Repository سر بزنه
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ProductCategory>()), Times.Never);
        }





        [Fact]
        public async Task CreateAsync_WithDuplicateSlug_ReturnsFailure()
        {
            // Arrange
            var dto = new CreateProductCategoryDto { Name = "موس" };
            _repositoryMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(true);  // شبیه سازی برای اینکه اسلاگ از قبل وجود دارد

            // Act
            var result = await _sut.CreateAsync(dto);


            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("قبلا ثبت شده"));

        }









        [Fact]
        public async Task DeleteAsync_WhenCategoryHasNoProducts_ReturnsSuccess()
        {
            // Arrange
            var category = new ProductCategory { Id = 1, Name = "لب تاپ", Slug = "لب-تاپ" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _repositoryMock.Setup(r => r.HasProductsAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            result.Success.Should().BeTrue();
            _repositoryMock.Verify(r => r.Delete(category), Times.Once);
        }






        [Fact]
        public async Task DeleteAsync_WhenCategoryHasProducts_ReturnsFailure()
        {
            // Arrange
            var category = new ProductCategory { Id = 1, Name = "لب تاپ", Slug = "لب-تاپ" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _repositoryMock.Setup(r => r.HasProductsAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("محصول داره"));
            _repositoryMock.Verify(r => r.Delete(It.IsAny<ProductCategory>()), Times.Never);
        }





    }
}
