// <copyright file="SentNotificationDataEntity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.ReceivedNotificationData
{
    using Microsoft.Azure.Cosmos.Table;
    using System;

    /// <summary>
    /// Received notification entity class.
    /// This entity holds all of the information about a recipient and the results for
    /// a notification having been reveived to that recipient.
    /// </summary>
    public class ReceivedNotificationDataEntity : TableEntity
    {
        /// <summary>
        /// This value is to be used when the entity is first initialized and stored and does
        /// not yet have a valid status code from a response for an attempt at sending the
        /// notification to the recipient.
        /// </summary>
        public static readonly int InitializationStatusCode = 0;

        /// <summary>
        /// This value indicates that the Azure Function that attempted to process the queue message
        /// threw an exception. Because of this, this temporary status code is stored because
        /// the function will re-queue the queue message and try to process the queue message
        /// again. If the message fails to be processed enough times, then a different status
        /// code will be stored.
        /// </summary>
        public static readonly int FaultedAndRetryingStatusCode = -1;

        /// <summary>
        /// This value indicates that the Azure Function that attempted to process the queue message
        /// has failed to process the queue message enough times and thrown enough exceptions that
        /// the queue message will be placed on the dead letter queue. In this state, the queue
        /// message will not be attempted again. Because of this, this final faulted status code
        /// will be stored to indicate this final faulted state.
        /// </summary>
        public static readonly int FinalFaultedStatusCode = -2;

        /// <summary>
        /// String indicating the recipient type for the given notification was a user.
        /// </summary>
        public static readonly string UserRecipientType = "User";

        /// <summary>
        /// String indicating the recipient type for the given notification was a team.
        /// </summary>
        public static readonly string TeamRecipientType = "Team";

        /// <summary>
        /// String indicating success of sending the notification to the recipient.
        /// </summary>
        public static readonly string Succeeded = "Succeeded";

        /// <summary>
        /// String indicating a recipient is not found when sending the notification to
        /// the recipient.
        /// </summary>
        public static readonly string RecipientNotFound = "RecipientNotFound";

        /// <summary>
        /// String indicating a failure response was received when sending the notification to
        /// the recipient.
        /// </summary>
        public static readonly string Failed = "Failed";

        /// <summary>
        /// [Deprecated] String indicating sending the notification to the recipient was throttled
        /// and not sent successfully.
        /// </summary>
        public static readonly string Throttled = "Throttled";

        /// <summary>
        /// String indicating that processing the current queue message resulted in an exception so
        /// the message is being re-queued and attempted again. Because of this, this string will be
        /// stored in the repository as the delivery status until a more final state is reached.
        /// </summary>
        public static readonly string Retrying = "Retrying";

        /// <summary>
        /// Gets or sets a value indicating the name of recipient the notification was sent to
        /// using the recipient type strings at the top of this class.
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the email of recipient the notification was sent to
        /// using the recipient type strings at the top of this class.
        /// </summary>
        public string RecipientMail { get; set; }

        /// <summary>
        /// Gets or sets the recipient's unique identifier.
        ///     If the recipient is a user, this should be the AAD Id.
        ///     If the recipient is a team, this should be the team Id.
        /// </summary>
        public string RecipientId { get; set; }

        /// <summary>
        /// Gets or sets the DateTime the last recorded attempt at sending the notification to this
        /// recipient was completed.
        /// </summary>
        public DateTime? ReceivedDate { get; set; }

        /// <summary>
        /// Gets or sets the conversation id for the recipient.
        /// </summary>
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets the clicked URL for the recipient.
        /// </summary>
        public string ClickedUrl { get; set; }

        /// <summary>
        /// Gets or sets the button id for the recipient.
        /// </summary>
        public string ButtonId { get; set; }
    }
}
