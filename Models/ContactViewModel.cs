using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LesserToUmbraco.Models
{
    public class ContactViewModel
    {
        [Required (ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Plase enter your email")]
        [EmailAddress(ErrorMessage = "please enter a valid email address")]
        public string Email { get; set; }

        [Required] [MaxLength(500, ErrorMessage = "Your message must be 500 chars or less")]
        public string Message { get; set; }

    }
}