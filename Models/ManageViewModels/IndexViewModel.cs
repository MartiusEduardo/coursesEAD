using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Simulacao.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public string StatusMessage { get; set; }
        [MaxLength(50)]
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
        public string UserPhoto { get; set; }
        [Display(Name = "Full Name")]
        [StringLength(200)]
        [Required]
        public string FullName { get; set; }
    }
}
