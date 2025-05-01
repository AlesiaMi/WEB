namespace WEB.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
       // public string Description { get; set; }
        public QuestionType Type { get; set; }
       // public int Order { get; set; } = 0;
        public bool IsRequired { get; set; }
        public int TemplateId { get; set; }
        public Template Template { get; set; }
        public List<string>? Options { get; set; }
        public List<Option> Option { get; set; } = new();

        // public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

    public enum QuestionType
    {
        
        Text,      
        Number,     
        MultipleChoice,       
        Checkbox,
        Date
    }
}