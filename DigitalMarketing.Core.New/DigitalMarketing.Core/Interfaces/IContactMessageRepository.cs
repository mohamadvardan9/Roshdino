using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Core.DigitalMarketing.Core.Interfaces
{
    public interface IContactMessageRepository
    {
        /// <summary>
        /// Retrieves all contactMessages
        /// </summary>
        /// <returns>
        /// The task result contains a list of all contactMessages ordered by createdAt date descending
        /// </returns>>
        Task<List<ContactMessage>> GetAllAsync();
        /// <summary>
        /// Retrieves a contactMessage by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the contactMessage.</param>
        /// <returns>
        /// The task result contains the contactMessage with its related data if found; otherwise, null.
        /// </returns>
        Task<ContactMessage?> GetByIdAsync(int id);






        Task AddAsync(ContactMessage message);
        // <summary>
        /// Marks the specified contact message as read
        /// and updates it in the data context.
        /// </summary>
        /// <param name="message">
        /// The contact message to mark as read.
        /// </param>
        void MarkAsRead(ContactMessage message);
        void Delete(ContactMessage message);





        /// <summary>
        /// Gets the total number of unread contact messages.
        /// </summary>
        /// <returns>
        /// The number of contact messages that have not been marked as read.
        /// </returns>
        Task<int> GetUnreadCountAsync();
        Task SaveChangesAsync();





        // for search
        Task<IReadOnlyList<ContactMessage>> SearchAsync(string query, int limit);

    }
}
