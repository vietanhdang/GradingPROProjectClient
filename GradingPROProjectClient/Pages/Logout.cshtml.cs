using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GradingPROProjectClient.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            Response.Cookies.Delete("Grading-AccessToken");
            HttpContext.Session.Remove("Grading-User");
            Response.Redirect("/Login");
        }
    }
}
