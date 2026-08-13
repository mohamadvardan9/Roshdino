using AutoMapper;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;
using DigitalMarketing.DigitalMarketing.Services.Helpers;
using DigitalMarketing.DigitalMarketing.Services.Helpers.FileService;
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
        private readonly IFileUploadHelper _fileUploadHelper;


        public ProductService(IProductRepository repository, IProductCategoryRepository categoryRepository
            , IMapper mapper
            , IValidator<CreateProductDto> createValidator
            , IValidator<UpdateProductDto> updateValidator
            , IFileUploadHelper fileUploadHelper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _fileUploadHelper = fileUploadHelper;


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

                    var (success, path, error) = await _fileUploadHelper.SaveImageAsync(image, "products");

                    if (!success)
                        return ServiceResult.Fail(error!);

                    var isMainImage = product.Images.Count == 0; // calculate for the first image, if the image is first,set it as main
                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = path!,
                        IsMain = isMainImage // if it be first image this will be true
                    });
                }
            }


            // Save Product
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();


            return ServiceResult.Ok();
        }




        
        public async Task<ServiceResult> UpdateAsync(UpdateProductDto dto)
        {
            // -------------------------
            // Validation
            // -------------------------

            var validation = await _updateValidator.ValidateAsync(dto);

            if (!validation.IsValid)
                return ServiceResult.Fail(
                    validation.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray());


            // -------------------------
            // Get Product
            // -------------------------

            var product = await _repository.GetByIdAsync(dto.Id);

            if (product == null)
                return ServiceResult.Fail("محصول پیدا نشد.");


            // -------------------------
            // Validate Category
            // -------------------------

            var category = await _categoryRepository.GetByIdAsync(dto.ProductCategoryId);

            if (category == null)
                return ServiceResult.Fail("دسته‌بندی انتخاب‌شده معتبر نیست.");


            // -------------------------
            // Generate Slug
            // -------------------------

            var slug = SlugHelper.GenerateSlug(dto.Title);

            if (await _repository.SlugExistsAsync(slug,excludeId: dto.Id))
                return ServiceResult.Fail("محصولی با این عنوان قبلاً ثبت شده.");


            // -------------------------
            // Image Logic
            // -------------------------

            // آیا محصول قبل از اضافه شدن تصاویر جدید،
            // تصویر اصلی دارد؟
            var hasExistingMainImage = product.Images.Any(i => i.IsMain);


            // مشخص می‌کند اولین تصویر جدید هستیم
            var isFirstNewImage = true;


            if (dto.NewImages != null && dto.NewImages.Any())
            {
                foreach (var file in dto.NewImages.Where(f => f != null && f.Length > 0))
                {
                    // Upload
                    var (success, path, error) = await _fileUploadHelper.SaveImageAsync(file,"products");


                    if (!success)
                        return ServiceResult.Fail(error!);


                    // اگر محصول تصویر اصلی نداشته باشد،
                    // اولین تصویر جدید را تصویر اصلی قرار می‌دهیم.
                    var isMain = !hasExistingMainImage && isFirstNewImage;


                    product.Images.Add(new ProductImage
                    {
                        ProductId = product.Id,

                        ImageUrl = path!,

                        IsMain = isMain
                    });


                    isFirstNewImage = false;
                }
            }


            // -------------------------
            // Update Product Information
            // -------------------------

            _mapper.Map(dto, product);


            product.Slug = slug;

            product.UpdatedAt = DateTime.UtcNow;


            // -------------------------
            // Save
            // -------------------------

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

            var product = await _repository.GetByIdAsync(productId);
            if (product == null)
                return ServiceResult.Fail("محصول پیدا نشد.");

            var image = await _repository.GetImageByIdAsync(imageId);
            if (image == null)
                return ServiceResult.Fail("تصویر یافت نشد.");




            // بررسی می‌کنیم آیا تصویر حذف‌شده Main بوده
            var wasMain = image.IsMain;
            
            _fileUploadHelper.DeleteImage(image.ImageUrl);

            // حذف تصویر از Collection
            product.Images.Remove(image);

            // اگه تصویر اصلی حذف شذ و هنوز تصویر دیگری وجود داشت :
            if(wasMain && product.Images.Any())
            {
                var newMainImage = product.Images.First();

                newMainImage.IsMain = true;
            }

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
