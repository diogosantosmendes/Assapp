using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
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

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve conter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar password")]
        [Compare("Password", ErrorMessage = "A password não coincide.")]
        public string ConfirmPassword { get; set; }

        // *****************************************************************
        // User data required for this system
        [Required]
        [RegularExpression(@"[\w]+", ErrorMessage = "O {0} deve ter caracteres válidos")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[\w]+", ErrorMessage = "O {0} deve ter caracteres válidos")]
        [Display(Name = "Apelido")]
        public string Nickname { get; set; }

        [Required]
        [Phone]
        [StringLength(13, ErrorMessage = "Deve inserir um número válido", MinimumLength = 9)]
        [Display(Name = "Telemóvel")]
        public string Phone { get; set; }
        //********************************************************************
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
