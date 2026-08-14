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

            _categoryRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ArticleCategory { Id = 1, Name = "مهندسی کامل", Slug = "مهندسی-کامل" });
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            _articleRepoMock.Verify(r => r.AddAsync(It.IsAny<Article>()), Times.Once);


        }








        [Fact]
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








        [Fact]
        public async Task CreateAsync_WhenCoverImageUploadFails_ReturnsFailure()
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
                .ReturnsAsync((false, (string?)null, "فرمت .exe مجاز نیست."));

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("مجاز نیست"));
            _articleRepoMock.Verify(r => r.AddAsync(It.IsAny<Article>()), Times.Never);

        }





        [Fact]
        public async Task CreateAsync_WithInvalidCategory_ReturnsFailure()
        {
            // Arrange
            var dto = new CreateArticleDto
            {
                Title = "مهندس",
                Summary = "مندسی",
                Content = "مهندسی",
                ArticleCategoryId = 12,
            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ArticleCategory?)null);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("معتبر نیست"));
            _articleRepoMock.Verify(r => r.AddAsync(It.IsAny<Article>()), Times.Never);
        }







        [Fact]
        public async Task CreateAsync_WithDuplicateSlug_ReturnsFailure()
        {
            // Arrange
            var dto = new CreateArticleDto
            {
                Title = "مهندس",
                Summary = "مندسی",
                Content = "مهندسی",
                ArticleCategoryId = 1,
            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ArticleCategory { Id = 1, Name = "مهندسی کامل", Slug = "مهندسی-کامل" });
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("مقاله قبلا وجود داشته"));
        }






        [Fact]
        public async Task UpdateAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var article = new Article { Id = 1, Title = "قدیمی", Slug = "قدیمی", ArticleCategoryId = 3 };

            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);
            _categoryRepoMock.Setup(r => r.GetByIdAsync(3))
                .ReturnsAsync(new ArticleCategory { Id = 3, Name = "اخبار", Slug = "اخبار" });
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            var dtp = new UpdateArticleDto
            {
                Id = 1,
                Title = "جدید",
                Summary = "جدیدی",
                Content = "جدیدیییی",
                ArticleCategoryId = 3
            };


            // Act
            var result = await _sut.UpdateAsync(dtp);

            // Assert
            result.Success.Should().BeTrue();
            article.Title.Should().Be("جدید");
        }







        [Fact]
        public async Task UpdateAsync_WithNewCoverImage_ReplacesAndDeletesOldImage()
        {
            // Arrange
            var article = new Article
            {
                Id = 1,
                Title = "قدیمی",
                ArticleCategoryId = 1,
                CoverImageUrl = "/uploads/articles/old.jpg"
            };

            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);
            _categoryRepoMock.Setup(r => r.GetByIdAsync(3))
                .ReturnsAsync(new ArticleCategory { Id = 3, Name = "اخبار", Slug = "اخبار" });
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            var dto = new UpdateArticleDto
            {
                Id = 1,
                Title = "جدید",
                Summary = "جدیدی",
                Content = "جدیدیییی",
                ArticleCategoryId = 3,
                NewCoverImage = CreateFakeFormFile("new.jpg")
            };

            _fileUploadHelperMock.Setup(r => r.SaveImageAsync(dto.NewCoverImage,"articles"))
                .ReturnsAsync((true, "/uploads/articles/new.jpg", (string?)null));

            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            article.CoverImageUrl.Should().Be("/uploads/articles/new.jpg");
            _fileUploadHelperMock.Verify(f => f.DeleteImage("/uploads/articles/old.jpg"), Times.Once);
        }







        [Fact]
        public async Task UpdateAsync_WithoutNewCoverImage_KeepsOldImage_AndDoesNotCallDelete()
        {
            // Arrange
            var article = new Article
            {
                Id = 1,
                Title = "قدیمی",
                ArticleCategoryId = 1,
                CoverImageUrl = "/uploads/articles/old.jpg"
            };

            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);
            _categoryRepoMock.Setup(r => r.GetByIdAsync(3))
                .ReturnsAsync(new ArticleCategory { Id = 3, Name = "اخبار", Slug = "اخبار" });
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            var dto = new UpdateArticleDto
            {
                Id = 1,
                Title = "جدید",
                Summary = "جدیدی",
                Content = "جدیدیییی",
                ArticleCategoryId = 3
            };

            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            article.CoverImageUrl.Should().Be("/uploads/articles/old.jpg");
            _fileUploadHelperMock.Verify(f => f.DeleteImage(It.IsAny<string>()), Times.Never);
        }





        [Fact]
        public async Task UpdateAsync_WhenNewCoverImageUploadFails_ReturnsFailure_AndDoesNotDeleteOldImage()
        {
            // Arrange
            var article = new Article
            {
                Id = 1,
                Title = "قدیمی",
                ArticleCategoryId = 1,
                CoverImageUrl = "/uploads/articles/old.jpg"
            };

            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);
            _categoryRepoMock.Setup(r => r.GetByIdAsync(3))
                .ReturnsAsync(new ArticleCategory { Id = 3, Name = "اخبار", Slug = "اخبار" });
            _articleRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            var dto = new UpdateArticleDto
            {
                Id = 1,
                Title = "جدید",
                Summary = "جدیدی",
                Content = "جدیدیییی",
                ArticleCategoryId = 3,
                NewCoverImage = CreateFakeFormFile("big.jpg", length: 10_000_000)
            };

            _fileUploadHelperMock.Setup(r => r.SaveImageAsync(dto.NewCoverImage, "articles"))
                .ReturnsAsync((false, (string?)null, "حجم فایل نباید بیشتر از ۵ مگابایت باشد."));

            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("۵ مگابایت"));

            // نکته‌ی مهم: چون آپلود جدید شکست خورده، نباید عکس قدیمی پاک بشه
            _fileUploadHelperMock.Verify(f => f.DeleteImage(It.IsAny<string>()), Times.Never);
            article.CoverImageUrl.Should().Be("/uploads/articles/old.jpg");
        }










        [Fact]
        public async Task TogglePublishAsync_WhenPublishing_SetsPublishedAtToNow()
        {
            // Arrange
            var article = new Article { Id = 1, IsPublished = false, PublishedAt = DateTime.UtcNow.AddDays(-10) };
            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);

            var before = DateTime.UtcNow;

            // Act
            var result = await _sut.TogglePublishAsync(1);
            var after = DateTime.UtcNow;

            // Assert
            result.Success.Should().BeTrue();
            article.IsPublished.Should().BeTrue();
            article.PublishedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        }






        [Fact]
        public async Task TogglePublishAsync_WhenUnpublishing_DoesNotChangePublishedAt()
        {
            // Arrange
            var orginalDate = DateTime.UtcNow.AddDays(-10);
            var article = new Article { Id = 1, IsPublished = true, PublishedAt = orginalDate };
            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);

            // Act
            var result = await _sut.TogglePublishAsync(1);

            // Assert
            result.Success.Should().BeTrue();
            article.IsPublished.Should().BeFalse();
            article.PublishedAt.Should().Be(orginalDate); // دست‌نخورده مونده

        }





        [Fact]
        public async Task TogglePublishAsync_WhenArticleNotFound_ReturnsFailure()
        {
            // Arrange
            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Article?)null);

            // Act
            var result = await _sut.TogglePublishAsync(1);

            // Assert
            result.Success.Should().BeFalse();
        }




        [Fact]
        public async Task RemoveImageAsync_WhenArticleNotFound_ReturnsFailure()
        {
            // Arrange
            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Article?)null);

            // Act
            var result = await _sut.RemoveImageAsync(1);

            // Assett
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("مقاله پیدا نشد"));
        }






        [Fact]
        public async Task RemoveImageAsync_WhenArticleHasNoImage_ReturnsFailure()
        {
            // Arrange
            var article = new Article { Id = 1, CoverImageUrl = null };
            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);

            // Act
            var resutl = await _sut.RemoveImageAsync(1);

            // Assert
            resutl.Success.Should().BeFalse();
            resutl.Errors.Should().Contain(e => e.Contains("تصویری برای حذف ندارد"));
            _fileUploadHelperMock.Verify(f => f.DeleteImage(It.IsAny<string>()), Times.Never);

        }




        [Fact]
        public async Task RemoveImageAsync_WhenValid_DeletesFileAndCallsRepository()
        {
            // Arrange
            var article = new Article { Id = 1, CoverImageUrl = "/uploads/articles/cover.jpg" };
            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);

            // Act
            var result = await _sut.RemoveImageAsync(1);

            // Assert
            result.Success.Should().BeTrue();
            _fileUploadHelperMock.Verify(f => f.DeleteImage("/uploads/articles/cover.jpg"), Times.Once);
            _articleRepoMock.Verify(r => r.RemoveImage(article), Times.Once);
        }

    }
}
