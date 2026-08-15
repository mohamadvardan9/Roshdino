using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Validators.ContactMessage
{
    public class CreateContactMessageDtoValidator : AbstractValidator<CreateContactMessageDto>
    {
        public CreateContactMessageDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("نام الزامی است.")
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است.")
                .EmailAddress().WithMessage("فرمت ایمیل معتبر نیست.")
                .MaximumLength(250);

            RuleFor(x => x.Phone)
                .Matches(@"^(?:\+98|0)?9\d{9}$")
                .WithMessage("شماره موبایل معتبر وارد کنید.");


            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("متن پیام الزامی است.")
                .MaximumLength(2000);

            // اگه فیلد Honeypot پر شده باشه، یعنی ربات بوده - رد می‌کنیم
            RuleFor(x => x.Website)
                .Must(string.IsNullOrEmpty)
                .WithMessage("درخواست نامعتبر است.");
        }
    }
}
