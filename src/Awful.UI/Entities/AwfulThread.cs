// <copyright file="AwfulThread.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Awful.Entities.Threads;
using Force.DeepCloner;

namespace Awful.UI.Entities
{
    /// <summary>
    /// Awful Database Thread.
    /// </summary>
    public class AwfulThread : Awful.Entities.Threads.Thread, INotifyPropertyChanged
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulThread"/> class.
        /// </summary>
        public AwfulThread()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulThread"/> class.
        /// </summary>
        /// <param name="parent">API Thread.</param>
        public AwfulThread(Awful.Entities.Threads.Thread parent)
        {
            parent.DeepCloneTo(this);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable notifications for updates.
        /// </summary>
        public bool EnableBookmarkNotifications { get; set; }

        /// <summary>
        /// Gets or sets the Forum Id.
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// Gets or sets the Sort Order.
        /// </summary>
        public int SortOrder { get; set; }

        public void Update(AwfulThread parent)
        {
            parent.DeepCloneTo(this);
            OnPropertyChanged();
        }

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
