using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Text.RegularExpressions;

namespace DigitalMarketing.DigitalMarketing.Services.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string title)
        {
            var slug = title.Trim().ToLowerInvariant();
            slug = Regex.Replace(slug, @"\s+", "-");   // فاصله‌ها به خط تیره
            slug = Regex.Replace(slug, @"[^a-z0-9\u0600-\u06FF\-]", ""); // فقط حروف/عدد انگلیسی، فارسی و خط تیره
            slug = Regex.Replace(slug, @"-+", "-").Trim('-');   // چند خط‌ تیره پشت‌ سرهم رو یکی کن

            return slug;
        }
    }
}
