using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


    }


}