// <copyright file="Binary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Awful.Core.Utilities
{
    /// <summary>
    /// Binary Extensions.
    /// </summary>
    internal static class Binary
    {
        /// <summary>
        /// Convert an object to a Byte Array.
        /// </summary>
        /// <param name="objData">Object Data.</param>
        /// <returns>Byte Array.</returns>
        internal static byte[]? ObjectToByteArray(object objData)
        {
            if (objData == null)
            {
                return default;
            }

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(objData, GetJsonSerializerOptions()));
        }

        /// <summary>
        /// Convert a byte array to an Object of T.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>T.</returns>
        internal static T? ByteArrayToObject<T>(byte[] byteArray)
        {
            if (byteArray == null || !byteArray.Any())
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(byteArray, GetJsonSerializerOptions());
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}