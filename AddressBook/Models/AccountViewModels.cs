using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class SessionDataModel
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userRole { get; set; }
       
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Temporary Password")]
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required.", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "First Name can be maximum 50 characters long.")]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last Name is required.", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Last Name can be maximum 50 characters long.")]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email Address is required.", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Email address can be maximum 50 characters long.")]
        /* [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Please provide a valid email address.")]*/
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}