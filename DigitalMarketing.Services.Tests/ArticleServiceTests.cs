using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;
using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
using DigitalMarketing.DigitalMarketing.Services.Implementations;
using DigitalMarketing.DigitalMarketing.Services.Mapping;
using DigitalMarketing.DigitalMarketing.Services.Validators.Article;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.Tests
{
    public class ArticleServiceTests
    {
        private readonly Mock<IArticleRepository> _articleRepoMock;
        private readonly Mock<IArticleCategoryRepository> _categoryRepoMock;
        private readonly Mock<IFileUploadHelper> _fileUploadHelperMock;
        private readonly IMapper _mapper;
        private readonly ArticleService _sut;

        public ArticleServiceTests()
        {
            _articleRepoMock = new Mock<IArticleRepository>();
            _categoryRepoMock = new Mock<IArticleCategoryRepository>();
            _fileUploadHelperMock = new Mock<IFileUploadHelper>();

            using var loggerFactory = LoggerFactory.Create(builder => { });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ArticleProfile>();
            },
            loggerFactory
            );

            _mapper = config.CreateMapper();


            var createValidator = new CreateArticleDtoValidator();
            var updateValidator = new UpdateArticleDtoValidator();


            _sut = new ArticleService(_articleRepoMock.Object, _categoryRepoMock.Object, _mapper, createValidator, updateValidator, _fileUploadHelperMock.Object);
        }







        private static IFormFile CreateFakeFormFile(string fileName = "cover.jpg", int length = 1000)
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
            var dto = new CreateArticleDto
            {
                Title = "مهندس",
                Summary = "مندسی",
                Content = "مهندسی",
                ArticleCategoryId = 1
            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ArticleCategory { Id = 1 , Name = "مهندسی کامل", Slug = "مهندسی-کامل"});
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            _articleRepoMock.Verify(r => r.AddAsync(It.IsAny<Article>()), Times.Once);


        }








        [ Fact]
        public async Task CreateAsync_WithCoverImage_UploadsAndSetsPath()
        {
            // Arrange
            var dto = new CreateArticleDto
            {
                Title = "مهندس",
                Summary = "مندسی",
                Content = "مهندسی",
                ArticleCategoryId = 1,
                CoverImage = CreateFakeFormFile("cover.jpg")
            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ArticleCategory { Id = 1, Name = "مهندسی کامل", Slug = "مهندسی-کامل" });
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);
            _fileUploadHelperMock.Setup(f => f.SaveImageAsync(dto.CoverImage, "articles"))
                 .ReturnsAsync((true, "/uploads/articles/cover.jpg", (string?)null));

            Article? captured = null;
            _articleRepoMock.Setup(r => r.AddAsync(It.IsAny<Article>()))
                .Callback<Article>(a => captured = a)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            captured!.CoverImageUrl.Should().Be("/uploads/articles/cover.jpg");
        }













    }
}
