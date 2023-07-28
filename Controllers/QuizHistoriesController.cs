using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizDB_API.Models;

/* 
   To access the API's "Get" operations, the URL would be constructed based on the route defined in the controller.
   In this case, the route is set as [Route("api/[controller]")], where [controller] is replaced by the name of the controller without the "Controller" suffix.

   For example, for this controller, the base URL would be "https://yourdomain.com/api/QuizHistories"
*/

[ApiController]
[Route("api/[controller]")]
public class QuizHistoriesController : ControllerBase
{
    private readonly QuizDbContext _context;

    public QuizHistoriesController(QuizDbContext context)
    {
        _context = context;
    }

    // GET: api/QuizHistories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizHistory>>> GetQuizHistories()
    {
        // Returns a list of all QuizHistory records
        return await _context.QuizHistories!.ToListAsync();
    }

    // GET: api/QuizHistories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizHistory>> GetQuizHistory(int id)
    {
        // Returns the QuizHistory record with the specified 'id'
        var quizHistory = await _context.QuizHistories!.FindAsync(id);

        // If the record is not found, return 404 Not Found
        if (quizHistory == null)
        {
            return NotFound();
        }

        return quizHistory;
    }

    // POST: api/QuizHistories
    [HttpPost]
    public async Task<ActionResult<QuizHistory>> CreateQuizHistory(QuizHistory quizHistory)
    {
        // Adds a new QuizHistory record to the database
        _context.QuizHistories?.Add(quizHistory);
        await _context.SaveChangesAsync();

        // Returns the created record along with its unique id
        return CreatedAtAction(nameof(GetQuizHistory), new { id = quizHistory.Id }, quizHistory);
    }

    // PUT: api/QuizHistories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuizHistory(int id, QuizHistory quizHistory)
    {
        // If the id in the URL does not match the id in the quizHistory object, return 400 Bad Request
        if (id != quizHistory.Id)
        {
            return BadRequest();
        }

        // Updates the QuizHistory record with the specified 'id'
        _context.Entry(quizHistory).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        // Returns 204 No Content indicating successful update
        return NoContent();
    }

    // DELETE: api/QuizHistories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuizHistory(int id)
    {
        // Finds the QuizHistory record with the specified 'id'
        var quizHistory = await _context.QuizHistories!.FindAsync(id);

        // If the record is not found, return 404 Not Found
        if (quizHistory == null)
        {
            return NotFound();
        }

        // Removes the QuizHistory record from the database
        _context.QuizHistories.Remove(quizHistory);
        await _context.SaveChangesAsync();

        // Returns the deleted record
        return Ok(quizHistory);
    }
}
