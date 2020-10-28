// <copyright file="OpenThreadInBrowserCommand.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Awful.Core.Utilities;
using Awful.Database.Entities;
using Windows.System;

namespace Awful.Windows.UI.Tools.Commands
{
    public class OpenThreadInBrowserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public async void Execute(object parameter)
        {
            if (parameter == null)
            {
                return;
            }

            if (parameter is AwfulThread thread)
            {
                var endpoint = string.Format(CultureInfo.InvariantCulture, EndPoints.GotoNewPostEndpoint, thread.ThreadId);
                await Launcher.LaunchUriAsync(new Uri(endpoint));
            }
        }
    }
}
