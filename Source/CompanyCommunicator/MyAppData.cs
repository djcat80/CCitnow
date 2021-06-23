// <copyright file="MyAppData.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Class created for storing configuration for using it in all classes
    /// </summary>
    public static class MyAppData
    {
        /// <summary>
        /// Object where we store the configuration
        /// </summary>
        public static IConfiguration configuration;
    }
}
