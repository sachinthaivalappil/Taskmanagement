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

        // GET: api/Products
        
        // GET: api/Products/5
        [HttpGet("login")]
        public async Task<ActionResult<object>> ValidateUserLogin(int user_id, string user_type, string password)
        {
            // Validate parameters
            if (user_id <= 0 || string.IsNullOrEmpty(user_type) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Invalid input parameters.");
            }

            // Check if the user_type is 'admin' or 'user' and query the appropriate table
            if (user_type.ToLower() == "admin")
            {
                // Query the admin_master table for admin login
                var admin = await _context.AdminMasters 
                    .Where(a => a.AdminId == user_id && a.Password == password)
                    .FirstOrDefaultAsync();

                if (admin == null)
                {
                    return NotFound("Invalid admin credentials.");
                }

                return Ok(new { UserType = "Admin", UserData = "Success" });
            }
            else if (user_type.ToLower() == "user")
            {
                // Query the user_master table for user login
                var user = await _context.UserMasters
                    .Where(u => u.UserId == user_id && u.Password == password)
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
        
    }
}
