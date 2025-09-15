using System.Text.Json;

namespace vision_api_sample_post.Models
{
    public class FunctionDef
    {
        public string name { get; set; }
        public string preview { get; set; }
        public JsonElement request { get; set; }
    }
}
