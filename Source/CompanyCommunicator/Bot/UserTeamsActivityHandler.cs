// <copyright file="UserTeamsActivityHandler.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Bot
{
    using AdaptiveCards;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.NotificationData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.ReceivedNotificationData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Services.MicrosoftGraph;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Company Communicator User Bot.
    /// Captures user data, team data.
    /// </summary>
    public class UserTeamsActivityHandler : TeamsActivityHandler
    {
        private static readonly string TeamRenamedEventType = "teamRenamed";

        private readonly TeamsDataCapture teamsDataCapture;
        private readonly IReceivedNotificationDataRepository dataRep;
        private readonly ISendingNotificationDataRepository notificationRepo;
        private readonly IUsersService usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTeamsActivityHandler"/> class.
        /// </summary>
        /// <param name="teamsDataCapture">Teams data capture service.</param>
        /// <param name="dataRep">ReceivedNotificationDataEntity.</param>
        /// <param name="notificationRepo">Notifications repository</param>
        /// <param name="usersService">Adaptive card creator</param>
        public UserTeamsActivityHandler(TeamsDataCapture teamsDataCapture, IReceivedNotificationDataRepository dataRep, ISendingNotificationDataRepository notificationRepo, IUsersService usersService)
        {
            this.teamsDataCapture = teamsDataCapture ?? throw new ArgumentNullException(nameof(teamsDataCapture));
            this.dataRep = dataRep ?? throw new ArgumentNullException(nameof(dataRep));
            this.notificationRepo = notificationRepo ?? throw new ArgumentNullException(nameof(notificationRepo));
            this.usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        /// <inheritdoc/>
        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action, CancellationToken cancellationToken)
        {
            return await base.OnTeamsMessagingExtensionSubmitActionAsync(turnContext, action, cancellationToken);
        }

        /// <inheritdoc/>
        protected override async Task<InvokeResponse> OnInvokeActivityAsync(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            return await base.OnInvokeActivityAsync(turnContext, cancellationToken);
        }

        /// <inheritdoc/>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            DataJSON datajson = System.Text.Json.JsonSerializer.Deserialize<DataJSON>(turnContext.Activity.Value.ToString());
            await this.dataRep.EnsureReceivedNotificationDataTableExistsAsync();
            Graph.User user = await this.usersService.GetUserAsync(turnContext.Activity.From.AadObjectId);
            ReceivedNotificationDataEntity notification = new ReceivedNotificationDataEntity
            {
                RecipientId = turnContext.Activity.From.AadObjectId,
                RecipientName = user.DisplayName,
                RecipientMail = user.UserPrincipalName,
                Timestamp = turnContext.Activity.Timestamp.Value,
                ConversationId = datajson.notificationID,
                PartitionKey = datajson.notificationID,
                RowKey = turnContext.Activity.Id,
                ClickedUrl = datajson.url,
                ButtonId = datajson.buttonID,
            };
            await this.dataRep.InsertOrMergeAsync(notification);

            var notificationRep = await this.notificationRepo.GetAsync(
                NotificationDataTableNames.SendingNotificationsPartition,
                datajson.notificationID);

            /*
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(notificationRep.Content),
            };*/

            AdaptiveCardParseResult result = AdaptiveCard.FromJson(notificationRep.Content);
            AdaptiveCard card = result.Card;

            card.Actions.RemoveAt(card.Actions.Count - 1);
            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = turnContext.Activity.Locale == "es-ES" ? "Recepción confirmada" : "Reception confirmed",
                Color = AdaptiveTextColor.Good,
            });

            if (card.Actions.Count == 1)
            {
                var act = card.Actions[0] as AdaptiveOpenUrlAction;
                act.Url = new Uri(act.Url.ToString().Replace("[_AAID_]", turnContext.Activity.From.AadObjectId), UriKind.RelativeOrAbsolute);
                card.Actions[0] = act;
            }

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = JsonConvert.DeserializeObject(card.ToJson()),
            };

            var reply = MessageFactory.Attachment(adaptiveCardAttachment);
            reply.Id = turnContext.Activity.ReplyToId;
            try
            {
                await turnContext.UpdateActivityAsync(reply, cancellationToken);
            }
            catch (Exception ex)
            {
                await base.OnMessageActivityAsync(turnContext, cancellationToken);
            }
        }

        protected override async Task<InvokeResponse> OnTeamsCardActionInvokeAsync(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            return await base.OnInvokeActivityAsync(turnContext, cancellationToken);
        }

        protected override Task OnMessageReactionActivityAsync(ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            return base.OnMessageReactionActivityAsync(turnContext, cancellationToken);
        }

        protected override Task OnReactionsAddedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            return base.OnReactionsAddedAsync(messageReactions, turnContext, cancellationToken);
        }

        protected override Task OnReactionsRemovedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            return base.OnReactionsRemovedAsync(messageReactions, turnContext, cancellationToken);
        }

        /// <summary>
        /// Invoked when a conversation update activity is received from the channel.
        /// </summary>
        /// <param name="turnContext">The context object for this turn.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        protected override async Task OnConversationUpdateActivityAsync(
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {
            // base.OnConversationUpdateActivityAsync is useful when it comes to responding to users being added to or removed from the conversation.
            // For example, a bot could respond to a user being added by greeting the user.
            // By default, base.OnConversationUpdateActivityAsync will call <see cref="OnMembersAddedAsync(IList{ChannelAccount}, ITurnContext{IConversationUpdateActivity}, CancellationToken)"/>
            // if any users have been added or <see cref="OnMembersRemovedAsync(IList{ChannelAccount}, ITurnContext{IConversationUpdateActivity}, CancellationToken)"/>
            // if any users have been removed. base.OnConversationUpdateActivityAsync checks the member ID so that it only responds to updates regarding members other than the bot itself.
            await base.OnConversationUpdateActivityAsync(turnContext, cancellationToken);

            var activity = turnContext.Activity;

            var isTeamRenamed = this.IsTeamInformationUpdated(activity);
            if (isTeamRenamed)
            {
                await this.teamsDataCapture.OnTeamInformationUpdatedAsync(activity);
            }

            if (activity.MembersAdded != null)
            {
                await this.teamsDataCapture.OnBotAddedAsync(activity);
            }

            if (activity.MembersRemoved != null)
            {
                await this.teamsDataCapture.OnBotRemovedAsync(activity);
            }
        }

        private bool IsTeamInformationUpdated(IConversationUpdateActivity activity)
        {
            if (activity == null)
            {
                return false;
            }

            var channelData = activity.GetChannelData<TeamsChannelData>();
            if (channelData == null)
            {
                return false;
            }

            return UserTeamsActivityHandler.TeamRenamedEventType.Equals(channelData.EventType, StringComparison.OrdinalIgnoreCase);
        }

        private class DataJSON
        {
            public string url { get; set; }
            public string buttonID { get; set; }
            public string notificationID { get; set; }
        };
    }
}