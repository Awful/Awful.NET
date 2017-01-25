using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text;

namespace Mazui.Core.Models.Users
{
    public class UserAuth
    {
        [Key]
        public int UserAuthId { get; set; }
        public string UserName { get; set; }

        public string AvatarLink { get; set; }

        public string CookiePath { get; set; }

        public CookieContainer AuthCookies { get; set; }

        public bool IsDefaultUser { get; set; }
    }
}
