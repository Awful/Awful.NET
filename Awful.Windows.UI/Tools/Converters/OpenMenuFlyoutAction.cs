// <copyright file="OpenMenuFlyoutAction.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Awful.Windows.UI.Tools.Converters
{
    /// <summary>
    /// Open Menu Flyout Action.
    /// </summary>
    public class OpenMenuFlyoutAction : DependencyObject, IAction
    {
        /// <inheritdoc/>
        public object Execute(object sender, object parameter)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);

            return null;
        }
    }
}
