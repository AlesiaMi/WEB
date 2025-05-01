using WEB.Data;

namespace WEB.Models
{
public class Answer
{
        public int Id { get; set; }
        public string Value { get; set; }
        // public int? IntValue { get; set; }
        // public bool? BoolValue { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

       // public int FormId { get; set; }
       // public Form Form { get; set; }
    }
}
