using System.Text;
using vision_api_sample_post.Models;

namespace vision_api_sample_post.Service
{
    public class VisionAPIService
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public VisionAPIService(IConfiguration configuration)
        {
            _apiKey = configuration["GoogleVision:ApiKey"];
            _client = new HttpClient();
        }

        public async Task<string> AnalyzeImageAsync(string json)
        {
            var url = $"https://vision.googleapis.com/v1/images:annotate?key={_apiKey}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
