using GradingPROProjectClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GradingPROProjectClient.Pages.Exam
{
    public class ExamManagementResultModel : BasePageModelModel
    {
        public Exam Exam { get; set; } = new Exam();
        public ExamManagementResultModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(httpClientFactory, httpContextAccessor, logger)
        {
        }

        public async Task<IActionResult> OnGetAsync(int examId)
        {
            try
            {
                if (examId > 0)
                {
                    var response = await _httpClient.GetAsync("Exam/getstudentexam?examId=" + examId);
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
                _logger.LogError(ex.Message);
            }
            return Page();
        }
    }
}
