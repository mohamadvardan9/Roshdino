namespace DigitalMarketing.DigitalMarketing.Services.Common
{


    // خیلی مهم :
    // هدفش این دو کلاس این است که کل پروژه یک روش یکسان برای مدیریت موفقیت و خطا داشته باشد

    // برای عملیات هایی که داده ایی برنمی گردانند مثل : حذف , فعال یا غیرفعال کردن و غیره
    public class ServiceResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new(); // لیست خطاها را نگه میدارد
        // example :
        // [
        //   "Slug already exists",
        //   "Category has articles"
        //]


        public static ServiceResult Ok() => new() { Success = true }; // یک نتیجه موفق میسازد با لیست خالی ارورها
        public static ServiceResult Fail(params string[] errors)
            => new() { Success = false, Errors = errors.ToList() }; // برای زمانی است که عملیات شکست میخورد
    }



    // برای عملیات هایی که داده ایی برمی گردانند
    // حرف T یعنی داده ایی که میخواهیم برگردانیم
    // example : T = ArticleCategoryDto
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Ok(T Data)
            => new() { Success = true, Data = Data };
        public static new ServiceResult<T> Fail(params string[] errors)
            => new() { Success = false, Errors = errors.ToList() };
    }
}
