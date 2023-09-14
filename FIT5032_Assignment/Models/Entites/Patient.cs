using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Patient
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}