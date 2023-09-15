using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class FeedbackAndRating
    {
        [Key, ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
        public string Comment { get; set; }
        public virtual Appointment Appointment { get; set; }



    }

    
}