// <copyright file="ISentNotificationDataRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.ReceivedNotificationData
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Sent Notification data Repository.
    /// </summary>
    public interface IReceivedNotificationDataRepository : IRepository<ReceivedNotificationDataEntity>
    {
        /// <summary>
        /// This method ensures the ReceivedNotificationData table is created in the storage.
        /// This method should be called before kicking off an Azure function that uses the ReceivedNotificationData table.
        /// Otherwise the app will crash.
        /// By design, Azure functions (in this app) do not create a table if it's absent.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public Task EnsureReceivedNotificationDataTableExistsAsync();
    }
}
