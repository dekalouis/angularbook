// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System.Security.Claims;
// using BookApi.Domain.Entities;
// using BookApi.Infrastructure.Data;
// using BookApi.Dtos;  // ← Import the response DTO

// [ApiController]
// [Route("api/[controller]")]
// [Authorize]
// public class BookController : ControllerBase
// {
//     private readonly AppDbContext _context;

//     public BookController(AppDbContext context)
//     {
//         _context = context;
//     }

//     // GET /api/book
//     [HttpGet]
//     public async Task<ActionResult<List<BookResponseDto>>> GetMyBooks()
//     {
//         // int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//         //!EXTRACT USER ID FROM JWT CLAIM!!!
//         var claim = User.FindFirst(ClaimTypes.NameIdentifier);
//         if (claim == null) return Unauthorized();
//         int userId = int.Parse(claim.Value);


//         //Query only books that belong to this user and map output to DATA TRANSFER OBJECT
//         var books = await _context.Books
//             .Where(b => b.UserId == userId)
//             .Select(b => new BookResponseDto
//             {
//                 Id = b.Id,
//                 Title = b.Title,
//                 Author = b.Author,
//                 IsRead = b.IsRead
//             })
//             .ToListAsync();

//         return Ok(books);
//     }

//     // POST /api/book
//     [HttpPost]
//     public async Task<ActionResult<BookResponseDto>> AddBook(BookDto request)
//     {
//         //Get the current user ID
//         var claim = User.FindFirst(ClaimTypes.NameIdentifier);
//         if (claim == null) return Unauthorized();
//         int userId = int.Parse(claim.Value);

//         // int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

//         //Create new book that is linked to the user!!
//         // var book = new Book
//         // {
//         //     Title = request.Title,
//         //     Author = request.Author,
//         //     IsRead = false,
//         //     UserId = userId
//         // };

//         if (string.IsNullOrWhiteSpace(request.Title))
//             return BadRequest(new { message = "Title cannot be empty." });

//         if (string.IsNullOrWhiteSpace(request.Author))
//             return BadRequest(new { message = "Author cannot be empty." });

//         //! SO THAT IT IS PERMANENT
//         var user = await _context.Users.FindAsync(userId);
//         var book = new Book
//         {
//             Title = request.Title,
//             Author = request.Author,
//             IsRead = false,
//             UserId = userId,
//             User = user!  //trust it’s not null because FindAsync will return null only if user doesn't exist!
//         };

//         //dont forget to update DB after changes like the above
//         /*
//         dotnet ef migrations add UpdateBookUserRequired
//         dotnet ef database update

//         */

//         //SAVE TO DATABASE
//         _context.Books.Add(book);
//         await _context.SaveChangesAsync();

//         //Return  response CLEANN WITH NEW DTO
//         var response = new BookResponseDto
//         {
//             Id = book.Id,
//             Title = book.Title,
//             Author = book.Author,
//             IsRead = book.IsRead
//         };

//         return Ok(response);
//     }

//     // GET /api/book/{id}
//     //!!!GET BOOK BY ID
//     [HttpGet("{id}")]
//     public async Task<ActionResult<BookResponseDto>> GetBookById(int id)
//     {
//         var claim = User.FindFirst(ClaimTypes.NameIdentifier);
//         if (claim == null) return Unauthorized();
//         int userId = int.Parse(claim.Value);

//         var book = await _context.Books
//             .Where(b => b.Id == id && b.UserId == userId)
//             .Select(b => new BookResponseDto
//             {
//                 Id = b.Id,
//                 Title = b.Title,
//                 Author = b.Author,
//                 IsRead = b.IsRead
//             })
//             .FirstOrDefaultAsync();

//         if (book == null) return NotFound();

//         return Ok(book);
//     }


//     // PATCH /api/book/{id}/toggle 
//     //to toggle READ unREAD status
//     [HttpPatch("book/{id}/toggle")]
//     public async Task<ActionResult> ToggleRead(int id)
//     {
//         //Safely EXTRACT ID from JWT 
//         var claim = User.FindFirst(ClaimTypes.NameIdentifier);
//         if (claim == null) return Unauthorized();
//         int userId = int.Parse(claim.Value);

//         //Find books belonging to the user
//         var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
//         if (book == null) return NotFound();

//         //Toggle read status!!
//         book.IsRead = !book.IsRead;
//         await _context.SaveChangesAsync();

//         return Ok(new { message = "Toggled", isRead = book.IsRead });
//     }

//     // DELETE /api/book/{id}
//     [HttpDelete("{id}")]
//     public async Task<ActionResult> DeleteBook(int id)
//     {
//         //Safely EXTRACT ID from JWT 
//         var claim = User.FindFirst(ClaimTypes.NameIdentifier);
//         if (claim == null) return Unauthorized();
//         int userId = int.Parse(claim.Value);

//         //Find books belonging to the user
//         var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
//         if (book == null) return NotFound();

//         //Delete book
//         _context.Books.Remove(book);
//         await _context.SaveChangesAsync();

//         return Ok(new { message = "Book deleted successfully" });
//     }

//     // PUT /api/book/{id}
//     [HttpPut("{id}")]
//     public async Task<ActionResult<BookResponseDto>> UpdateBook(int id, BookDto request)
//     {
//         //Safely EXTRACT ID from JWT 
//         var claim = User.FindFirst(ClaimTypes.NameIdentifier);
//         if (claim == null) return Unauthorized();
//         int userId = int.Parse(claim.Value);

//         //Find books belonging to the user
//         var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
//         if (book == null) return NotFound();

//         //Update book details
//         book.Title = request.Title;
//         book.Author = request.Author;

//         await _context.SaveChangesAsync();

//         //Return updated book as response
//         var response = new BookResponseDto
//         {
//             Id = book.Id,
//             Title = book.Title,
//             Author = book.Author,
//             IsRead = book.IsRead
//         };

//         return Ok(response);
//     }



// }
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BookApi.Application.Features.Books.Commands;
using BookApi.Application.Features.Books.Queries;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new UnauthorizedAccessException();
        return int.Parse(claim.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyBooks()
    {
        var query = new GetUserBooksQuery { UserId = GetUserId() };
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var query = new GetBookByIdQuery { Id = id, UserId = GetUserId() };
        var book = await _mediator.Send(query);
        return book == null ? NotFound() : Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(CreateBookCommand command)
    {
        command.UserId = GetUserId();
        var newBookId = await _mediator.Send(command);
        return Ok(new { id = newBookId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, UpdateBookCommand command)
    {
        command.Id = id;
        command.UserId = GetUserId();
        var result = await _mediator.Send(command);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleRead(int id)
    {
        var command = new ToggleReadCommand { Id = id, UserId = GetUserId() };
        var result = await _mediator.Send(command);
        return result == null ? NotFound() : Ok(new { message = "Toggled", isRead = result });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var command = new DeleteBookCommand { Id = id, UserId = GetUserId() };
        var success = await _mediator.Send(command);
        return success ? Ok(new { message = "Deleted" }) : NotFound();
    }

}
