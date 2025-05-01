using WEB.Data;

namespace WEB.Models
{
    public class Template
    {
        //шаблон формы
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        //public string Topic { get; set; }
        //public string ImageUrl { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       // public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Связи
       // public string AuthorId { get; set; }
        //public ApplicationUser Author { get; set; }

        //public ICollection<Form> Forms { get; set; } = new List<Form>();
        public List<Question> Questions { get; set; } = new();
        public string AuthorId { get; internal set; }
        //        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        //      public ICollection<Like> Likes { get; set; } = new List<Like>();
        //    public ICollection<TemplateAccess> Accesses { get; set; } = new List<TemplateAccess>();
        //   public ICollection<TemplateTag> TemplateTags { get; set; } = new List<TemplateTag>();
    }
}
