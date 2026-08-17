using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.MainDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Interfaces
{
    public interface IMainService
    {
        Task<MainStatsDto> GetStatsAsync();
    }
}
