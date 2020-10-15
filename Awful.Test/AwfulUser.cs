using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Test
{
    /// <summary>
    /// Awful user, set up for tests.
    /// </summary>
    public enum AwfulUser
    {
        /// <summary>
        /// Default, an unauthenticated user.
        /// </summary>
        Unauthenticated,

        /// <summary>
        /// A standard user.
        /// </summary>
        Standard,

        /// <summary>
        /// A user with a platinum upgrade.
        /// </summary>
        Platinum,

        /// <summary>
        /// A user who has been put under probation.
        /// </summary>
        Probation,
    }
}
