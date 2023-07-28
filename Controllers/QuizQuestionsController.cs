using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizDB_API.Models;
/* 
    To access the API's "Get" operations, the URL would be constructed based on the route defined in the controller.
    In this case, the route is set as [Route("api/[controller]")], where [controller] is replaced by the name of the controller without the "Controller" suffix.
*/

[ApiController]
[Route("api/[controller]")]
public class QuizQuestionsController : ControllerBase
{
    private readonly QuizDbContext _context;

    public QuizQuestionsController(QuizDbContext context)
    {
        _context = context;
    }

    [HttpGet("CheckAnswer")]
    public async Task<ActionResult<QuizQuestion>> CheckAnswer(int id, char optionname)
    {
        if (id < 2 || optionname == '\0')
            return BadRequest(); 

        // Retrieve a specific quiz question by its ID
        var quizQuestion = await _context.QuizQuestions!.FindAsync(id);

        // If the quiz question with the specified ID is not found, return a 404 Not Found response
        if (quizQuestion == null)
            return NotFound();

        // Compare the retrieved answer name with the provided option name
        bool result = char.ToUpper(quizQuestion.OptionName) == char.ToUpper(optionname);

        // Return the JSON response
        return Ok(result);
    }

    // Implement CRUD operations for QuizQuestion here
}
