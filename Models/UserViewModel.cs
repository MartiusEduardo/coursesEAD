using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Simulacao.Models {
    public class UserViewModel{
        public ApplicationUser ApplicationUser { get; set; }
        public List<IdentityUserRole<string>> UserRoles { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<Course> Courses { get; set; }
    }
}