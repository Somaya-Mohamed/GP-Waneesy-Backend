using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Classes
{
    public class AiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AiService> _logger;

        public AiService(HttpClient httpClient, ILogger<AiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.Timeout = TimeSpan.FromSeconds(90);  
        }

        public async Task<string?> GenerateCartoonAsync(string originalImageUrl)
        {
            try
            {
                _logger.LogInformation("Calling AI with URL: {Url}", originalImageUrl);

                var payload = new
                {
                    data = new[] { originalImageUrl }
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "https://nermeen28-second-anime-gan.hf.space/run/predict",
                    payload);

                _logger.LogInformation("AI Response Status: {Status}", response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("AI Error: {Error}", errorContent);
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<AiResponse>();

                if (result?.data == null || result.data.Count == 0)
                {
                    _logger.LogWarning("AI returned empty data");
                    return null;
                }

                var cartoonUrl = result.data.FirstOrDefault()?.ToString();
                _logger.LogInformation("AI Success - Cartoon URL: {Url}", cartoonUrl);

                return cartoonUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to call AI service");
                return null;
            }
        }


    }



    public class AiResponse
    {
        public List<object> data { get; set; } = new();
    }


}
