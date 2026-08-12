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






        

    }
}
