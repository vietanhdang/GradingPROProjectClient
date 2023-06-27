using GradingPROProjectClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GradingPROProjectClient.Pages.Exam
{
    public class ExamStudentResponse
    {

        public int ExamId { get; set; }


        public int StudentId { get; set; }

        /// <summary>
        /// Start time
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Submit time
        /// </summary>
        public DateTime? SubmitedTime { get; set; }

        /// <summary>
        /// Submit folder
        /// </summary>
        public string? SubmitedFolder { get; set; }

        /// <summary>
        /// Student's status
        /// 0: Not submit
        /// 1: Start
        /// 2: Submitted
        /// 3: Submitted late
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Student's Log
        /// </summary>
        public string? MarkLog { get; set; }

        /// <summary>
        /// Student's score
        /// </summary>
        public float? Score { get; set; }

        /// <summary>
        /// Count time student submit exam
        /// </summary>
        public int? CountTimeSubmit { get; set; }

        /// <summary>
        /// Exam's name
        /// </summary>
        public string? ExamName { get; set; }

        /// <summary>
        /// Exam's code
        /// </summary>
        public string? ExamCode { get; set; }

        /// <summary>
        /// Exam Start time
        /// </summary>
        public DateTime? ExamStartTime { get; set; }

        /// <summary>
        /// Exam End time
        /// </summary>
        public DateTime? ExamEndTime { get; set; }

        /// <summary>
        /// Exam folder
        /// </summary>
        public string? ExamQuestionFolder { get; set; }


        public int ExamStatus { get; set; }

        public int ExamStatusPermission
        {
            get
            {
                if (ExamStatus == 0)
                {
                    return 0;
                }
                else if (ExamStatus == 2 || (ExamStartTime != null && ExamEndTime != null && ExamStartTime.Value <= DateTime.Now && ExamEndTime.Value < DateTime.Now))
                {
                    // 2: Exam is over
                    return 2;
                }
                else if (ExamStatus == 1 || (ExamStartTime != null && ExamEndTime != null && ExamStartTime.Value <= DateTime.Now && ExamEndTime.Value > DateTime.Now))
                {
                    // 1: Exam is running
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class ExamRequestJoinDTO
    {
        /// <summary>
        /// Code of exam
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code not empty")]
        public string ExamCode { get; set; }

        /// <summary>
        /// Password of exam
        /// </summary>
        public string Password { get; set; }
    }
    public class IndexModel : BasePageModelModel
    {
        public IndexModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger logger) : base(httpClientFactory, httpContextAccessor, logger)
        {
        }

        public List<ExamStudentResponse> ExamStudents { get; set; } = null;
        public ExamRequestJoinDTO JoinExamDTO { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Exam/GetAllStudentExam");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
                    if (result.Success)
                    {
                        if (result.Data != null)
                        {
                            ExamStudents = JsonConvert.DeserializeObject<List<ExamStudentResponse>>(result.Data.ToString());
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
