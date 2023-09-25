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
        public int PatientId { get; set; } 
        public virtual Patient Patient { get; set; } 

        [Required]
        public string ScanPart { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime EndTime { get; set; }


        public string UserId { get; set; }  

        public virtual Clinic Clinic { get; set; }
        public virtual FeedbackAndRating Feedback { get; set; }
    }

}