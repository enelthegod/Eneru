using Eneru.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eneru.Controllers
{
    public class AssistantController : Controller
    {
        private readonly IAssistantService _assistant;

        public AssistantController(IAssistantService assistant)
        {
            _assistant = assistant;
        }

        // GET /Assistant — shows chat page
        [HttpGet]
        public IActionResult Index() => View();

        // POST /Assistant/Ask — called by JavaScript, returns JSON
        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { error = "Message cannot be empty" });

            var response = await _assistant.AskAsync(request.Message);
            return Ok(new { response });
        }
    }

    // Receives JSON body: { "message": "..." }
    public class AskRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}