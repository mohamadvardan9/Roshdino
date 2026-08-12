using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.Helpers;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Implementations
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly IArticleCategoryRepository _repo;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateArticleCategoryDto> _createValidator;
        private readonly IValidator<UpdateArticleCategoryDto> _updateValidator;

        public ArticleCategoryService(IArticleCategoryRepository repo,IMapper mapper,
            IValidator<CreateArticleCategoryDto> createValidator,
            IValidator<UpdateArticleCategoryDto> updateValidator)
        {
            _repo = repo;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }






        public async Task<List<ArticleCategoryDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            return _mapper.Map<List<ArticleCategoryDto>>(categories);
        }

        public async Task<ArticleCategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<ArticleCategoryDto>(category);
        }

        public async Task<ArticleCategoryDto?> GetBySlugAsync(string slug)
        {
            var category = await _repo.GetBySlugAsync(slug);
            return category == null ? null : _mapper.Map<ArticleCategoryDto>(category);

        }





        public async Task<ServiceResult> CreateAsync(CreateArticleCategoryDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var slug = SlugHelper.GenerateSlug(dto.Name);
            if (await _repo.SlugExistsAsync(slug))
                return ServiceResult.Fail("دسته‌بندی‌ای با این نام (یا مشابه) قبلاً ثبت شده.");

            var category = _mapper.Map<ArticleCategory>(dto);
            category.Slug = slug;

            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> UpdateAsync(int id , UpdateArticleCategoryDto dto)
        {
            var validator = await _updateValidator.ValidateAsync(dto);
            if (!validator.IsValid)
                return ServiceResult.Fail(validator.Errors.Select(e => e.ErrorMessage).ToArray());

            var category = await _repo.GetByIdAsync(id);
            if(category == null) return ServiceResult.Fail("دسته‌بندی پیدا نشد.");

            var slug = SlugHelper.GenerateSlug(dto.Name);
            if(await _repo.SlugExistsAsync(slug,excludeId: id))
                return ServiceResult.Fail("دسته‌بندی‌ای با این نام قبلاً ثبت شده.");

            // چون فیلدهاش زیاد نبود اتومپر نزدم براش
            category.Name = dto.Name;
            

            category.Slug = slug;



            _repo.Update(category);
            await _repo.SaveChangesAsync();

            return ServiceResult.Ok();
        }


        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var caategory = await _repo.GetByIdAsync(id);
            if(caategory == null) return ServiceResult.Fail("دسته‌بندی پیدا نشد.");

            if(await _repo.HasArticlesAsync(id))
                return ServiceResult.Fail("این دسته‌بندی مقاله داره؛ اول مقالات رو جابه‌جا یا حذف کن.");

            _repo.Delete(caategory);
            await _repo.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        

        
        
    }
}
