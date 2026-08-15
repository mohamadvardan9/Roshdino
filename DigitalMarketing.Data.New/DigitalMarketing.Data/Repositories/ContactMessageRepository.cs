using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.DigitalMarketing.Data.Repositories
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly MyDbContext _context;
        public ContactMessageRepository(MyDbContext context)
        {
            _context = context;
        }




        public async Task<List<ContactMessage>> GetAllAsync()
            => await _context.ContactMessages
            .OrderByDescending(cm => cm.CreatedAt)
            .ToListAsync();

        public async Task<ContactMessage?> GetByIdAsync(int id)
            => await _context.ContactMessages
            .FirstOrDefaultAsync(cm => cm.Id == id);




        public async Task AddAsync(ContactMessage message) => await _context.ContactMessages.AddAsync(message);

        public void MarkAsRead(ContactMessage message)
        {
            message.IsRead = true;
            _context.ContactMessages.Update(message);
        }

        public void Delete(ContactMessage message)
        {
            message.IsDeleted = true;
            _context.ContactMessages.Update(message);
        }




        public async Task<int> GetUnreadCountAsync()
            => await _context.ContactMessages
            .CountAsync(cm => !cm.IsRead);



        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        
    }
}
