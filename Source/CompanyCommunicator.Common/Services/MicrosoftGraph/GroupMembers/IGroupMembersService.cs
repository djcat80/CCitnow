﻿// <copyright file="IGroupMembersService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Services.MicrosoftGraph
{
    using Microsoft.Graph;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Group Members Service.
    /// </summary>
    public interface IGroupMembersService
    {
        /// <summary>
        /// Get groups members.
        /// </summary>
        /// <param name="groupId">Group Id.</param>
        /// <returns>Enumerator to iterate over a collection of <see cref="User"/>.</returns>
        Task<IEnumerable<User>> GetGroupMembersAsync(string groupId);

        /// <summary>
        /// get group members page by id.
        /// </summary>
        /// <param name="groupId">group id.</param>
        /// <returns>group members page.</returns>
        Task<IGroupTransitiveMembersCollectionWithReferencesPage> GetGroupMembersPageByIdAsync(string groupId);

        /// <summary>
        /// get group members page by next page ur;.
        /// </summary>
        /// <param name="groupMembersRef">group members page reference.</param>
        /// <param name="nextPageUrl">group members next page data link url.</param>
        /// <returns>group members page.</returns>
        Task<IGroupTransitiveMembersCollectionWithReferencesPage> GetGroupMembersNextPageAsnyc(IGroupTransitiveMembersCollectionWithReferencesPage groupMembersRef, string nextPageUrl);
    }
}
