using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Simulacao.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50, ErrorMessage = "The max length is 50")]
        public string Location { get; set; }
        [Display(Name = "Professional Summary")]
        public string ProfessionalSummary { get; set; }
        [MinLength(0)]
        [StringLength(10)]
        public string Identity { get; set; }
        [StringLength(10)]
        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [Display(Name = "User Photo")]
        public byte[] UserPhoto { get; set; }
        [Display(Name = "Full Name")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Required]
        public string FullName { get; set; }
    }
}
