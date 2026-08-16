using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.DigitalMarketing.Data.Repositories
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly MyDbContext _context;
        public AdminUserRepository(MyDbContext context)
        {
            _context = context;
        }







        public async Task<AdminUser?> GetByIdAsync(int id)
            => await _context.AdminUsers.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<AdminUser?> GetByUserNameAsync(string userName)
            => await _context.AdminUsers.FirstOrDefaultAsync(u => u.UserName == userName);

        public async Task AddAsync(AdminUser user)
            => await _context.AdminUsers.AddAsync(user);
        public void Update(AdminUser user)
            => _context.AdminUsers.Update(user);


        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        
    }
}
