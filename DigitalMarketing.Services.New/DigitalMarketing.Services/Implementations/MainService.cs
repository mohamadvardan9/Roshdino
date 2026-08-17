using AutoMapper;
using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.MainDto;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Implementations
{
    public class MainService : IMainService
    {
        private readonly IMainRepository _repository;
        private readonly IMapper _mapper;
        public MainService(IMainRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }




        public async Task<MainStatsDto> GetStatsAsync()
        {
            var since = DateTime.UtcNow.AddDays(-30);



            var articlesCountTask = _repository.GetArticlesCountAsync();
            var productsCountTask = _repository.GetProductsCountAsync();

            var draftArticlesTask = _repository.GetDraftArticlesCountAsync();
            var draftProductsTask = _repository.GetDraftProductsCountAsync();

            var unreadMessagesTask = _repository.GetUnreadMessagesCountAsync();

            var latestArticlesTask = _repository.GetLatestArticlesAsync(7);
            var latestProductsTask = _repository.GetLatestProductsAsync(5);

            var recentArticlesTask = _repository.GetArticlesCountSinceAsync(since);
            var recentProductsTask = _repository.GetProductsCountSinceAsync(since);


            // حالا همه رو در یه تسک اجرا میکنیم
            await Task.WhenAll(articlesCountTask, productsCountTask, draftArticlesTask, draftProductsTask, unreadMessagesTask,
                latestArticlesTask, latestProductsTask, recentArticlesTask,recentProductsTask);


            var articlesCoutn = await articlesCountTask;
            var productsCount = await productsCountTask;
            var recentArticles = await recentArticlesTask;
            var recentProducts = await recentProductsTask;



            return new MainStatsDto
            {
                ArticlesCount = articlesCoutn,
                ProductsCount = productsCount,

                DraftArticlesCount = await draftArticlesTask,
                DraftProductsCount = await draftProductsTask,

                UnreadMessagesCount = await unreadMessagesTask,

                ArticlesGrowthPercent = CalculateRecentContentSharePercent(articlesCoutn, recentArticles),
                ProductsGrowthPercent = CalculateRecentContentSharePercent(productsCount, recentProducts),


                LatestArticles = _mapper.Map<List<ArticleDto>>(await latestArticlesTask),
                LatestProducts = _mapper.Map<List<ProductDto>>(await latestProductsTask)
            };

        }



        // این متد برای محاسبه درصد محتوای جدید نسبت به کل محتوا استفاده می‌شود
        /*
         * مثلا اگر :
         * کل مقالات => 100
         * مثالات ایجاد شده در 30 روز اخیر => 25
         * خروجی متد میشود : 25درصد
         */
        /// <summary>
        /// Calculates the percentage of recently created content relative to the total content count.
        /// </summary>
        /// <param name="totalCount">
        /// The total number of content items.
        /// </param>
        /// <param name="recentCount">
        /// The number of content items created within the target time period.
        /// </param>
        /// <returns>
        /// The percentage of recent content rounded to one decimal place.
        /// Returns <c>0</c> if <paramref name="totalCount"/> is zero.
        /// </returns>
        private static double CalculateRecentContentSharePercent(int totalCount, int recentCount)
        {
            const int PercentageScale = 100;
            const int DecimalPrecision = 1; // نتیجه تا یک رقم اعشار گرد شود

            if(totalCount == 0)  return 0;


            var clampedRecentCount = Math.Min(recentCount, totalCount);

            var rawPercentage = (double)clampedRecentCount / totalCount * PercentageScale;

            return Math.Round(rawPercentage, DecimalPrecision);
        }





    }
}
