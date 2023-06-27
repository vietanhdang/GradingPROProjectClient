using GradingPROProjectClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GradingPROProjectClient.Pages.Exam
{
    public class ExamDetailModel : BasePageModelModel
    {
        public ExamDetailModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(httpClientFactory, httpContextAccessor, logger)
        {
        }

        public ExamStudentResponse? ExamStudent { get; set; } = null;
        public async Task<IActionResult> OnGetAsync(int? examDetailId)
        {
            try
            {
                var response = await _httpClient.GetAsync("Exam/" + examDetailId);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
                    if (result.Success)
                    {
                        if (result.Data != null)
                        {
                            ExamStudent = JsonConvert.DeserializeObject<ExamStudentResponse>(result.Data.ToString());
                            return Page();
                        }
                    }
                    else
                    {
                        return RedirectToPage("/Error");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }
            return Page();
        }
    }
}
