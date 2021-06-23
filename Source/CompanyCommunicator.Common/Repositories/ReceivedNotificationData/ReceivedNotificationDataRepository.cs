// <copyright file="SentNotificationDataRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.ReceivedNotificationData
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;

    /// <summary>
    /// Repository of the Received data in the table storage.
    /// </summary>
    public class ReceivedNotificationDataRepository : BaseRepository<ReceivedNotificationDataEntity>, IReceivedNotificationDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedNotificationDataRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public ReceivedNotificationDataRepository(
            ILogger<ReceivedNotificationDataRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: ReceivedNotificationDataTableNames.TableName,
                  defaultPartitionKey: ReceivedNotificationDataTableNames.DefaultPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task EnsureReceivedNotificationDataTableExistsAsync()
        {
            var exists = await this.Table.ExistsAsync();
            if (!exists)
            {
                await this.Table.CreateAsync();
            }
        }
    }
}
