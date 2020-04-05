using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkersApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.{8,})(?=.*[a-z])(?=.*[0-9])(?=.*[A-Z]).*$", ErrorMessage = "Password must contain at least 1 numeric character, 1 special character, 1 uppercase and 1 lowercase alphabetical character ")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool IsSeller { get; set; }
    }
}
