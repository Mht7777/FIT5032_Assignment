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
    // Custom validation attribute for a semicolon-separated list of email addresses.
    public class EmailListAttribute : ValidationAttribute
    {
        // Override the IsValid method from the base ValidationAttribute class.
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Convert the value object to a string, which should contain email addresses separated by semicolons.
            var emailList = Convert.ToString(value);

            // Split the emailList string into an array of individual email addresses.
            var emails = emailList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Iterate through each email address in the array.
            foreach (var email in emails)
            {
                try
                {
                    // Attempt to create a MailAddress object using the email address.
                    // The MailAddress constructor will throw a FormatException if the email address is invalid.
                    var addr = new System.Net.Mail.MailAddress(email);

                    // Check if the MailAddress object's Address property matches the original email string.
                    // If not, throw an exception to trigger the catch block below.
                    if (addr.Address != email) throw new Exception();
                }
                catch
                {
                    // If an exception occurs (due to an invalid email address),
                    // return a new ValidationResult object with an error message.
                    return new ValidationResult("Invalid email address: " + email);
                }
            }

            // If all email addresses are valid (i.e., no exceptions were thrown),
            // return a ValidationResult object that indicates success.
            return ValidationResult.Success;
        }
    }



}