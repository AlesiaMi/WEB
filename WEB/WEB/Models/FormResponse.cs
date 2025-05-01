using WEB.Data;

namespace WEB.Models
{
    public class FormResponse
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public Template Template { get; set; }
        public string RespondentId { get; set; }
        public ApplicationUser Respondent { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public List<Answer> Answers { get; set; } = new();
    }
}

