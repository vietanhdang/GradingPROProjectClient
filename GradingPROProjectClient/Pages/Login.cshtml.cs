using GradingPROProjectClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GradingPROProjectClient.Pages
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^\S+@fpt\.edu\.vn$", ErrorMessage = "Email must be a valid @fpt.edu.vn address.")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password not empty")]
        public string? Password { get; set; }
    }
    public class LoginModel : BasePageModelModel
    {
        public LoginModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(httpClientFactory, httpContextAccessor, logger)
        {
        }

        [BindProperty]
        public LoginDTO Model { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                else
                {
                    var response = await _httpClient.PostAsJsonAsync("Account/Login", Model);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
                        if (result.Success)
                        {
                            if (result.Data != null)
                            {
                                var user = JsonConvert.DeserializeObject<UserInfo>(result.Data.ToString());
                                var cookieOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                                };
                                Response.Cookies.Append("Grading-AccessToken", user.Token, cookieOptions);
                                // Response.Cookies.Append("Grading-User", result.Data.ToString(), cookieOptions);
                                return RedirectToPage("/Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("ErrorMessage", result.Message);
                            return Page();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorMessage", "Invalid login attempt.");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { message = ex.Message, title = "Error" });
            }
            return Page();
        }
    }
}
