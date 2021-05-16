using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Lab4.Models
{
    public class AccountData
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [StringLength(50, ErrorMessage = "Must be under 50 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        [StringLength(50, ErrorMessage = "Must be under 50 characters")]
        public string LastName { get; set; }
        public string BDay { get; set; }
        public string BMonth { get; set; }
        public string BYear { get; set; }
        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Must be under 20 characters")]
        public string Password { get; set; }
        public bool Remember { get; set; }
        public string ResetCode { get; set; }

        private ApplicationContext db;

        public string Create(AccountData AD)
        {
            string message = "Successful";
            return message;
        }
        public AccountData()
        {
            setResetCode();
            db = new ApplicationContext();
        }

        public void SaveAccountData()
        {
            db.accountDatas.Add(this);
            db.SaveChanges();
        }

        private void setResetCode()
        {
            Random rand = new Random();
            ResetCode = "";
            for (int i = 0; i < 5; i++)
            {
                ResetCode += "" + rand.Next(10);
            }
        }

        public bool FindEmail()
        {
            var MYemail = db.accountDatas.Where(p =>EF.Functions.Like(p.Email, this.Email));
            foreach (var item in MYemail)
                if (item.Email == this.Email)
                    return true;
            return false;
        }

        public static string GetResetCode(string Email)
        {
            ApplicationContext db = new ApplicationContext();
            var MYemail = db.accountDatas.Where(p => EF.Functions.Like(p.Email, Email));
            foreach (var item in MYemail)
            {
                if (item.Email == Email)
                    return item.ResetCode;
            }
            return "0";
        }
        public static bool setNewPassword(string Email, string Password)
        {

            ApplicationContext db = new ApplicationContext();
            var MYemail = db.accountDatas.Where(p => EF.Functions.Like(p.Email, Email));
            foreach (var item in MYemail)
            {
                if (item.Email == Email)
                {
                    item.Password = Password;
                    return true;
                }
            }
            return false;
        }


    }

}
