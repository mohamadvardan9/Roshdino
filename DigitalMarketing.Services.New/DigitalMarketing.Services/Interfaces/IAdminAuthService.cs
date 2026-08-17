using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminUserDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Interfaces
{
    public interface IAdminAuthService
    {
        /// <summary>
        /// Validates the user's login credentials and returns the user's identifier
        /// when authentication is successful.
        /// </summary>
        /// <param name="dto">
        /// The login information containing the username and password.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> containing the user's identifier
        /// when the credentials are valid; otherwise, a failed result with
        /// an appropriate error message.
        /// </returns>
        /// <remarks>
        /// A generic error message is returned when either the username or password
        /// is invalid to prevent user enumeration and avoid revealing whether
        /// a specific username exists.
        /// </remarks>
        Task<ServiceResult<int>> ValidateLoginAsync(LoginDto dto);
        /// <summary>
        /// Changes the user's password after validating the current password
        /// and the provided password change information.
        /// </summary>
        /// <param name="dto">
        /// The password change information containing the user identifier,
        /// current password, and new password.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceResult"/> indicating whether the password
        /// was successfully changed.
        /// </returns>
        /// <remarks>
        /// The current password is verified before updating the password.
        /// The new password must be hashed using BCrypt before being persisted.
        /// </remarks>
        Task<ServiceResult> ChangePasswordAsync(ChangePasswordDto dto);
        /// <summary>
        /// Updates the user's last login timestamp.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user whose last login time should be updated.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous update operation.
        /// </returns>
        /// <remarks>
        /// The timestamp is stored using <see cref="DateTime.UtcNow"/> to ensure
        /// consistency regardless of the server's local time zone.
        /// If the specified user does not exist, no changes are made.
        /// </remarks>
        Task UpdateLastLoginAsync(int userId);
    }
}
