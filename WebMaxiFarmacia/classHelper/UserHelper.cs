﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using WebMaxiFarmacia.Models;

namespace WebMaxiFarmacia.classHelper
{
    public class UserHelper
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();

        private static maxifarmaciabdContext db = new maxifarmaciabdContext();

        public static bool DeleteUser(string NombreUser)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userAsp = userManager.FindByEmail(NombreUser);

            if (NombreUser == null)
            {
                return false;
            }

            var response =  userManager.Delete(userAsp);

            return response.Succeeded;
        }

        public static bool UpdateUser(string oldUser, string newUser)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userAsp = userManager.FindByEmail(oldUser);

            if (oldUser == null)
            {
                return false;
            }

            userAsp.UserName = newUser;
            userAsp.Email = newUser;

            var response = userManager.Update(userAsp);

            return response.Succeeded;

        }

        public static void CheckRole(string roleName)

        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check to see if Role Exists, if not create it

            if (!roleManager.RoleExists(roleName))

            {

                roleManager.Create(new IdentityRole(roleName));

            }

        }

        public static void CheckSuperUser()

        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var email = WebConfigurationManager.AppSettings["AdminUser"];

            var password = WebConfigurationManager.AppSettings["AdminPassWord"];

            var userASP = userManager.FindByName(email);

            if (userASP == null)

            {

                CreateUserASP(email, "SuperAdmin", password);

                return;

            }

            userManager.AddToRole(userASP.Id, "SuperAdmin");

        }

        public static void CreateUserASP(string email, string roleName)

        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser

            {

                Email = email,

                UserName = email,

            };

            userManager.Create(userASP, email);

            userManager.AddToRole(userASP.Id, roleName);

        }

        public static void CreateUserASP(string email, string roleName, string password)

        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser

            {

                Email = email,

                UserName = email,

            };

            userManager.Create(userASP, password);

            userManager.AddToRole(userASP.Id, roleName);

        }

        public static async Task PasswordRecovery(string email)

        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = userManager.FindByEmail(email);

            if (userASP == null)

            {

                return;

            }

            var user = db.Users.Where(tp => tp.NombreUser == email).FirstOrDefault();

            if (user == null)

            {

                return;

            }

            var random = new Random();

            var newPassword = random.Next(10000000).ToString();

            //var newPassword = string.Format("{0}{2:04}*",

            ////user.NombreUser.Trim().ToUpper().Substring(0, 1),

            ////user.NombreUser.Trim().ToLower(),

            //random.Next(10000));

            userManager.RemovePassword(userASP.Id);

            userManager.AddPassword(userASP.Id, newPassword);

            var subject = "MaxiFarm-Soporte";

            var body = string.Format(@"
                                        <h1>Recuperacion de  Contraseña</h1>

                                        <p>Tu nueva Contraseña es: <strong>{0}</strong></p>

                                        <p>Puedes iniciar sesion con la nueva contraseña",

            newPassword);

            await MailHelper.SendMail(email, subject, body);

        }

        //public void Dispose()

        //{

        //    userContext.Dispose();

        //    db.Dispose();

        //}

    }
}
