using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizDB_API.Models;

/* 
    To access the API's operations, the URL would be constructed based on the route defined in the controller.
    In this case, the route is set as [Route("api/[controller]")], where [controller] is replaced by the name of the controller without the "Controller" suffix, which is "QuizUsers" in this case.
*/

[ApiController]
[Route("api/[controller]")]
public class QuizUsersController : ControllerBase
{
    private readonly QuizDbContext _context;

    public QuizUsersController(QuizDbContext context)
    {
        _context = context;
    }

    // Implement CRUD operations for QuizUser here

    // GET: api/QuizUsers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizUser>>> GetQuizUsers()
    {
        // Retrieve all QuizUsers from the database
        return await _context.QuizUsers!.ToListAsync();
    }

    // GET: api/QuizUsers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizUser>> GetQuizUser(int id)
    {
        // Retrieve a QuizUser by ID from the database
        var quizUser = await _context.QuizUsers!.FindAsync(id);

        if (quizUser == null)
        {
            return NotFound(); // If the QuizUser with the given ID is not found, return 404 Not Found status.
        }

        return quizUser;
    }

    // GET: api/QuizUsers/GetUserBy?username=xyz&firstname=abc&lastname=pqr
    [HttpGet("GetUserBy")]
    public async Task<ActionResult<IEnumerable<QuizUser>>> GetUserBy(
        string? username = null,
        string? firstname = null,
        string? lastname = null)
    {
        // Filter users based on the specified parameters (username, firstname, and/or lastname)

        // We start by creating a queryable variable that represents all the QuizUsers in the database.
        // We use IQueryable<T> to build our query before executing it, allowing us to apply filters based on the provided parameters.

        IQueryable<QuizUser> query = _context.QuizUsers!.AsQueryable();

        // Check if the 'username' parameter is provided and not empty. If it is, add a filter to the query to find users with matching usernames.
        if (!string.IsNullOrEmpty(username))
        {
            query = query.Where(u => EF.Functions.ILike(u.UserName!, username));
        }

        // Check if the 'firstname' parameter is provided and not empty. If it is, add a filter to the query to find users with matching first names.
        if (!string.IsNullOrEmpty(firstname))
        {
            query = query.Where(u => EF.Functions.ILike(u.FirstName!, firstname));
        }

        // Check if the 'lastname' parameter is provided and not empty. If it is, add a filter to the query to find users with matching last names.
        if (!string.IsNullOrEmpty(lastname))
        {
            query = query.Where(u => EF.Functions.ILike(u.LastName!, lastname));
        }

        // Execute the query and return the filtered list of users

        // Now that we have built our query with the necessary filters based on the provided parameters, we execute the query and retrieve the list of users that match the criteria.

        var users = await query.ToListAsync();

        // If there are no users that match the specified parameters, return a 404 Not Found status.
        if (users.Count == 0)
        {
            return NotFound();
        }

        // If users are found, return the list of matched users.
        return users;
    }

    // POST: api/QuizUsers
    [HttpPost]
    public async Task<ActionResult<QuizUser>> CreateQuizUser(QuizUser quizUser)
    {
        // Create a new QuizUser in the database
        _context.QuizUsers?.Add(quizUser);
        await _context.SaveChangesAsync();

        // Return the newly created QuizUser along with the route to access it (Location header in the response).
        return CreatedAtAction(nameof(GetQuizUser), new { id = quizUser.Id }, quizUser);
    }

    // PUT: api/QuizUsers/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuizUser(int id, QuizUser updatedQuizUser)
    {
        // Check if the provided ID matches the QuizUser's ID
        if (id != updatedQuizUser.Id)
        {
            return BadRequest(); // If IDs don't match, return 400 Bad Request status.
        }

        // Update the QuizUser in the database
        _context.Entry(updatedQuizUser).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // If the QuizUser with the given ID doesn't exist, return 404 Not Found status.
            if (!_context.QuizUsers!.Any(u => u.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // Return 204 No Content status if the update is successful.
    }

    // DELETE: api/QuizUsers/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuizUser(int id)
    {
        // Find the QuizUser by ID in the database
        var quizUser = await _context.QuizUsers!.FindAsync(id);
        if (quizUser == null)
        {
            return NotFound(); // If the QuizUser with the given ID doesn't exist, return 404 Not Found status.
        }

        // Remove the QuizUser from the database
        _context.QuizUsers.Remove(quizUser);
        await _context.SaveChangesAsync();

        return NoContent(); // Return 204 No Content status if the deletion is successful.
    }
}
