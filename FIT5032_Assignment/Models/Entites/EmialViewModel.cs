using FIT5032_Assignment.Models.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using FIT5032_Assignment.Utils;

namespace FIT5032_Assignment.Models
{
    public class EmailViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter at least one email address.")]
        [EmailList(ErrorMessage = "Invalid Email")]
        public string Emails { get; set; }

        [Required(ErrorMessage = "Please input subject")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid subject format. Only letters, numbers, and spaces are allowed.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please input contents")] 
        [AllowHtml]
        [RegularExpression(@"^[a-zA-Z0-9\s.,!?()\-_+=@#$%^&*<>:;'""[\]{}\\/]*$", 
            ErrorMessage = "Invalid contents format. Only letters, numbers, and common punctuation are allowed.")]
        public string Contents { get; set; }

        public HttpPostedFileBase Attachment { get; set; }
    }



}