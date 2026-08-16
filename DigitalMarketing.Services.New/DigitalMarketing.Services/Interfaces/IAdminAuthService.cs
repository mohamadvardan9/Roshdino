using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminUserDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Interfaces
{
    public interface IAdminAuthService
    {
        Task<ServiceResult<int>> ValidateLoginAsync(LoginDto dto);
        Task<ServiceResult> ChangePasswordAsync(ChangePasswordDto dto);
        Task UpdateLastLoginAsync(int userId);
    }
}
