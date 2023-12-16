using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SSL.Models;

namespace SSL
{
    public class RoleConfig
    {
        public static void RegisterRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            roleManager.Create(new IdentityRole("Gold"));
            roleManager.Create(new IdentityRole("Silver"));
            roleManager.Create(new IdentityRole("Free"));
        }
    }
}