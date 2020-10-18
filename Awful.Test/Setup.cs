// <copyright file="Setup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Awful.Core.Utilities;
using Awful.Database.Context;

namespace Awful.Test
{
    /// <summary>
    /// Setting up user tests.
    /// </summary>
    public static class Setup
    {
        /// <summary>
        /// Setup Awful Database Context.
        /// </summary>
        /// <param name="properties">Test Properties.</param>
        /// <returns>AwfulContext.</returns>
        public static AwfulContext SetupContext(TestPlatformProperties properties)
        {
            return new AwfulContext(properties);
        }

        /// <summary>
        /// Setup Web Client.
        /// </summary>
        /// <param name="awfulUser">An Awful User.</param>
        /// <returns>An AwfulClient.</returns>
        public static async Task<AwfulClient> SetupWebClient(AwfulUser awfulUser)
        {
            string username;
            string password;
            string cookiePath;
            switch (awfulUser)
            {
                case AwfulUser.Standard:
                    username = Environment.GetEnvironmentVariable("AWFUL_USER");
                    password = Environment.GetEnvironmentVariable("AWFUL_PASSWORD");
                    cookiePath = "user.cookies";
                    break;
                case AwfulUser.Platinum:
                    username = Environment.GetEnvironmentVariable("AWFUL_PLATINUM_USER");
                    password = Environment.GetEnvironmentVariable("AWFUL_PLATINUM_PASSWORD");
                    cookiePath = "user.platinum.cookies";
                    break;
                case AwfulUser.Probation:
                    username = Environment.GetEnvironmentVariable("AWFUL_PROB_USER");
                    password = Environment.GetEnvironmentVariable("AWFUL_PROB_PASSWORD");
                    cookiePath = "user.prob.cookies";
                    break;
                case AwfulUser.Unauthenticated:
                default:
                    return new AwfulClient();
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Username/Password is missing from environment variables!");
            }

            if (!File.Exists(cookiePath))
            {
                var webClient = new AwfulClient();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return webClient;
                }

                var authManager = new AuthenticationManager(webClient);
                var result = await authManager.AuthenticateAsync(username, password).ConfigureAwait(false);
                if (!result.IsSuccess)
                {
                    throw new Exception("Could not log in!");
                }

                using (FileStream stream = File.Create(cookiePath))
                {
                    var formatter = new BinaryFormatter();
                    System.Console.WriteLine("Serializing cookie container");
                    formatter.Serialize(stream, webClient.CookieContainer);
                }

                return webClient;
            }
            else
            {
                System.Net.CookieContainer cookieContainer;
                using (FileStream stream = File.OpenRead(cookiePath))
                {
                    var formatter = new BinaryFormatter();
                    System.Console.WriteLine("Deserializing cookie container");
                    cookieContainer = (System.Net.CookieContainer)formatter.Deserialize(stream);
                    return new AwfulClient(cookieContainer);
                }
            }
        }
    }
}
