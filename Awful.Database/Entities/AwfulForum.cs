// <copyright file="AwfulForum.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Awful.Core.Entities.JSON;
using Force.DeepCloner;

namespace Awful.Database.Entities
{
    /// <summary>
    /// Awful Forum.
    /// </summary>
    public class AwfulForum : Forum, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulForum"/> class.
        /// </summary>
        public AwfulForum()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulForum"/> class.
        /// </summary>
        /// <param name="parent">API Forum.</param>
        public AwfulForum(Forum parent)
        {
            parent.DeepCloneTo(this);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = this.PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
