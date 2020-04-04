using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkersApp.Models
{
    public class Worker
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        [Range(0, 90, ErrorMessage = "Can only be between 0 .. 90")]
        public int Experience { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}
