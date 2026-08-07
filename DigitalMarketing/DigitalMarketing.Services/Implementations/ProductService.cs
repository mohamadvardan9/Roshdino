using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using DigitalMarketing.DigitalMarketing.Services.Helpers;
using DigitalMarketing.DigitalMarketing.Services.Helpers.GetFile;
using DigitalMarketing.DigitalMarketing.Services.Interfaces;
using FluentValidation;

namespace DigitalMarketing.DigitalMarketing.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository repository, IProductCategoryRepository categoryRepository
            , IMapper mapper
            , IValidator<CreateProductDto> createValidator
            , IValidator<UpdateProductDto> updateValidator
            , IFileService fileService)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _fileService = fileService;


        }








        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<List<ProductDto>> GetPublishedAsync()
        {
            var products = await _repository.GetPublishedAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetBySlugAsync(string slug)
        {
            var product = await _repository.GetBySlugAsync(slug);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<List<ProductDto>> GetByCategoryAsync(int categoryId)
        {
            var products = await _repository.GetByCategoryAsync(categoryId);
            return _mapper.Map<List<ProductDto>>(products);
        }















        public async Task<ServiceResult> CreateAsync(CreateProductDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var categoryExists = await _categoryRepository
                .GetByIdAsync(dto.ProductCategoryId);
            if (categoryExists == null)
                return ServiceResult.Fail("دسته‌بندی انتخاب‌شده معتبر نیست.");

            var slug = SlugHelper.GenerateSlug(dto.Title);
            if (await _repository.SlugExistsAsync(slug))
                return ServiceResult.Fail("محصولی با این عنوان (یا مشابه) قبلاً ثبت شده.");




            
            var product = _mapper.Map<Product>(dto);
            product.Slug = slug;



            // Upload Images
            if (dto.Images != null && dto.Images.Any()) // لیست تصاویر ارسالی دال نباشد و حداقل یک فایل داخل لیست وجود داشته باشد
            {
                foreach (var image in dto.Images)
                {
                    var imagePath = await _fileService.UploadImageAsync(
                        image,
                        "products"
                    );


                    var isMainImage = product.Images.Count == 0; // calculate for the first image, if the image is first,set it as main
                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imagePath,
                        IsMain = isMainImage // if it be first image this will be true
                    });
                }
            }


            // Save Product
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();


            return ServiceResult.Ok();
        }




        // از اینجا شروع به خواندن کن
        public async Task<ServiceResult> UpdateAsync(UpdateProductDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var product = await _repository.GetByIdAsync(dto.Id);
            if (product == null)
                return ServiceResult.Fail("محصول پیدا نشد.");

            var category = await _categoryRepository.GetByIdAsync(dto.ProductCategoryId);
            if (category == null)
                return ServiceResult.Fail("دسته‌بندی انتخاب‌شده معتبر نیست.");

            var slug = SlugHelper.GenerateSlug(dto.Title);
            if (await _repository.SlugExistsAsync(slug, excludeId: dto.Id))
                return ServiceResult.Fail("محصولی با این عنوان قبلاً ثبت شده.");




            
            _mapper.Map(dto, product);
            product.Slug = slug;
            product.UpdatedAt = DateTime.UtcNow;




            // Image Logic
            // عکس‌های جدید اضافه می‌شن (بدون حذف قبلی‌ها؛ حذف جدا مدیریت می‌شه)

            bool hasExistingMain = product.Images.Any(i => i.IsMain); // this product has main photo or not ?

            var firstImagePath = dto.NewImagePaths.FirstOrDefault();

            foreach (var path in dto.NewImagePaths)
            {
                product.Images.Add(new ProductImage
                {
                    ImageUrl = path,
                    IsMain = !hasExistingMain && path == firstImagePath
                });
            }


            _repository.Update(product);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }



        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if(product == null)
                return ServiceResult.Fail("محصول پیدا نشد.");

            _repository.Delete(product); // soft delete
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();

        }











        public async Task<ServiceResult> RemoveImageAsync(int imageId, int productId)
        {
            var image = await _repository.GetImageByIdAsync(imageId);

            if (image == null || image.ProductId != productId)
                return ServiceResult.Fail("تصویر یافت نشد.");
            if (image.ProductId != productId)
                return ServiceResult.Fail("این تصویر متعلق به محصول مشخص شده نیست");


            await _fileService.DeleteAsync(image.ImageUrl);

            _repository.RemoveImage(image);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> SetMainImageAsync(int productId, int imageId)
        {
            var image = await _repository.GetImageByIdAsync(imageId);

            if (image == null || image.ProductId != productId)
                return ServiceResult.Fail("تصویر معتبر نیست.");

            await _repository.SetMainImageAsync(productId, imageId);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> TogglePublishAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return ServiceResult.Fail("محصول پیدا نشد.");

            product.IsPublished = !product.IsPublished;
            _repository.Update(product);
            await _repository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

    }
}
