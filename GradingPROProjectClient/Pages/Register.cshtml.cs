using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GradingPROProjectClient.Pages
{
    public class ContactModel
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^\S+@fpt\.edu\.vn$", ErrorMessage = "Email must be a valid @fpt.edu.vn address.")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Code not empty")]
        public string? Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name not empty")]
        public string? Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password not empty")]
        public string? Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password not empty")]
        [Compare("Password", ErrorMessage = "Confirm Password not match")]
        public string? ConfirmPassword { get; set; }

        /// <summary>
        /// Student's phone number
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Student's address
        /// </summary>
        public string? Address { get; set; }
    }
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        public ContactModel Model { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
