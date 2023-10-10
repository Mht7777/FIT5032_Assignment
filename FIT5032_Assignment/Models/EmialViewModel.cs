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
        [Display(Name ="Email")]
        [EmailAddress(ErrorMessage ="Invaild Emial")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Plase input subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Plase input contents")]
        [AllowHtml]
        public string Contents { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
    }
}