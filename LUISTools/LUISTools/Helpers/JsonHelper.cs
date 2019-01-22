// ***********************************************************************
// <copyright file="JsonHelper.cs">
//     Copyright (c) CBC Ltd. All rights reserved.
// </copyright>
// <summary>Help for JSON</summary>
// ***********************************************************************

namespace LUISTools.Helpers
{
    using System.IO;
    using Newtonsoft.Json;

    /// <summary>
    /// Json support.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Serializes to stream.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="stream">The stream to receive serialized data.</param>
        public static void SerializeToStream(object value, Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, value);
                jsonWriter.Flush();
            }
        }

        /// <summary>
        /// Deserializes from stream.
        /// </summary>
        /// <typeparam name="T">type to serialize to.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>Rehydrated object of T.</returns>
        public static T DeserializeFromStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
