using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BookApi.Models;
using BookApi.Data;
using BookApi.Dtos;  // ← Import the response DTO

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/book
    [HttpGet]
    public async Task<ActionResult<List<BookResponseDto>>> GetMyBooks()
    {
        // int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //!EXTRACT USER ID FROM JWT CLAIM!!!
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized();
        int userId = int.Parse(claim.Value);


        //Query only books that belong to this user and map output to DATA TRANSFER OBJECT
        var books = await _context.Books
            .Where(b => b.UserId == userId)
            .Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                IsRead = b.IsRead
            })
            .ToListAsync();

        return Ok(books);
    }

    // POST /api/book
    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> AddBook(BookDto request)
    {
        //Get the current user ID
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized();
        int userId = int.Parse(claim.Value);

        // int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //Create new book that is linked to the user!!
        // var book = new Book
        // {
        //     Title = request.Title,
        //     Author = request.Author,
        //     IsRead = false,
        //     UserId = userId
        // };

        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title cannot be empty." });

        if (string.IsNullOrWhiteSpace(request.Author))
            return BadRequest(new { message = "Author cannot be empty." });

        //! SO THAT IT IS PERMANENT
        var user = await _context.Users.FindAsync(userId);
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            IsRead = false,
            UserId = userId,
            User = user!  //trust it’s not null because FindAsync will return null only if user doesn't exist!
        };

        //dont forget to update DB after changes like the above
        /*
        dotnet ef migrations add UpdateBookUserRequired
        dotnet ef database update

        */

        //SAVE TO DATABASE
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        //Return  response CLEANN WITH NEW DTO
        var response = new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsRead = book.IsRead
        };

        return Ok(response);
    }

    // PATCH /api/book/{id}/toggle 
    //to toggle READ unREAD status
    [HttpPatch("{id}/toggle")]
    public async Task<ActionResult> ToggleRead(int id)
    {
        //Safely EXTRACT ID from JWT 
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized();
        int userId = int.Parse(claim.Value);

        //Find books belonging to the user
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
        if (book == null) return NotFound();

        //Toggle read status!!
        book.IsRead = !book.IsRead;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Toggled", isRead = book.IsRead });
    }
}
