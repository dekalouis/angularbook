
//DTO to shape the output when GET books for frontend
namespace BookApi.Dtos;


public class BookResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public bool IsRead { get; set; }
}
