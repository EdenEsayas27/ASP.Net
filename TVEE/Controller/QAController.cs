
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tvee.Models;
using Tvee.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Tvee.Controllers
{
    [Route("api/qa")]
    [ApiController]
    public class QAController : ControllerBase
    {
        private readonly QAService _qaService;

        public QAController(QAService qaService)
        {
            _qaService = qaService;
        }

        // Post a new question
        [Authorize]
        [HttpPost("questions")]
        public async Task<IActionResult> PostQuestion([FromBody] QuestionDto questionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var question = new Question
            {
                Title = questionDto.Title,
                Content = questionDto.Content,
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                Answers = new List<Answer>() // Initialize with an empty list
            };

            var createdQuestion = await _qaService.PostQuestion(question);
            return Ok(createdQuestion);
        }

        // Post an answer to a question
        [Authorize]
        [HttpPost("questions/{questionId}/answers")]
        public async Task<IActionResult> PostAnswer(string questionId, [FromBody] AnswerDto answerDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var answer = new Answer
            {
                Id = Guid.NewGuid().ToString(), // Generate a new unique ID for the answer
                Content = answerDto.Content,
                UserId = userId,
                Timestamp = DateTime.UtcNow
            };

            var updatedQuestion = await _qaService.PostAnswer(questionId, answer);
            if (updatedQuestion == null)
            {
                return NotFound("Question not found.");
            }

            return Ok(updatedQuestion);
        }

        // Get a single question with its answers
        [HttpGet("questions/{questionId}")]
        public async Task<IActionResult> GetQuestionById(string questionId)
        {
            var question = await _qaService.GetQuestionById(questionId);
            if (question == null)
            {
                return NotFound("Question not found.");
            }

            var questionResponse = new
            {
                question.Id,
                question.Title,
                question.Content,
                question.UserId,
                question.Timestamp,
                Answers = question.Answers.Select(a => new
                {
                    a.Id,
                    a.Content,
                    a.UserId,
                    a.Timestamp
                }).ToList()
            };

            return Ok(questionResponse);
        }

        // Edit a question (only the user who posted it can edit)
        [Authorize]
        [HttpPut("questions/{questionId}")]
        public async Task<IActionResult> EditQuestion(string questionId, [FromBody] EditQuestionDto editDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var updatedQuestion = await _qaService.EditQuestion(questionId, userId, editDto.NewContent);
            if (updatedQuestion == null)
            {
                return NotFound("Question not found or you do not have permission to edit it.");
            }
            return Ok(updatedQuestion);
        }


        // Delete a question (only the user who posted it can delete)
        [Authorize]
        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(string questionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Invalid token.");
            }

            var deleted = await _qaService.DeleteQuestion(questionId, userId);
            if (!deleted)
            {
                return NotFound("Question not found or you do not have permission to delete it.");
            }
            return Ok("Question deleted successfully.");
        }
    }
}