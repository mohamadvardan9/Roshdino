using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.Tests
{
    public class FileUploadHelperTests : IDisposable
    {
        private readonly string _tempRoot;
        private readonly FileUploadHelper _sut;
        public FileUploadHelperTests()
        {
            // یه پوشه‌ی موقت واقعی می‌سازیم تا WebRootPath واقعی باشه
            _tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempRoot);

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns(_tempRoot);

            _sut = new FileUploadHelper(envMock.Object);
        }


        private static IFormFile CreateFakeFormFile(string fileName, long length, byte[]? content = null)
        {
            content ??= new byte[] { 1, 2, 3 };
            var stream = new MemoryStream(content);

            var mock = new Mock<IFormFile>();
            mock.Setup(f => f.FileName).Returns(fileName);
            mock.Setup(f => f.Length).Returns(length);
            mock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream target, CancellationToken _) => stream.CopyToAsync(target));

            return mock.Object;
        }




        // =========================================================
        // Validation Logic
        // =========================================================
        [Theory]
        [InlineData("virus.exe")]
        [InlineData("document.pdf")]
        [InlineData("archive.zip")]
        public async Task SaveImageAsync_WithInvalidExtension_ReturnsFailure(string fileName)
        {
            // Arrange
            var file = CreateFakeFormFile(fileName, length: 1000);

            // Act
            var (success, path, error) = await _sut.SaveImageAsync(file, "products");

            // Assert
            success.Should().BeFalse();
            path.Should().BeNull();
            error.Should().Contain("مجاز نیست");
        }




        [Theory]
        [InlineData("photo.jpg")]
        [InlineData("photo.jpeg")]
        [InlineData("photo.png")]
        [InlineData("photo.webp")]
        [InlineData("PHOTO.JPG")] // حروف بزرگ هم باید قبول بشه
        public async Task SaveImageAsync_WithValidExtension_Succeeds(string fileName)
        {
            // Arrange
            var file = CreateFakeFormFile(fileName, length: 1000);

            // Act
            var (success, path, error) = await _sut.SaveImageAsync(file, "products");

            // Assert
            success.Should().BeTrue();
            path.Should().NotBeNullOrEmpty();
            error.Should().BeNull();

        }





        [Fact]
        public async Task SaveImageAsync_WithFileLargerThan5MB_ReturnsFailure()
        {
            // Arrange
            var file = CreateFakeFormFile("big.jpg", length: 6 * 1024 * 1024); // 6MB

            // Act
            var (success, path, error) = await _sut.SaveImageAsync(file, "products");

            // Act
            success.Should().BeFalse();
            error.Should().Contain("۵ مگابایت");
        }








        [Fact]
        public async Task SaveImageAsync_WithFileExactly5MB_Succeeds()
        {
            // نکته‌ی مهم: کد فعلی از > استفاده کرده، نه >=، پس دقیقاً ۵ مگابایت باید قبول بشه
            var file = CreateFakeFormFile("exact.jpg", length: 5 * 1024 * 1024);

            // Act
            var (success, path, error) = await _sut.SaveImageAsync(file, "products");

            // Assert
            success.Should().BeTrue();
            path.Should().NotBeNullOrEmpty();
            error.Should().BeNull();
        }













        public void Dispose()
        {
            if(Directory.Exists(_tempRoot))
                Directory.Delete(_tempRoot, recursive: true);
        }
    }
}
