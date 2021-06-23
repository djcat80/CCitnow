// <copyright file="DraftNotificationsController.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.ReceivedNotificationData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Services.MicrosoftGraph;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Controller for the click capture and redirect.
    /// </summary>
    [Route("api/redirect")]

    // [Authorize(PolicyNames.MustBeValidUpnPolicy)]
    public class RedirectController : ControllerBase
    {
        private readonly IReceivedNotificationDataRepository dataRep;
        private readonly IUsersService usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectController"/> class.
        /// </summary>
        /// <param name="dataRep">ReceivedNotificationDataEntity.</param>
        /// <param name="usersService">Users service</param>
        public RedirectController(IReceivedNotificationDataRepository dataRep, IUsersService usersService)
        {
            this.dataRep = dataRep ?? throw new ArgumentNullException(nameof(dataRep));
            this.usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        /// <summary>
        /// Register the click an then redirects to url.
        /// </summary>
        /// <param name="redirectUrl">The url to redirect after proccess.</param>
        /// <param name="notificationID">The ID of the notification.</param>
        /// <param name="userID">The AAID of the user.</param>
        /// <param name="buttonID">The number of the button clicked.</param>
        /// <returns>A redirect.</returns>
        [HttpGet]
        public async Task<RedirectResult> RegisterRedirectAsync([FromQuery(Name = "redirectUrl")] string redirectUrl, [FromQuery(Name = "notificationID")] string notificationID, [FromQuery(Name = "userID")] string userID, [FromQuery(Name = "buttonID")] string buttonID)
        {
            var headers = this.Request.Headers;
            Graph.User user = await this.usersService.GetUserAsync(userID);
            await this.dataRep.EnsureReceivedNotificationDataTableExistsAsync();
            ReceivedNotificationDataEntity notification = new ReceivedNotificationDataEntity
            {
                RecipientId = userID,
                Timestamp = DateTime.UtcNow,
                ConversationId = notificationID,
                PartitionKey = notificationID,
                RowKey = notificationID + DateTime.UtcNow.ToShortTimeString(),
                ClickedUrl = redirectUrl,
                ButtonId = buttonID,
                RecipientName = user.DisplayName,
                RecipientMail = user.UserPrincipalName,
            };
            await this.dataRep.InsertOrMergeAsync(notification);
            return this.Redirect(redirectUrl);
        }

        [HttpPost]
        public RedirectResult RegisterRedirectPost()
        {
            var headers = this.Request.Headers;
            return this.Redirect("ok");
        }
    }
}
