using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminUserDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Validators.AdminUser
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("نام کاربری الزامی است");
            RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور الزامی است");
        }
    }
}
