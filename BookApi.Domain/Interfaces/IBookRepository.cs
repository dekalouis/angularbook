using BookApi.Domain.Entities;

namespace BookApi.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksByUserIdAsync(int userId);
        Task<Book?> GetBookByIdAsync(int bookId, int userId);
        Task<int> CreateBookAsync(Book book);
        Task<Book?> UpdateBookAsync(Book book);
        Task<bool?> ToggleReadAsync(int bookId, int userId);
        Task<bool> DeleteBookAsync(int bookId, int userId);
    }
}
