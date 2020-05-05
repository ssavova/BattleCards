using BattleCards.Services;
using BattleCards.ViewModels.Users;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace BattleCards.Controllers
{
    public class UsersController :Controller
    {
        private readonly IUsersService service;

        public UsersController(UsersService service)
        {
            this.service = service;
        }


        public HttpResponse Login()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/Cards/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
           
            var userId = this.service.GetUserId(username, password);

            if (userId == null)
            {
                return this.Redirect("/Users/Register");
            }

            this.SignIn(userId);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse Register()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/Cards/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {

            if (input.Password != input.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            if (input.Username?.Length < 5 || input.Username?.Length > 20)
            {
                return this.Redirect("/Users/Register");
            }

            if (input.Password?.Length < 6 || input.Password?.Length > 20)
            {
                return this.Redirect("/Users/Register");
            }

            if (!IsValid(input.Email))
            {
                return this.Redirect("/Users/Register");
            }

            if (this.service.IsUsernameUsed(input.Username))
            {
                return this.Redirect("/Users/Register");
            }

            if (this.service.IsEmailUsed(input.Email))
            {
                return this.Redirect("/Users/Register");
            }

            this.service.CreateUser(input.Username, input.Email, input.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.SignOut();
            return this.Redirect("/");
        }

        private bool IsValid(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
