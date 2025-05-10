using BookApi.Application.Interfaces;
using BookApi.Domain.Entities;
using BookApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetBooksByUserIdAsync(int userId)
        {
            return await _context.Books
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int bookId, int userId)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == bookId && b.UserId == userId);
        }

        public async Task<int> CreateBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task<Book?> UpdateBookAsync(Book updatedBook)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == updatedBook.Id && b.UserId == updatedBook.UserId);
            if (book == null) return null;

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;

            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool?> ToggleReadAsync(int bookId, int userId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.UserId == userId);
            if (book == null) return null;

            book.IsRead = !book.IsRead;
            await _context.SaveChangesAsync();
            return book.IsRead;
        }

        public async Task<bool> DeleteBookAsync(int bookId, int userId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.UserId == userId);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
