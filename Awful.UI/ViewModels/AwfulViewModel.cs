// <copyright file="AwfulViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Database;
using Awful.UI.Interfaces;

namespace Awful.UI.ViewModels
{
    /// <summary>
    /// Awful View Model.
    /// </summary>
    public class AwfulViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulViewModel"/> class.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="error">Error Handler.</param>
        /// <param name="navigation">Navigation.</param>
        public AwfulViewModel(
            IDatabase database,
            IAwfulErrorHandler error,
            IAwfulNavigationHandler navigation)
            : base(database, error, navigation)
        {
        }
    }
}