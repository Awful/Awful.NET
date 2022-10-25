// <copyright file="ThreadTag.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Reflection;

namespace Awful.ThreadTags
{
    /// <summary>
    /// Awful Thread Tags.
    /// </summary>
    public class ThreadTag
    {
        private string[] imageNames;

        public ThreadTag()
        {
            this.imageNames = typeof(ThreadTag).GetTypeInfo().Assembly.GetManifestResourceNames();
        }

        public byte[] GetThreadTag(string name)
        {
            var image = $"Awful.ThreadTags.{name}.png";

            if (!this.imageNames.Contains(image))
            {
                image = "Awful.ThreadTags.missing.png";
            }

            using (Stream stream = typeof(ThreadTag).GetTypeInfo().Assembly.GetManifestResourceStream(image))
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public string[] ImageNames => this.imageNames;
    }
}
