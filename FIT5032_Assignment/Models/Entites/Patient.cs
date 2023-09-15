using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }

}