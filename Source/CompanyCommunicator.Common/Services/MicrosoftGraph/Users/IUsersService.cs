// <copyright file="IUsersService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Services.MicrosoftGraph
{
    using Microsoft.Graph;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Get the User data.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// get the list of users by group of userids.
        /// </summary>
        /// <param name="userIdsByGroups">list of grouped user ids.</param>
        /// <returns>list of users.</returns>
        Task<IEnumerable<User>> GetBatchByUserIds(IEnumerable<IEnumerable<string>> userIdsByGroups);

        /// <summary>
        /// get the list of users by group of usermails.
        /// </summary>
        /// <param name="userMails">list of user mails.</param>
        /// <returns>list of users.</returns>
        Task<IEnumerable<User>> GetBatchByUserMails(IEnumerable<string> userMails);

        /// <summary>
        /// get the stream of users.
        /// </summary>
        /// <param name="filter">the filter condition.</param>
        /// <returns>stream of users.</returns>
        IAsyncEnumerable<IEnumerable<User>> GetUsersAsync(string filter = null);

        /// <summary>
        /// get user by id.
        /// </summary>
        /// <param name="userId">the user id.</param>
        /// <returns>user data.</returns>
        Task<User> GetUserAsync(string userId);

        /// <summary>
        /// Gets all the users in the tenant. Doesn't include 'Guest' users.
        ///
        /// Note: If delta link is passed, the API returns delta changes only.
        /// </summary>
        /// <param name="deltaLink">Delta link.</param>
        /// <returns>List of users and delta link.</returns>
        Task<(IEnumerable<User>, string)> GetAllUsersAsync(string deltaLink = null);

        /// <summary>
        /// Checks if the user has teams license.
        /// </summary>
        /// <param name="userId">User's AAD id.</param>
        /// <returns>true if the user has teams license, false otherwise.</returns>
        Task<bool> HasTeamsLicenseAsync(string userId);

        /// <summary>
        /// Get the list of users from a list.
        /// </summary>
        /// <param name="listUsers">User's mail list.</param>
        /// <returns>LList of users.</returns>
        Task<IEnumerable<User>> GetListUsersAsync(List<string> listUsers);
    }
}