﻿// <copyright file="NotificationRepositoryExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Repositories.Extensions
{
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.NotificationData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Models;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for the repository of the notification data.
    /// </summary>
    public static class NotificationRepositoryExtensions
    {
        /// <summary>
        /// Create a new draft notification.
        /// </summary>
        /// <param name="notificationRepository">The notification repository.</param>
        /// <param name="notification">Draft Notification model class instance passed in from Web API.</param>
        /// <param name="userName">Name of the user who is running the application.</param>
        /// <returns>The newly created notification's id.</returns>
        public static async Task<string> CreateDraftNotificationAsync(
            this INotificationDataRepository notificationRepository,
            DraftNotification notification,
            string userName)
        {
            var newId = notificationRepository.TableRowKeyGenerator.CreateNewKeyOrderingOldestToMostRecent();

            var notificationEntity = new NotificationDataEntity
            {
                PartitionKey = NotificationDataTableNames.DraftNotificationsPartition,
                RowKey = newId,
                Id = newId,
                Title = notification.Title,
                ImageLink = notification.ImageLink,
                Summary = notification.Summary,
                Author = notification.Author,
                ButtonTitle = notification.ButtonTitle,
                ButtonLink = notification.ButtonLink,
                ButtonTitle2 = notification.ButtonTitle2,
                ButtonLink2 = notification.ButtonLink2,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow,
                IsDraft = true,
                IsScheduled = notification.IsScheduled,
                IsImportant = notification.IsImportant,
                Teams = notification.Teams,
                Rosters = notification.Rosters,
                Groups = notification.Groups,
                AllUsers = notification.AllUsers,
                ListUsers = notification.ListUsers,
                CsvUsers = notification.CsvUsers,
                ScheduledDate = notification.ScheduledDate,
            };

            await notificationRepository.CreateOrUpdateAsync(notificationEntity);

            return newId;
        }
    }
}
