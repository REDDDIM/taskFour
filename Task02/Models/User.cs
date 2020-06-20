using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task02.Models
{
    public class User
    {   
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public DateTime DateReg { get; set; }
        [Required]
        public DateTime LastLogin { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Password { get; set; }
    }
}