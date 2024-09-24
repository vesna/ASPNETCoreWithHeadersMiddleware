using System.ComponentModel.DataAnnotations;

namespace ASPNETCoreWithHeadersMiddleware.DTOs
{
    public class Post
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
