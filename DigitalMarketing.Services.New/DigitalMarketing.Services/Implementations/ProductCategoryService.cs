using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductCategoryDtos;
using DigitalMarketing.DigitalMarketing.Services.Helpers;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Implementations
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductCategoryDto> _createValidator;
        private readonly IValidator<UpdateProductCategoryDto> _updateValidator;
        public ProductCategoryService(IProductCategoryRepository repository, IMapper mapper,
            IValidator<CreateProductCategoryDto> createValidator,
            IValidator<UpdateProductCategoryDto> updateValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }







        public async Task<List<ProductCategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<List<ProductCategoryDto>>(categories);
        }

        public async Task<ProductCategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            return category == null ? null : _mapper.Map<ProductCategoryDto>(category);
        }

        public async Task<ProductCategoryDto?> GetBySlugAsync(string slug)
        {
            var category = await _repository.GetBySlugAsync(slug);

            return category == null ? null : _mapper.Map<ProductCategoryDto>(category);
        }






        public async Task<ServiceResult> CreateAsync(CreateProductCategoryDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid) return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());
            
            var slug = SlugHelper.GenerateSlug(dto.Name);
            if (await _repository.SlugExistsAsync(slug))
                return ServiceResult.Fail("دسته بندی ای با این نام یا مشابه ثبلا ثبت شده است");

            var caategory = _mapper.Map<ProductCategory>(dto);
            caategory.Slug = slug;



            await _repository.AddAsync(caategory);
            await _repository.SaveChangesAsync();


            return ServiceResult.Ok();
        }


        public async Task<ServiceResult> UpdateAsync(int id , UpdateProductCategoryDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid) return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());


            var category = await _repository.GetByIdAsync(id);
            if (category == null) return ServiceResult.Fail("دسته بندی پیدا نشد");


            var slug = SlugHelper.GenerateSlug(dto.Name);
            if(await _repository.SlugExistsAsync(slug, excludeId: id))
                return ServiceResult.Fail("دسته بندی ای با این نام یا مشابه ثبلا ثبت شده است");


            category.Name = dto.Name;
            category.Slug = slug;


            _repository.Update(category);
            await _repository.SaveChangesAsync();



            return ServiceResult.Ok();
        }


        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return ServiceResult.Fail("دسته بندی پیدا نشد");


            if (await _repository.HasProductsAsync(id))
                return ServiceResult.Fail("این دسته‌بندی محصول داره؛ اول محصولات رو جابه‌جا یا حذف کن.");


            _repository.Delete(category);
            await _repository.SaveChangesAsync();


            return ServiceResult.Ok();
        }

        

        

        
    }
}
