using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Interfaces
{
    public interface IAdminSearchService
    {
        Task<IReadOnlyList<AdminSearchResultDto>> SearchAsync(string query, int limit = 10);
    }
}
