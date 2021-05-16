using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MailKit.Net.Smtp;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Ocsp;

namespace Lab4.Controllers
{
    public class MockupsController : Controller
    {
        public MockupsController()
        {

        }
        public IActionResult Mockups()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Reset(string code)
        {
            return View();
        }

        public IActionResult ResetVerify(string Code, string Email)
        {
            if (Request.Method == "GET")
                return View();
            string newcode = AccountData.GetResetCode(Email);
            if (Code == newcode)
            {
                return RedirectToAction("ChangePassword", new { Email = Email });
            }
            ViewData["letter"] = "Wrong code  " + Code + " " + newcode;
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword( string Email)
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string Password, string Email)
        {
            ViewData["Change"] ="  |  "+Email+" | " + AccountData.setNewPassword(Email, Password);
            return View();
        }


        [HttpPost]
        public IActionResult Reset(string Email, string Code)
        {
            string resetcode = AccountData.GetResetCode(Email);
            if (resetcode == "0")
            {
                ViewData["EmailError"] = "Email does not exist";
                return View();
            }
            if (Code != "I have a code")
            {
                var letter = SendEmail(Email, "Verify code", resetcode);
                ViewData["letter"] = letter;
            }
            ViewData["Email"] = ""+Email;
            ViewBag.Email = "" + Email;
            return RedirectToAction("ResetVerify", new { Email = Email  });
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            SetViewBagDayMonthsYear();
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(AccountData AD)
        {
            SetViewBagDayMonthsYear();
            if (ModelState["FirstName"].ValidationState == (ModelValidationState)2 &
                ModelState["LastName"].ValidationState == (ModelValidationState)2 &
                ModelState["Gender"].ValidationState == (ModelValidationState)2)
            {
                return RedirectToAction("SignUp2Page", AD);
            }
            return View(AD);
        }

        [HttpGet]
        public IActionResult SignUp2Page(AccountData AD)
        {
            return View(AD);
        }
        [HttpPost]
        public IActionResult SignUp2Page(AccountData AD,
            string Password1Confrim, string Passwor2Confrim, string Remember)
        {
            ViewData["EmailConfirm"] = "";
            if (ModelState["Email"].ValidationState != (ModelValidationState)2)
            {
                ViewData["EmailConfirm"] = "Enter the correct email address";
                return View();
            }
            if (AD.FindEmail())
            {
                ViewData["EmailConfirm"] =""+AD.Email+ "is used";
                return View();
            }
            if (Password1Confrim != Passwor2Confrim ||
                string.IsNullOrEmpty(Password1Confrim))
            {
                ViewData["PasswordConfirm"] = "Passwords do not match or are not specified";
                return View();
            }
            AD.Password = Password1Confrim;
            AD.SaveAccountData();
            return RedirectToAction("SignUpCredentials", AD);
        }


        public IActionResult SignUpCredentials(AccountData AD)
        {
            return View(AD);
        }
        public async Task SendEmail(string Email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация 4 Лаборатоной", "ha6n@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", Email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.AuthenticateAsync("ha6n@yandex.ru", "162534iva");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        private void SetViewBagMonths()
        {
            AccountData AD = new AccountData();
            ViewBag.Months = new SelectList(Date_str.Months(), "month"); ;
        }
        private void SetViewBagDayMonthsYear()
        {
            AccountData AD = new AccountData();
            ViewBag.Days = new SelectList(Date_str.Days(), "Days");
            ViewBag.Months = new SelectList(Date_str.Months(), "month"); ;
            ViewBag.Years = new SelectList(Date_str.Years(), "Years");
        }
    }
}