using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Core.DigitalMarketing.Core.Interfaces
{
    public interface IAdminUserRepository
    {
        Task <AdminUser?> GetByUserNameAsync(string userName);
        Task <AdminUser?> GetByIdAsync(int id);
        Task AddAsync(AdminUser user);
        void Update(AdminUser user);
        Task SaveChangesAsync();
    }
}
