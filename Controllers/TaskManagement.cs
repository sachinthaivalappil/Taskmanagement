using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_app.Models; // Replace with your namespace

namespace MvcApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        

        [HttpGet("login")]
        public async Task<ActionResult<object>> ValidateUserLogin([FromBody] LoginRequest loginRequest)
        {
            // Validate the input parameters
            if (loginRequest == null || loginRequest.UserId <= 0 || string.IsNullOrEmpty(loginRequest.UserType) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid input parameters.");
            }

            // Check if the user_type is 'admin' or 'user' and query the appropriate table
            if (loginRequest.UserType.ToLower() == "admin")
            {
                // Query the admin_master table for admin login
                var admin = await _context.AdminMasters
                    .Where(a => a.AdminId == loginRequest.UserId && a.Password == loginRequest.Password)
                    .FirstOrDefaultAsync();

                if (admin == null)
                {
                    return NotFound("Invalid admin credentials.");
                }

                return Ok(new { UserType = "Admin", UserData = "Success" });
            }
            else if (loginRequest.UserType.ToLower() == "user")
            {
                // Query the user_master table for user login
                var user = await _context.UserMasters
                    .Where(u => u.UserId == loginRequest.UserId && u.Password == loginRequest.Password)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("Invalid user credentials.");
                }

                return Ok(new { UserType = "User", UserData = "Success" });
            }
            else
            {
                return BadRequest("Invalid user type. Please use 'admin' or 'user'.");
            }
        }
      

    [HttpGet("fetch-tasks")]
    public async Task<ActionResult<object>> GetTaskDetails([FromBody] TaskRequest request)
    {
        // Validate input parameters
        if (request == null || request.UserId <= 0 || string.IsNullOrEmpty(request.UserType))
        {
            return BadRequest("Invalid input parameters.");
        }

        if (request.UserType.ToLower() == "admin")
        {
           
           int AdminId = int.Parse(request.UserId.ToString());

        // Convert AssignedTo to int where possible
        var tasksAssignedByAdmin = await _context.TaskMasters
            .Where(t => t.AssignedBy == AdminId)  // Assuming assigned_by is stored as string
            .Join(_context.UserMasters, 
                task => task.AssignedTo,  // Join on AssignedTo field from TaskMasters
                user => user.UserId,  // Assuming UserId is stored as a string in UserMasters
                (task, user) => new
                {
                    task.TaskId,
                    task.TaskName,
                    task.TaskDcr,
                    task.Department,
                    task.AssignedTo,
                    user.Name,  // Assuming UserMaster has a UserName field
                    task.DateOfAssignment,
                    task.TaskProgress,
                    task.TaskTargetdate,
                    task.TaskStatus
                })
            .ToListAsync();
        // Join with UserMasters on the processed AssignedToInt
       




            if (!tasksAssignedByAdmin.Any())
            {
                return NotFound("No tasks found assigned by the admin.");
            }

            return Ok(new { UserType = "Admin", Tasks = tasksAssignedByAdmin });
        }
        // Logic for regular user
        else if (request.UserType.ToLower()  == "user")
        {


        int UserId = int.Parse(request.UserId.ToString());
        var tasksAssignedToUser = await _context.TaskMasters
            .Where(t => t.AssignedTo == UserId)  // Assuming assigned_by is stored as string
            .Join(_context.AdminMasters, 
                task => task.AssignedBy,  // Join on AssignedTo field from TaskMasters
                user => user.AdminId,  // Assuming UserId is stored as a string in UserMasters
                (task, user) => new
                {
                    task.TaskId,
                    task.TaskName,
                    task.TaskDcr,
                    task.Department,
                    task.AssignedTo,
                    user.Name,  // Assuming UserMaster has a UserName field
                    task.DateOfAssignment,
                    task.TaskProgress,
                    task.TaskTargetdate,
                    task.TaskStatus
                })
            .ToListAsync();



            if (!tasksAssignedToUser.Any())
            {
                return NotFound("No tasks found assigned to the user.");
            }

            return Ok(new { UserType = "User", Tasks = tasksAssignedToUser });
        }
        else
        {
            return BadRequest("Invalid user type. Use 'admin' or 'user'.");
        }
    }

    [HttpPost("Task-Assignment")]
        public async Task<IActionResult> PostTaskAssignment([FromBody] TaskAssignmentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid task assignment data.");
            }

            var task = new TaskMaster
            {
                TaskName = request.TaskName,
                TaskDcr = request.TaskDcr,
                Department = request.Department,
                AssignedTo = request.AssignedTo,
                AssignedBy = request.AssignedBy,
                DateOfAssignment = request.DateOfAssignment,
                TaskTargetdate = request.TaskTargetDate,
                TaskProgress = 0, // Default value for new task
                TaskStatus = "Assigned" // Default task status
            };

            _context.TaskMasters.Add(task);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTaskById), new { id = task.TaskId }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // This is a helper method to get a task by ID after creation.
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskMaster>> GetTaskById(int id)
        {
            var task = await _context.TaskMasters.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }
    
    }

}
public class LoginRequest
{
    public int UserId { get; set; }
    public string UserType { get; set; }
    public string Password { get; set; }
}

public class TaskRequest
{
    public int UserId { get; set; }
    public string UserType { get; set; }
}

public class TaskAssignmentRequest
{
    public string TaskName { get; set; }
    public string TaskDcr { get; set; }
    public string Department { get; set; }
    public int AssignedTo { get; set; }
    public int AssignedBy { get; set; }
    public DateOnly DateOfAssignment { get; set; }
    public DateOnly TaskTargetDate { get; set; }
}
