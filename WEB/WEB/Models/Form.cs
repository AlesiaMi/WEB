using WEB.Data;

namespace WEB.Models
{
    public class Form
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public Template Template { get; set; }
        public string UserId { get; set; } // Кто заполнил
        public ApplicationUser User { get; set; }
        public DateTime FilledAt { get; set; }
        public ICollection<Answer> Answers { get; set; } // Ответы на вопросы    
        
    }       
}
