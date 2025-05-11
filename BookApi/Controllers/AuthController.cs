using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookApi.Infrastructure.Data;
using BookApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
//ensuring route is api/auth
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
        // Check if the email is already in use
        if (_context.Users.Any(u => u.Email == request.Email))
            // return BadRequest("Email already in use");
            return BadRequest(new { message = "Email already in use" });


        // Validate the password format (at least 6 characters )
        if (request.Password.Length < 6)
            // return BadRequest("Password must be at least 6 characters long");
            return BadRequest(new { message = "Password must be at least 6 characters long" });


        // if (!request.Password.Any(char.IsDigit))
        //     return BadRequest("Password must contain at least one number");



        //create and hash the password
        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        // Add the user to the database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // return Ok("User created");
        return Ok(new { message = "User created" });
        // return new JsonResult(new { message = "User created" }) { StatusCode = 200 };


    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto request)
    {
        //lookup user by the email first
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        //check if the user exists and if the password is correct
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");


        // Create a JWT token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email)
        };

        // Create the token using the secret key from appsettings.json
        var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: new SigningCredentials(

        //making sure that the secret key from appsettings.json is also long, has digits and symbols
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]!)),
        SecurityAlgorithms.HmacSha256)
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = jwt });
    }
}
