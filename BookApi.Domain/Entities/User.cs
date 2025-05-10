namespace BookApi.Domain.Entities

{
    //user entity to match the user table in the database
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        //navigation property that relates to books
        public List<Book> Books { get; set; } = [];
    }
}