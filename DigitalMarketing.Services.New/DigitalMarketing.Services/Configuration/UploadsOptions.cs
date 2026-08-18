using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Configuration
{

    /// <summary>
    /// مسیر فیزیکی روی دیسک که فایل‌های آپلودشده (تصاویر محصولات/مقالات) در آن ذخیره می‌شوند.
    /// این مسیر باید بین پروژه 
    /// Admin (که آپلود میکنه)
    /// و Web (که نمایش میده)
    /// مشترک باشد.
    /// </summary>
    public class UploadsOptions
    {
        public const string SectionName = "Uploads";

        /// <summary>مسیر فیزیکی کامل روی دیسک، : "E:\\RoshdinoUploads" یا "/var/shared-uploads"</summary>
        public string RootPath { get; set; } = null!;

        /// <summary>مسیر مجازی که در URL نمایش داده می‌شود، پیش‌فرض "/uploads"</summary>
        public string RequestPath { get; set; } = "/uploads";
    }
}
