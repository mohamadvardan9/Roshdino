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
                .ReturnsAsync(false);


            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();  // سرویس باید موفقیت برگرداند
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ProductCategory>()), Times.Once);  // یعنی AddAsync دقیقا باید یکبار اجرا شود
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);  // اینم یکبار
        }

























    }
}
