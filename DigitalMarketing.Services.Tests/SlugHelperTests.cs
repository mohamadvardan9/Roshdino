using DigitalMarketing.DigitalMarketing.Services.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.Tests
{
    public class SlugHelperTests
    {
        [Theory]
        [InlineData("Gaming LapTop","gaming-laptop")]
        [InlineData("لبتاپ ایسر","لبتاپ-ایسر")]
        [InlineData("  چند   فاصله  ", "چند-فاصله")]
        [InlineData("Product!!! 100%", "product-100")]
        [InlineData("Multiple---Dashes", "multiple-dashes")]
        [InlineData("UPPER CASE TITLE", "upper-case-title")]
        public void GenerateSlug_ProducesExpectedSlug(string input, string expected)
        {
            // Act
            var result = SlugHelper.GenerateSlug(input);

            // Assert
            result.Should().Be(expected);
        }




        [Fact]
        public void GenerateSlug_SameInput_AlwaysProducesSameOutput()
        {
            // Act
            // Slug باید Deterministic باشه - یه ورودی همیشه یه خروجی بده
            var firs = SlugHelper.GenerateSlug("محصول تست");
            var second = SlugHelper.GenerateSlug("محصول تست");

            // Assert
            firs.Should().Be(second);
        }


    }
}
