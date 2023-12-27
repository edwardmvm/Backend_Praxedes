using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Post
    {
        public int PostId { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
