// <copyright file="ForumListDataTemplateSelector.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.Core.Entities.JSON;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Templates
{
    /// <summary>
    /// Forum List Data Template Selector.
    /// </summary>
    public class ForumListDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CategoryTemplate { get; set; }

        public DataTemplate ForumTemplate { get; set; }

        /// <inheritdoc/>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item is Forum { HasThreads: false } ? this.CategoryTemplate : this.ForumTemplate;
        }
    }
}