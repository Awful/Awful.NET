// <copyright file="CookieContainerExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections;
using System.Net;
using System.Reflection;

namespace Awful.Core.Utilities
{
    /// <summary>
    /// Cookie Container Extesnions.
    /// </summary>
    internal static class CookieContainerExtensions
    {
        /// <summary>
        /// Get all cookies from cookie container.
        /// </summary>
        /// <param name="container">Cookie Container.</param>
        /// <returns>CookieCollection.</returns>
        internal static CookieCollection GetAllCookies(this CookieContainer container)
        {
            CookieCollection allCookies = new CookieCollection();
            Hashtable? domainTable = container.GetType()
                .GetRuntimeFields()
                .First(x => x.Name == "m_domainTable")
                .GetValue(container) as Hashtable;

            if (domainTable == null)
            {
                return new CookieCollection();
            }

            FieldInfo? pathListField = null;
            foreach (object domain in domainTable.Values)
            {
                SortedList? pathList = (pathListField ??= domain.GetType()
                    .GetRuntimeFields()
                    .First(x => x.Name == "m_list"))
                    .GetValue(domain) as SortedList;

                if (pathList is null)
                {
                    continue;
                }

                foreach (CookieCollection cookies in pathList.GetValueList())
                {
                    allCookies.Add(cookies);
                }
            }

            return allCookies;
        }
    }
}
