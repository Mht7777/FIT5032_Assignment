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

        // Foreign Key for Clinic
        public int ClinicId { get; set; }

        // Navigation Property for Clinic
        public virtual Clinic Clinic { get; set; }

        // User ID from AspNetUser table.
        public string UserId { get; set; }

    }

}