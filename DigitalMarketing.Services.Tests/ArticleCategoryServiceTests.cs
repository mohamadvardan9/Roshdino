using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.DigitalMarketing.Services.Validators.ArticleCategory;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.Tests
{
    public class ArticleCategoryServiceTests
    {
        private readonly Mock<IArticleCategoryRepository> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly ArticleCategoryService _sut;

        public ArticleCategoryServiceTests()
        {
            _repositoryMock = new Mock<IArticleCategoryRepository>();

            using var loggerFactory = LoggerFactory.Create(builder => { });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ArticleCategoryProfile>();
            },
            loggerFactory
            );

            _mapper = config.CreateMapper();


            var createValidator = new CreateArticleCategoryDtoValidator();
            var updateValidator = new UpdateArticleCategoryDtoValidator();

            _sut = new ArticleCategoryService(_repositoryMock.Object, _mapper, createValidator, updateValidator);

        }







        [Fact]
        public async Task CreateAsync_WithValidName_ReturnsSuccess()
        {
            // Arrange
            var dto = new CreateArticleCategoryDto { Name = "تتلو" };
            _repositoryMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);


            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ArticleCategory>()), Times.Once);
        }







        [Fact]
        public async Task CreateAsync_WithEmptyName_ReturnsFailure_AndDoesNotCallRepository()
        {
            // Arrange
            var dto = new CreateArticleCategoryDto { Name = "" };

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Contains("نام دسته‌بندی الزامی "));
            result.Errors.Should().NotBeEmpty();
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ArticleCategory>()), Times.Never);

        }








        [Fact]
        public async Task DeleteAsync_WhenCategoryHasNoArticles_ReturnsSuccess()
        {
            // Arrange
            var category = new ArticleCategory { Id = 1, Name = "سایت آماده", Slug = "سایت-آماده" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _repositoryMock.Setup(r => r.HasArticlesAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            result.Success.Should().BeTrue();
            _repositoryMock.Verify(r => r.Delete(category), Times.Once);
        }





        [Fact]
        public async Task DeleteAsync_WhenCategoryHasArticles_ReturnsFailure()
        {
            // Arrange
            var category = new ArticleCategory { Id = 1, Name = "سایت آماده", Slug = "سایت-آماده" };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _repositoryMock.Setup(r => r.HasArticlesAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("مقاله داره"));
            _repositoryMock.Verify(r => r.Delete(It.IsAny<ArticleCategory>()), Times.Never);
        }





        [Fact]
        public async Task DeleteAsync_WhenCategoryNotFound_ReturnsFailure()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ArticleCategory?)null);

            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("پیدا نشد"));
        }





    }
}
