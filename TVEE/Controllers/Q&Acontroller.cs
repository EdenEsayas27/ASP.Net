using Microsoft.AspNetCore.Mvc;
using TVEEAPI.Models;
using TVEEAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TVEEAPI.Helpers;

[ApiController]
[Route("api/questions")]
public class QAController : ControllerBase
{
    private static readonly List<Question> Questions = new();

    [HttpPost]
    [Authorize]
    public IActionResult AskQuestion([FromBody] Question question)
    {
        Questions.Add(question);
        return Ok(question);
    }

    [HttpGet]
    public IActionResult GetQuestions()
    {
        return Ok(Questions);
    }
}

public class Question
{
    public string User { get; set; }
    public string Content { get; set; }
}
