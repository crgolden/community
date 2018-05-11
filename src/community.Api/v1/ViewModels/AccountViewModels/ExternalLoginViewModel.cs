using System.ComponentModel.DataAnnotations;

namespace community.Api.v1.ViewModels.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
