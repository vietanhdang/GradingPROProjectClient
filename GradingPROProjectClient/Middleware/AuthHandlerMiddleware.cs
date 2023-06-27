
using GradingPROProjectClient.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace PROGradingAPI.Middleware
{
    public class AuthHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        protected readonly HttpClient _httpClient;
        public AuthHandlerMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44333/api/");
        }
        public async Task InvokeAsync(HttpContext context)
        {
            bool endRequest = false;
            try
            {
                var token = context.Request.Cookies["Grading-AccessToken"];
                // các trang được phép truy cập không cần token
                List<string> allowPages = new List<string> { "/Login", "/Logout", "/Error", "/Privacy", "/Register", "/ForgotPassword", "/ResetPassword" };
                if (!allowPages.Contains(context.Request.Path.Value))
                {
                    if (token == null)
                    {
                        context.Response.Redirect("/Login");
                    }
                    else
                    {
                        context.Items["AccessToken"] = token;
                        // check session Grading-User có tồn tại không
                        var user = context.Session.GetString("Grading-User");
                        if (user == null)
                        {
                            // nếu có token thì kiểm tra token có hợp lệ không
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var response = await _httpClient.GetAsync("Account/CheckToken");
                            if (response.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                context.Response.Redirect("/Login");
                            }
                            else if (response.IsSuccessStatusCode)
                            {
                                var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
                                if (result.Success)
                                {
                                    if (result.Data != null)
                                    {
                                        context.Session.SetString("Grading-User", result.Data.ToString());
                                        context.Items["UserInfo"] = JsonConvert.DeserializeObject<UserInfo>(result.Data.ToString());
                                    }
                                }
                                else
                                {
                                    context.Response.Redirect("/Login");
                                }
                            }
                        }
                        else
                        {
                            context.Items["UserInfo"] = JsonConvert.DeserializeObject<UserInfo>(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                endRequest = true;
                context.Response.Redirect("/Error?message=" + ex.Message + "&title=Internal Server Error");
            }
            if (!endRequest)
            {
                try
                {
                    await _next(context);
                }
                catch (UnauthorizedAccessException ex)
                {
                    if (ex != null)
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(ex.Data), Encoding.UTF8);
                    }
                }
            }
        }
    }
}
