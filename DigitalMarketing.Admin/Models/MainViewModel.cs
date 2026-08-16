namespace DigitalMarketing.Admin.Models
{
    public class MainViewModel
    {
        public int ArticlesCount { get; set; }
        public int ProductsCount { get; set; }
        public double ArticlesGrowth { get; set; }
        public List<MainArticleViewModel> LatestArticles { get; set; } = [];
        public List<MainProductViewModel> LatestProducts { get; set; } = [];
        public int DraftArticlesCount { get; set; }
        public int DraftProductsCount { get; set; }
    }
}
