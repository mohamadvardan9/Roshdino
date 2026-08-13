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
                .ReturnsAsync(new ProductCategory { Id = 1, Name = "لب تاپ", Slug = "لب-تاپ" });

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










        [Fact]
        public async Task CreateAsync_FirstImage_IsAlwaysSetAsMain()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Title = "تستی",
                ShortDescription = "توضحی کوتاه درباره محصول",
                Description = "توضیح کامل درباره محصول",
                ProductCategoryId = 1,
                Images = new List<IFormFile>
                {
                    CreateFakeFormFile("img1.jpg"),
                    CreateFakeFormFile("img2.jpg"),
                    CreateFakeFormFile("img3.jpg")
                }
            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(dto.ProductCategoryId))
                .ReturnsAsync(new ProductCategory { Id = 1, Name = "دسته تست", Slug = "دسته-تست" });

            _productRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            var counter = 0;
            _fileUploadHelperMock.Setup(f => f.SaveImageAsync(It.IsAny<IFormFile>(), "products"))
                .ReturnsAsync(() => (true, $"/uploads/products/img{++counter}.jpg", (string?)null));

            Product? capturedProduct = null;
            _productRepoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p)
                .Returns(Task.CompletedTask);


            // Act
            await _sut.CreateAsync(dto);


            // Assert
            capturedProduct.Should().NotBeNull();
            capturedProduct!.Images.Should().HaveCount(3);
            capturedProduct.Images.First().IsMain.Should().BeTrue();
            capturedProduct.Images.Skip(1).Should().OnlyContain(i => !i.IsMain);
        }













        [Fact]
        public async Task CreateAsync_WhenImageUploadFails_ReturnsFailure_AndNotSaveProduct()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Title = "Title",
                ShortDescription = "SDescription",
                Description = "Description",
                ProductCategoryId = 1,
                Images = new List<IFormFile> { CreateFakeFormFile("bad.exe") }

            };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new ProductCategory { Id = 1, Name = "دسته", Slug = "دسته" });

            _productRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            _fileUploadHelperMock.Setup(r => r.SaveImageAsync(It.IsAny<IFormFile>(), "products"))
                .ReturnsAsync((false, (string?)null, "مجاز نیست."));

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("مجاز نیست"));
            _productRepoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);

        }














        [Fact]
        public async Task UpdateAsync_WithValidDat_ReturnsSuccess()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Title = "Title",
                Slug = "title",
                ProductCategoryId = 3,
                Images = new List<ProductImage>()
            };

            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            _categoryRepoMock.Setup(r => r.GetByIdAsync(3))
                .ReturnsAsync(new ProductCategory { Id = 3, Name = "Title", Slug = "title" });

            _productRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            var dto = new UpdateProductDto
            {
                Id = 1,
                Title = "new Title",
                ShortDescription = "new Short",
                Description = "New Description",
                ProductCategoryId = 3,
                IsPublished = true
            };


            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            product.Title.Should().Be("new Title");
        }





        [Fact]
        public async Task UpdateAsync_WithNewImages_FirstNewImage_BecomesMain_WhenNoExistingMain()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Title = "مداد",
                ProductCategoryId = 2,
                Images = new List<ProductImage>()  // without any images
            };

            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            _categoryRepoMock.Setup(r => r.GetByIdAsync(2))
                .ReturnsAsync(new ProductCategory { Id = 2, Name = "Title", Slug = "title" });

            _productRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            var counter = 0;
            _fileUploadHelperMock.Setup(f => f.SaveImageAsync(It.IsAny<IFormFile>(), "products"))
                .ReturnsAsync(() => (true, $"/uploads/products/new{++counter}.jpg", (string?)null));

            var dto = new UpdateProductDto
            {
                Id = 1,
                Title = "مداد",
                ShortDescription = "مهم نیس",
                Description = "مهم نیست 2",
                ProductCategoryId = 2,
                NewImages = new List<IFormFile>
                {
                    CreateFakeFormFile("new1.jpg"),
                    CreateFakeFormFile("new2.jpg")
                }
            };


            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            product.Images.Should().HaveCount(2);
            product.Images.First().IsMain.Should().BeTrue();
            product.Images.Skip(1).Should().OnlyContain(i => !i.IsMain);
        }









        [Fact]
        public async Task UpdateAsync_WithNewImages_DoesNotOverrideExistingMain()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Title = "موس",
                ProductCategoryId = 2,
                Images = new List<ProductImage>
                {
                    new ProductImage {Id = 5, ImageUrl = "old.jpg", IsMain=true}
                }
            };

            _productRepoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

            _categoryRepoMock.Setup(r => r.GetByIdAsync(product.ProductCategoryId))
                .ReturnsAsync(new ProductCategory { Id = product.ProductCategoryId, Name = "کامپیوتر", Slug = "کامپیوتر" });

            _productRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            _fileUploadHelperMock.Setup(f => f.SaveImageAsync(It.IsAny<IFormFile>(), "products"))
                .ReturnsAsync((true, "/uploads/products/new.jpg", (string?)null));

            var dto = new UpdateProductDto
            {
                Id = 1,
                Title = "موس",
                ShortDescription = "کوتاه",
                Description = "کامل",
                ProductCategoryId = 2,
                NewImages = new List<IFormFile> { CreateFakeFormFile("new.jpg") }
            };

            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            product.Images.Should().HaveCount(2);
            product.Images.Single(i => i.ImageUrl == "old.jpg").IsMain.Should().BeTrue();
            product.Images.Single(i => i.ImageUrl == "/uploads/products/new.jpg").IsMain.Should().BeFalse();
        }





        [Fact]
        public async Task UpdateAsync_WhenNewImageUploadFails_ReturnsFailure()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Title = "محصول",
                ProductCategoryId = 3,
                Images = new List<ProductImage>()
            };

            _productRepoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

            _categoryRepoMock.Setup(r => r.GetByIdAsync(product.ProductCategoryId))
                .ReturnsAsync(new ProductCategory { Id = product.ProductCategoryId, Name = "سایت", Slug = "سایت" });

            _productRepoMock.Setup(r => r.SlugExistsAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(false);

            _fileUploadHelperMock.Setup(f => f.SaveImageAsync(It.IsAny<IFormFile>(), "products"))
                .ReturnsAsync((false, (string?)null, "حجم فایل نباید بیشتر"));

            var dto = new UpdateProductDto
            {
                Id = 1,
                Title = "محصول",
                ShortDescription = "ممممم",
                Description = "دددددد",
                ProductCategoryId = 3,
                NewImages = new List<IFormFile> { CreateFakeFormFile("big.jpg") }
            };

            // Act
            var result = await _sut.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("حجم فایل نباید بیشتر"));
            _productRepoMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Never);


        }









        [Fact]
        public async Task RemoveImageAsync_WhenProductNotFound_ReturnsFailure()
        {
            // Arrange
            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            // Act
            var result = await _sut.RemoveImageAsync(imageId: 10, productId: 1);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("محصول پیدا نشد"));
            _productRepoMock.Verify(r => r.RemoveImage(It.IsAny<ProductImage>()), Times.Never);
        }






        [Fact]
        public async Task RemoveImageAsync_WhenImageNotFound_ReturnsFailure()
        {
            // Arrange
            var product = new Product { Id = 1, Images = new List<ProductImage>() };
            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _productRepoMock.Setup(r => r.GetImageByIdAsync(10)).ReturnsAsync((ProductImage?)null);

            // Act
            var result = await _sut.RemoveImageAsync(10, 1);


            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("تصویر یافت نشد"));
            _productRepoMock.Verify(r => r.RemoveImage(It.IsAny<ProductImage>()), Times.Never);

        }




        [Fact]
        public async Task RomoveImageAsync_WhenMainImageRemoved_PromotesNextImageToMainImage()
        {
            // Arrange
            var mainIamge = new ProductImage { Id = 10, ImageUrl = "main.jpg", IsMain = true };
            var secondImage = new ProductImage { Id = 15, ImageUrl = "second.jpg", IsMain = false };

            var product = new Product { Id = 1, Images = new List<ProductImage> { mainIamge, secondImage } };

            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _productRepoMock.Setup(r => r.GetImageByIdAsync(10)).ReturnsAsync(mainIamge);

            // Act
            var result = await _sut.RemoveImageAsync(10, 1);

            // Assert
            result.Success.Should().BeTrue();
            _fileUploadHelperMock.Verify(f => f.DeleteImage("main.jpg"), Times.Once);
            product.Images.Should().NotContain(mainIamge);
            secondImage.IsMain.Should().BeTrue();

        }





        [Fact]
        public async Task RemoveImageAsync_WhenNonMainImageRemoved_DoesNotChangeMain()
        {
            // Arrange
            var mainIamge = new ProductImage { Id = 10, ImageUrl = "main.jpg", IsMain = true };
            var secondImage = new ProductImage { Id = 15, ImageUrl = "second.jpg", IsMain = false };

            var product = new Product { Id = 1, Images = new List<ProductImage> { mainIamge, secondImage } };

            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _productRepoMock.Setup(r => r.GetImageByIdAsync(15)).ReturnsAsync(secondImage);

            // Act
            var result = await _sut.RemoveImageAsync(15, 1);

            // Assert
            result.Success.Should().BeTrue();
            mainIamge.IsMain.Should().BeTrue();
        }








        [Fact]
        public async Task RemoveImageAsync_WhenLastImageRemoved_NoErrorEvenThoughNoImagesRemain()
        {
            // Arrange
            var onlyImage = new ProductImage { Id = 10, ImageUrl = "only.jpg", IsMain = true };
            var product = new Product { Id = 1, Images = new List<ProductImage> { onlyImage } };

            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _productRepoMock.Setup(r => r.GetImageByIdAsync(10)).ReturnsAsync(onlyImage);

            // Act
            var result = await _sut.RemoveImageAsync(10, 1);

            // Assert
            result.Success.Should().BeTrue();
            product.Images.Should().BeEmpty();
            _productRepoMock.Verify(r => r.RemoveImage(It.IsAny<ProductImage>()), Times.Once);


        }









        // هدف این تست این است که مطمئن شود هیچ محصولی نمی‌تواند تصویر متعلق به محصول دیگری را به عنوان تصویر اصلی خود انتخاب کند
        // و در نتیجه از ناسازگاری داده‌ها و دستکاری اشتباه جلوگیری شود
        [Fact]
        public async Task SetMainImageAsync_WhenImageBelongsToDifferentProduct_ReturnsFailure()
        {
            // Arrange
            var image = new ProductImage { Id = 10, ProductId = 5 , ImageUrl = "img.jpg"};
            _productRepoMock.Setup(r => r.GetImageByIdAsync(10)).ReturnsAsync((ProductImage)image);

            // Act
            var result = await _sut.SetMainImageAsync(1, 10); // نکته مهمش اینجاست

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Contains("معتبر نیست"));
        }










    }
}
