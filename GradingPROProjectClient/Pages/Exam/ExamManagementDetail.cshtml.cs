using GradingPROProjectClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GradingPROProjectClient.Pages.Exam
{
    public class ExamManagementDetailModel : BasePageModelModel
    {
        public ExamManagementDetailModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(httpClientFactory, httpContextAccessor, logger)
        {
        }

        [BindProperty]
        public Exam Exam { get; set; } = new Exam();
        public string FormMode { get; set; } = "view";


        public async Task<IActionResult> OnGetAsync(int examId, string mode = "view")
        {
            try
            {
                FormMode = mode;
                if (examId != 0)
                {
                    var response = await _httpClient.GetAsync("Exam/teacher/" + examId);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
                        if (result.Success)
                        {
                            if (result.Data != null)
                            {
                                Exam = JsonConvert.DeserializeObject<Exam>(result.Data.ToString());
                            }
                        }
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
