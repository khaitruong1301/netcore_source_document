using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.models;
using api.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public UsersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Where(u => !u.Deleted).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.Deleted)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserViewModel userViewModel)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid User ID." });
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            // Cập nhật các trường cụ thể
            user.Username = userViewModel.Username;
            user.FullName = userViewModel.FullName;
            user.PasswordHash = PasswordHasher.HashPassword(userViewModel.Password);
            user.Email = userViewModel.Email;
            user.Phone = userViewModel.Phone;
            user.UpdatedAt = DateTime.UtcNow;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(new { message = "User not found." });
                }
                else
                {
                    throw;
                }
            }
            return Ok(user);
        }



        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Deleted)
            {
                return NotFound();
            }

            user.Deleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // _context.Users.Remove(user);//Xoá thực sự

            return NoContent();
        }





        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(UserViewModel userViewModel)
        {
            var us = await _context.Users.SingleOrDefaultAsync(u => u.Email == userViewModel.Email || u.Username == userViewModel.Username);
            if (us != null)
            {
                return Conflict(new { message = "User with this email or username already exists." });
            }
            var user = new User
            {
                Username = userViewModel.Username,
                PasswordHash = PasswordHasher.HashPassword(userViewModel.Password),
                Email = userViewModel.Email,
                Phone = userViewModel.Phone,
                Role = Roles.Buyer
            };

            

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, userViewModel);
        }

        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {

            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == changePasswordViewModel.EmailOrUsername || user.Username == changePasswordViewModel.EmailOrUsername);
            if (user == null)
            {
                return NotFound(new { message = "Current password is incorrect." });
            }

            // Verify current password
            if (!PasswordHasher.VerifyPassword(changePasswordViewModel.CurrentPassword, user.PasswordHash))
            {
                return BadRequest(new { message = "Current password is incorrect." });
            }

            // Update to new password
            user.PasswordHash = PasswordHasher.HashPassword(changePasswordViewModel.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Password changed successfully." });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
