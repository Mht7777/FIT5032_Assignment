using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Clinic
    {
        public Clinic()
        {
            Appointments = new HashSet<Appointment>();
            StaffMembers = new HashSet<Staff>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string ClinicName { get; set; }
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Staff> StaffMembers { get; set; }
    }

}