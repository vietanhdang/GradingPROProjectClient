using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace GradingPROProjectClient.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Message { get; set; } = "Have you tried turning it off and on again?";

        public string Title { get; set; } = "Woops! <br>Something went wrong :(";

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string message, string title)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;
            }
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
            }
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}