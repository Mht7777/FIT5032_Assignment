using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Assignment.Models.Entites
{
    public class Images
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Path { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "File Name")]
        public string Name { get; set; }
        public int AppointmentId { get; set; }


    }
}