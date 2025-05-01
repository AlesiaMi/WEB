using WEB.Data;

namespace WEB.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TemplateId { get; set; }
        public Template Template { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
