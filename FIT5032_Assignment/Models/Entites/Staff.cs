using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        public int ClinicId { get; set; }

        public virtual Clinic Clinic { get; set; }

        public string UserId { get; set; }

    }

}