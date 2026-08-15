using AutoMapper;
using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Implementations
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateContactMessageDto> _createValidator;
        public ContactMessageService(IContactMessageRepository contactMessageRepository, IMapper mapper,
            IValidator<CreateContactMessageDto> createValidator)
        {
            _contactMessageRepository = contactMessageRepository;
            _mapper = mapper;
            _createValidator = createValidator;
        }




        public async Task<List<ContactMessageDto>> GetAllAsync()
        {
            var messages = await _contactMessageRepository.GetAllAsync();
            return _mapper.Map<List<ContactMessageDto>>(messages);
        }

        public async Task<ContactMessageDto?> GetByIdAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            return message == null ? null : _mapper.Map<ContactMessageDto>(message);
        }

        public Task<int> GetUnreadCountAsync() => _contactMessageRepository.GetUnreadCountAsync();



        public async Task<ServiceResult> CreateAsync(CreateContactMessageDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return ServiceResult.Fail(validation.Errors.Select(e => e.ErrorMessage).ToArray());

            var message = _mapper.Map<ContactMessage>(dto);

            await _contactMessageRepository.AddAsync(message);
            await _contactMessageRepository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> MarkAsReadAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            if (message == null)
                return ServiceResult.Fail("پیام پیدا نشد.");

            _contactMessageRepository.MarkAsRead(message);
            await _contactMessageRepository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var message = await _contactMessageRepository.GetByIdAsync(id);
            if (message == null)
                return ServiceResult.Fail("پیام پیدا نشد.");

            _contactMessageRepository.Delete(message);
            await _contactMessageRepository.SaveChangesAsync();

            return ServiceResult.Ok();
        }

    }
}
