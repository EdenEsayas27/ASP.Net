using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tvee.Services;
using Tvee.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Tvee.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestService _requestService;
        private readonly IUserService _userService;

        // Constructor to inject services
        public RequestController(RequestService requestService, IUserService userService)
        {
            _requestService = requestService;
            _userService = userService;
        }

        // Endpoint for sending a request (Students can send requests)
        [Authorize(Roles = "Student")] // Only students can access this endpoint
        [HttpPost("send")]
        public async Task<IActionResult> SendRequest([FromBody] SendRequestDTO sendRequestDTO)
        {
            // Validate the DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate that the sender is a student
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (senderId == null)
            {
                return Unauthorized("Invalid token.");
            }

            // Validate the receiver is a teacher
            var receiverUser = await _userService.GetUserById(sendRequestDTO.ReceiverId);
            if (receiverUser?.Role != "Teacher")
            {
                return BadRequest("The receiver must be a teacher.");
            }

            // Call service to create the request
            var request = await _requestService.SendRequest(senderId, sendRequestDTO.ReceiverId, sendRequestDTO.Title, sendRequestDTO.Description);
            return Ok(request);
        }

        // Endpoint for teachers to get their requests (Teachers can view requests)
        [Authorize(Roles = "Teacher")]
        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetRequestsForTeacher([FromRoute] string teacherId)
        {
            // Manually validate the teacherId parameter
            if (string.IsNullOrEmpty(teacherId))
            {
                return BadRequest("Teacher ID is required.");
            }

            // Validate that the teacherId from the token matches the provided teacherId
            var teacherIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (teacherIdFromToken == null)
            {
                return Unauthorized("Invalid token.");
            }

            // Optional: You could further validate that the provided teacherId matches the token's teacherId if needed
            if (teacherIdFromToken != teacherId)
            {
                return Unauthorized("You do not have access to this teacher's requests.");
            }

            var requests = await _requestService.GetRequestsForTeacher(teacherId);
            return Ok(requests);
        }

    //     [Authorize(Roles = "Teacher")]
    //     [HttpPost("read/{requestId}")]
    //     public async Task<IActionResult> MarkRequestAsRead([FromRoute] string requestId)
    //     {
    //         // Log the requestId for debugging
    //         Console.WriteLine($"Request ID: {requestId}");

    //         var teacherIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //         if (teacherIdFromToken == null)
    //         {
    //             return Unauthorized("Invalid token.");
    //         }

    //         // Call service to mark the request as read
    //         await _requestService.MarkRequestAsRead(requestId);
    //         return Ok("Request marked as read.");
    //     }
     }

    // DTO for sending requests
    public class SendRequestDTO
    {
        [Required]
        public string ReceiverId { get; set; }  // Teacher's ID

        [Required]
        [StringLength(100)]
        public string Title { get; set; }  // Title of the request

        [Required]
        public string Description { get; set; }  // Description of the request
    }
}
