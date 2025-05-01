using WEB.Data;

namespace WEB.Models
{
    public class TemplateAccess
    {
        public int TemplateId { get; set; }
        public Template Template { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
