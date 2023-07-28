using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizDB_API.Models;
/* 
    To access the API's "Get" operations, the URL would be constructed based on the route defined in the controller.
    In this case, the route is set as [Route("api/[controller]")], where [controller] is replaced by the name of the controller without the "Controller" suffix.
*/

[ApiController]
[Route("api/[controller]")]
public class QuizOptionsController : ControllerBase
{
    private readonly QuizDbContext _context;

    public QuizOptionsController(QuizDbContext context)
    {
        _context = context;
    }

    // Implement CRUD operations for QuizOption here
}
