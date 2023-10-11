using FIT5032_Assignment.Models.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FIT5032_Assignment.Models
{
    public class EmailViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter at least one email address.")]
        [EmailList(ErrorMessage = "Invalid Email")]
        public string Emails { get; set; }

        [Required(ErrorMessage = "Please input subject")] 
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please input contents")] 
        [AllowHtml]
        public string Contents { get; set; }

        public HttpPostedFileBase Attachment { get; set; }
    }
    public class EmailListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var emailList = Convert.ToString(value);
            var emails = emailList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var email in emails)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    if (addr.Address != email) throw new Exception();
                }
                catch
                {
                    return new ValidationResult("Invalid email address: " + email);
                }
            }

            return ValidationResult.Success;
        }
    }


}