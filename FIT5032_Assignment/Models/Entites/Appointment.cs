using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        [Required]
        public string PatientName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ScanPart { get; set; }
        [Required]
        public string Gender { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Status { get; set; }

        public string UserID { get; set; }


        public virtual Clinic Clinic { get; set; }
        public virtual FeedbackAndRating Feedback { get; set; }


    }
}