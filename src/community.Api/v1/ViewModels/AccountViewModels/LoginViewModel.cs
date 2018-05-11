using System.ComponentModel.DataAnnotations;

namespace community.Api.v1.ViewModels.AccountViewModels
{
    // TypeScript: login.interface.ts
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
