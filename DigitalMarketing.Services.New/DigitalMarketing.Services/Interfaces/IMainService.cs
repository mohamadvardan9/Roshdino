using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.MainDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Interfaces
{
    public interface IMainService
    {
        /// <summary>
        /// Asynchronously retrieves and aggregates the data required for the dashboard.
        /// Executes independent database queries in parallel to improve performance.
        /// </summary>
        /// <returns>
        /// A <see cref="MainStatsDto"/> containing article and product statistics,
        /// draft counts, unread message count, growth percentages,
        /// and the latest articles and products.
        /// </returns>
        Task<MainStatsDto> GetStatsAsync();
    }
}
