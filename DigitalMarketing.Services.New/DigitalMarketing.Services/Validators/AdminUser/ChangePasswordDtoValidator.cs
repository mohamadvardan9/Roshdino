using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminUserDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Validators.AdminUser
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("رمز عبور فعلی الزامی است");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("رمز عبور جدید الزامی است")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کارکتر باشد");
        }
    }
}
