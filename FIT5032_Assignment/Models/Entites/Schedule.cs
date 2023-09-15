using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        [Required]
        public DateTime TimeSlot { get; set; }
        [Required]
        public bool IsOccupied { get; set; }
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        public int? AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }

    }
}