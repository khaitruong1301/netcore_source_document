using api.models;
using api.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationContext _context;
    // Inject UserManager and IConfiguration for dependency injection
    public AuthController(ApplicationContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
    }

    // Register a new user
    [HttpPost("register")]
    [AllowAnonymous] // Allow access without authentication
    public async Task<IActionResult> Register([FromBody] UserViewModel userDto)
    {
        // Check if the user already exists
        var userExists = await _context.Users.SingleOrDefaultAsync(user => user.Email == userDto.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "User already exists!" });
        User userModel = new User();
        userModel.Email = userDto.Email;
        userModel.Username = userDto.Username;
        userModel.FullName = userDto.FullName;
        userModel.PasswordHash = PasswordHasher.HashPassword(userDto.Password);
        userModel.Alias = SlugHelper.StringToSlug(userDto.FullName);
        userModel.Role = Roles.Buyer;
        userModel.Phone = userDto.Phone;
        try
        {
            // Attempt to create the user with the provided password
            var result =  _context.Users.AddAsync(userModel);
            _context.SaveChanges();

            var res = await _context.UserRoles.AddAsync(new UserRole {UserId =userModel.Id,RoleId=Roles.Buyer});
            _context.SaveChanges();

        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });

        }
        return Ok(new { Message = "User created successfully!" });
    }


    // [HttpPost("register")]
    // [AllowAnonymous] // Allow access without authentication
    // public async Task<IActionResult> Register([FromBody] UserViewModel userDto)
    // {
    //     // Check if the user already exists
    //     var userExists = await _context.Users.SingleOrDefaultAsync(user => user.Email == userDto.Email);
    //     if (userExists != null)
    //         return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "User already exists!" });
    //     // Create a new IdentityUser

    //     // var user = new IdentityUser
    //     // {
    //     //     UserName = userDto.Username,
    //     //     Email = userDto.Email, // Assuming email is same as username for simplicity
    //     //     SecurityStamp = Guid.NewGuid().ToString() // Generate a new security stamp
    //     // };
    //     User userModel = new User();
    //     userModel.Email = userDto.Email;
    //     userModel.Username = userDto.Username;
    //     userModel.FullName = userDto.FullName;
    //     userModel.PasswordHash = PasswordHasher.HashPassword( userDto.Password);
    //     userModel.Alias = SlugHelper.StringToSlug(userDto.FullName);
    //     userModel.Role =  Roles.Buyer;
    //     userModel.Phone = userDto.Phone;
    //     try{
    //          // Attempt to create the user with the provided password
    //         var result = await _context.Users.AddAsync(userModel);
    //         _context.SaveChanges();
    //     }catch(Exception ex) {
    //         return BadRequest(new { Message = ex.Message });

    //     }


    //     return Ok(new { Message = "User created successfully!" });
    // }

    // User login
    [HttpPost("login")]
    [AllowAnonymous] // Allow access without authentication
    public async Task<IActionResult> Login([FromBody] UserLoginViewModel userDto)
    {
        // Find the user by username
        var userExists = await _context.Users.SingleOrDefaultAsync(user => user.Username == userDto.EmailOrUsername || user.Email == userDto.EmailOrUsername);
        if (userExists != null && PasswordHasher.VerifyPassword(userDto.Password, userExists.PasswordHash))
        {
            //Get roles
            var roles = _context.UserRoles.Where(item => item.UserId == userExists.Id).Select( n => new {Role = n.RoleId});
            // Define claims to be included in the JWT
            var authClaims = new[]
            {
                new Claim(ClaimTypes.Name, userExists.FullName),
                new Claim("email", userExists.Email),
                new Claim("userName", userExists.Username),
                new Claim("phone", userExists.Phone),
                new Claim("Roles", JSON.Stringify(roles)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create signing key for JWT
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Generate JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3), // Token expiration time
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            string s = JSON.Stringify(roles);
            // Return the generated token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        // If authentication fails, return Unauthorized
        return Unauthorized();
    }

    // GET: api/Auth/profile
    [HttpGet("profile")]
    [Authorize] // Require the user to be authenticated
    public async Task<IActionResult> GetProfile()
    {
        // Lấy thông tin người dùng từ claims
        var email = User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Email)?.Value;
        string email1 = Request.Headers["Authorization"];


        // Lấy token từ header
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // Khởi tạo JwtSecurityTokenHandler
        var handler = new JwtSecurityTokenHandler();

        // Giải mã token
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        // Lấy phần body (payload) của token
        var tokenPayload = jsonToken.Payload;

        // Convert payload sang định dạng JSON string (nếu cần)
        string jsonPayload = JsonSerializer.Serialize(tokenPayload);

        // Kiểm tra xem người dùng có tồn tại không
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }

        // Trả về thông tin cần thiết của người dùng
        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.FullName,
            user.Role,
            user.Phone
        });
    }



}
