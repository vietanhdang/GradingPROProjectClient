

using GradingPROProjectClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GradingPROProjectClient.Pages.Exam
{
    public class Exam
    {
        public int ExamId { get; set; }
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Exam Name is required")]
        public string ExamName { get; set; }

        [Required(ErrorMessage = "Exam Code is required")]
        public string ExamCode { get; set; }
        public string? Password { get; set; }
        public string? QuestionFolder { get; set; }
        [Required(ErrorMessage = "Question File is required")]
        public IFormFile? QuestionFile { get; set; }
        public string? TestCaseFolder { get; set; }
        [Required(ErrorMessage = "Test Case File is required")]
        public IFormFile? TestCaseFile { get; set; }

        [Required(ErrorMessage = "Total Score is required")]
        public float? TotalScore { get; set; } = 10;
        public string? AnswerFolder { get; set; }
        public IFormFile? AnswerFile { get; set; }

        [Required(ErrorMessage = "Total Questions is required")]
        public int? TotalQuestions { get; set; }

        [Required(ErrorMessage = "Start Time is required")]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required")]
        public DateTime? EndTime { get; set; }

        public int Status { get; set; }

        public bool IsStudentTakeExam { get; set; } = false;

        public ICollection<ExamStudentCustomDTO>? ExamStudents { get; set; }

        public bool IsShowScore { get; set; }
    }
    public class ExamStudentCustomDTO
    {
        public int ExamStudentId { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? SubmitedTime { get; set; }
        public string? SubmitedFolder { get; set; }
        public int Status { get; set; }
        public string? MarkLog { get; set; }
        public float? Score { get; set; }
        public int? CountTimeSubmit { get; set; }
    }
    public class ExamManagementModel : BasePageModelModel
    {
        public ExamManagementModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(httpClientFactory, httpContextAccessor, logger)
        {
        }

        public List<Exam> Exams { get; set; } = null;


        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Exam/GetAllTeacherExam");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
                    if (result.Success)
                    {
                        if (result.Data != null)
                        {
                            Exams = JsonConvert.DeserializeObject<List<Exam>>(result.Data.ToString());
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
