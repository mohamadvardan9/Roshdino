using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminSearch
{
    public class AdminSearchResultDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
