using Microsoft.EntityFrameworkCore;
using WEB.Data;
using WEB.Models;

namespace WEB.Services
{
    public class TemplateService
    {
        private readonly ApplicationDbContext _context;

        public TemplateService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Создание шаблона
        public async Task<Template> CreateTemplateAsync(Template template, string userId)
        {
            template.AuthorId = userId;
            template.CreatedAt = DateTime.UtcNow;
            //template.UpdatedAt = DateTime.UtcNow;

            _context.Templates.Add(template);
            await _context.SaveChangesAsync();

            return template;
        }

        // Редактирование шаблона
        public async Task<Template> EditTemplateAsync(int id, Template updatedData, string userId)
        {
            var template = await _context.Templates
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                throw new KeyNotFoundException("Template not found");

            // Только автор или админ может редактировать
            if (template.AuthorId != userId)
                throw new UnauthorizedAccessException("You don't have permission to edit this template");

            template.Title = updatedData.Title;
            template.Description = updatedData.Description;
            //template.Topic = updatedData.Topic;
            template.IsPublic = updatedData.IsPublic;
            //template.UpdatedAt = DateTime.UtcNow;

            _context.Templates.Update(template);
            await _context.SaveChangesAsync();

            return template;
        }

        // Удаление шаблона
        public async Task DeleteTemplateAsync(int id, string userId)
        {
            var template = await _context.Templates
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                throw new KeyNotFoundException("Template not found");

            if (template.AuthorId != userId)
                throw new UnauthorizedAccessException("You don't have permission to delete this template");

            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
        }

        // Получение шаблона по ID
        public async Task<Template> GetTemplateByIdAsync(int id)
        {
            var template = await _context.Templates
                .Include(t => t.Questions) // Загружаем связанные вопросы
                //.Include(t => t.Comments)  // Загружаем комментарии
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
            {
                throw new KeyNotFoundException("Template not found");
            }

            return template;
        }

        // Получение всех шаблонов
        public async Task<List<Template>> GetAllTemplatesAsync()
        {
            return await _context.Templates
                .Include(t => t.Questions)
                //.Include(t => t.Comments)
                .ToListAsync();
        }
    }
}
