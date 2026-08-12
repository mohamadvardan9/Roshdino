using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;
using DigitalMarketing.DigitalMarketing.Services.Helpers;
using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;
        private readonly IArticleCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateArticleDto> _createValidator;
        private readonly IValidator<UpdateArticleDto> _updateValidator;
        private readonly IFileUploadHelper _fileUploadHelper;

        public ArticleService(IArticleRepository repository, IArticleCategoryRepository categoryRepository,
            IMapper mapper, IValidator<CreateArticleDto> createValidator,
            IValidator<UpdateArticleDto> updateValidator,
            IFileUploadHelper fileUploadHelper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _fileUploadHelper = fileUploadHelper;
        }









        public async Task<List<ArticleDto>> GetAllAsync()
        {
            var articles = await _repository.GetAllAsync();
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<ArticleDto?> GetByIdAsync(int id)
        {
            var article = await _repository.GetByIdAsync(id);
            return article ==null ? null : _mapper.Map<ArticleDto>(article);
        }

        public async Task<List<ArticleDto>> GetPublishedAsync()
        {
            var article = await _repository.GetPublishedAsync();
            return _mapper.Map<List<ArticleDto>>(article);
        }


        public async Task<ArticleDto?> GetBySlugAsync(string slug)
        {
            var article = await _repository.GetBySlugAsync(slug);
            return article == null ? null : _mapper.Map<ArticleDto>(article);
        }


        public async Task<List<ArticleDto>> GetByCategoryAsync(int categoryId)
        {
            var articles = await _repository.GetByCategoryAsync(categoryId);
            return _mapper.Map<List<ArticleDto>>(articles);
        }














        public async Task<ServiceResult> CreateAsync(CreateArticleDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var category = await _categoryRepository.GetByIdAsync(dto.ArticleCategoryId);
            if (category == null)
                return ServiceResult.Fail("دسته‌بندی انتخاب‌شده معتبر نیست.");

            var slug = SlugHelper.GenerateSlug(dto.Title);
            if(await _repository.SlugExistsAsync(slug))
                return ServiceResult.Fail("این مقاله قبلا وجود داشته است.");


            // Upload Cover Image
            if (dto.CoverImage is { Length: > 0 })
            {
                var (success, path, error) = await _fileUploadHelper.SaveImageAsync(dto.CoverImage,"articles");

                if (!success)
                    return ServiceResult.Fail(error!);

                dto.CoverImagePath = path;
            }



            var article = _mapper.Map<Article>(dto);
            article.Slug = slug;
            article.PublishedAt = DateTime.UtcNow;
            article.UpdatedAt = null;

            


            await _repository.AddAsync(article);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }


        public async Task<ServiceResult> UpdateAsync(UpdateArticleDto dto, Action<string>? deleteOldCoverCallback = null)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var article = await _repository.GetByIdAsync(dto.Id);
            if (article == null)
                return ServiceResult.Fail("مقاله پیدا نشد.");

            var category = await _categoryRepository.GetByIdAsync(dto.ArticleCategoryId);
            if (category == null)
                return ServiceResult.Fail("دسته‌بندی انتخاب‌شده معتبر نیست.");

            var slug = SlugHelper.GenerateSlug(dto.Title);
            if (await _repository.SlugExistsAsync(slug, excludeId: dto.Id))
                return ServiceResult.Fail("این مقاله قبلا وجود داشته است.");




            // keep old image before any change
            var oldCoverImage = article.CoverImageUrl;

            // -------------------------
            // Map Article
            // -------------------------

            _mapper.Map(dto, article);

            article.Slug = slug;

            // -------------------------
            // Upload New Cover
            // -------------------------

            if (dto.NewCoverImage is { Length: > 0 })
            {
                var (success, path, error) =
                    await _fileUploadHelper.SaveImageAsync(
                        dto.NewCoverImage,
                        "articles");

                if (!success)
                    return ServiceResult.Fail(error!);

                // new image
                article.CoverImageUrl = path;

                // delete old image 
                if (!string.IsNullOrEmpty(oldCoverImage))
                {
                    _fileUploadHelper.DeleteImage(oldCoverImage);
                }
            }



            _repository.Update(article);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var article = await _repository.GetByIdAsync(id);
            if (article == null)
                return ServiceResult.Fail("مقاله پیدا نشد.");

            _repository.Delete(article);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        

        

        

        

        

        public async Task<ServiceResult> TogglePublishAsync(int id)
        {
            var article = await  _repository.GetByIdAsync(id);
            if (article == null)
                return ServiceResult.Fail("مقاله پیدا نشد.");

            article.IsPublished = !article.IsPublished;
            if (article.IsPublished == true)
                article.PublishedAt = DateTime.UtcNow;
            

            _repository.Update(article);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> RemoveImageAsync(int articleId)
        {
            var article = await _repository.GetByIdAsync(articleId);
            if (article == null)
                return ServiceResult.Fail("مقاله پیدا نشد.");

            if (string.IsNullOrWhiteSpace(article.CoverImageUrl))
                return ServiceResult.Fail("این مقاله تصویری برای حذف ندارد");


            _fileUploadHelper.DeleteImage(article.CoverImageUrl);

            _repository.RemoveImage(article);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }



    }
}
