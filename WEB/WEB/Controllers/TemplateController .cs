using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB.Data;
using WEB.Models;
using WEB.Services;

namespace WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Template>>> GetTemplates()
        {
            return await _context.Templates
                .Include(t => t.Questions)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Template>> CreateTemplate(Template template)
        {
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTemplate), new { id = template.Id }, template);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetTemplate(int id)
        {
            var template = await _context.Templates
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null) return NotFound();
            return template;
        }
    }

    [Route("api/responses")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResponsesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<FormResponse>> SubmitResponse(FormResponse response)
        {
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();
            return Ok(response);
        }

        [HttpGet("template/{templateId}")]
        public async Task<ActionResult<List<FormResponse>>> GetResponsesForTemplate(int templateId)
        {
            return await _context.Responses
                .Where(r => r.TemplateId == templateId)
                .Include(r => r.Answers)
                .ToListAsync();
        }
    }
}
