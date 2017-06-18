using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppHarbor.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter username.")]
        [RegularExpression(@"^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Please enter valid username.")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter password")]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /**
         * just to show features of TestSystemService is allowed to register user with diffrent roles
         */
        [Display(Name= "If not checked will be sign in as Student")]
        public bool IsLector { get; set; }
    }
}