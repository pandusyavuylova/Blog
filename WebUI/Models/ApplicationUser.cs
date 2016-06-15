using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationUser: IdentityUser
    {
        //public ApplicationRole Role { get; set; }
        public ApplicationUser()
        {
        }
    }
}