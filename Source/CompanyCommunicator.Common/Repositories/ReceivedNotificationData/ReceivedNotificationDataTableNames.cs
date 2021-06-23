namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.ReceivedNotificationData
{
    /// <summary>
    /// Received Notification data table names.
    /// </summary>
    public static class ReceivedNotificationDataTableNames
    {
        /// <summary>
        /// Table name for the sent notification data table.
        /// </summary>
        public static readonly string TableName = "ReceivedData";

        /// <summary>
        /// Default partition - should not be used.
        /// </summary>
        public static readonly string DefaultPartition = "Default";
    }
}
