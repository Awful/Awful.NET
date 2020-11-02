// <copyright file="FlyoutHelper.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Awful.Windows.UI.Helpers
{
    /// <summary>
    /// Flyout Helper.
    /// </summary>
    public static class FlyoutHelper
    {
        /// <summary>
        /// Is Visible Property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached(
            "IsOpen", typeof(bool), typeof(FlyoutHelper), new PropertyMetadata(true, IsOpenChangedCallback));

        /// <summary>
        /// Parent Property.
        /// </summary>
        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.RegisterAttached(
            "Parent", typeof(FrameworkElement), typeof(FlyoutHelper), null);

        /// <summary>
        /// Set Is Open.
        /// </summary>
        /// <param name="element">The Element to Open.</param>
        /// <param name="value">The value to open.</param>
        public static void SetIsOpen(DependencyObject element, bool value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(IsVisibleProperty, value);
        }

        /// <summary>
        /// Get is open.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <returns>Boolean.</returns>
        public static bool GetIsOpen(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(IsVisibleProperty);
        }

        /// <summary>
        /// Set parent element.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="value">Parent.</param>
        public static void SetParent(DependencyObject element, FrameworkElement value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(ParentProperty, value);
        }

        /// <summary>
        /// Get parent element.
        /// </summary>
        /// <param name="element">Element to get parent of.</param>
        /// <returns>Parent Element.</returns>
        public static FrameworkElement GetParent(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (FrameworkElement)element.GetValue(ParentProperty);
        }

        private static void IsOpenChangedCallback(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var fb = d as FlyoutBase;
            if (fb == null)
            {
                return;
            }

            try
            {
                if ((bool)e.NewValue)
                {
                    fb.Closed += Flyout_Closed;
                    fb.ShowAt(GetParent(d));
                }
                else
                {
                    fb.Closed -= Flyout_Closed;
                    fb.Hide();
                }
            }
            catch (Exception)
            {

            }
        }

        private static void Flyout_Closed(object sender, object e)
        {
            // When the flyout is closed, sets its IsOpen attached property to false.
            SetIsOpen(sender as DependencyObject, false);
        }
    }
}