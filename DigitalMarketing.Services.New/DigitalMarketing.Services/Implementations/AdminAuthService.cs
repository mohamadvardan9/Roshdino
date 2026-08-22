using AutoMapper;
using BCrypt.Net;
using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminUserDtos;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Implementations
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly IAdminUserRepository _repository;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;

        public AdminAuthService(IAdminUserRepository repository, IValidator<LoginDto> loginValidator
            , IValidator<ChangePasswordDto> changePasswordValidator)
        {
            _repository = repository;
            _loginValidator = loginValidator;
            _changePasswordValidator = changePasswordValidator;
        }




        public async Task<ServiceResult<int>> ValidateLoginAsync(LoginDto dto)
        {
            var validation = await _loginValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult<int>.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());


            var user = await _repository.GetByUserNameAsync(dto.UserName);

            // پیام یکسان برای "کاربر نیست" و "پسورد غلطه" - جلوگیری از افشای اینکه کدوم یوزرنیم معتبره
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PassHash))
                return ServiceResult<int>.Fail("نام کاربری یا رمزعبور اشتباه است");

            return ServiceResult<int>.Ok(user.Id);
        }


        public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var validation = await _changePasswordValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var user = await _repository.GetByIdAsync(dto.UserId);
            if (user == null)
                return ServiceResult.Fail("کاربر پیدا نشد");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PassHash))
                return ServiceResult.Fail("رمز فعلی اشتباه است");

            user.PassHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);


            _repository.Update(user);
            await _repository.SaveChangesAsync();
            return ServiceResult.Ok();
        }




        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if(user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                _repository.Update(user);
                await _repository.SaveChangesAsync();

            }
        }

        
    }
}
