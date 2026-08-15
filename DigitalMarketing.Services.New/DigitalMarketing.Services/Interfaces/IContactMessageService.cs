using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Interfaces
{
    public interface IContactMessageService
    {
        Task<List<ContactMessageDto>> GetAllAsync();
        Task<ContactMessageDto?> GetByIdAsync(int id);
        Task<int> GetUnreadCountAsync();

        Task<ServiceResult> CreateAsync(CreateContactMessageDto dto);
        Task<ServiceResult> MarkAsReadAsync(int id);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
